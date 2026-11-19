/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.oM.Base.Attributes;
using System;
using System.ComponentModel;
using System.Linq;

namespace BH.Engine.MachineLearning
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        [Description("Eigenvalues and eigenvectors of a real symmetric matrix by cyclic Jacobi rotation, sorted by descending eigenvalue.\n" +
                     "Exact and fully deterministic, which matters in a UI that re-solves constantly, and accurate for the small eigenvalues a variance threshold has to judge. " +
                     "Each eigenvector's sign is fixed so its largest entry is positive, so a component never flips between two nearly identical inputs.")]
        [Input("matrix", "A square symmetric matrix. It is not modified.")]
        [Output("eigen", "Values holds the eigenvalues, largest first. Vectors holds the eigenvectors as its columns: Vectors[row, i] is entry row of the eigenvector for Values[i].")]
        internal static (double[] Values, double[,] Vectors) SymmetricEigen(double[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            int d = matrix.GetLength(0);
            if (matrix.GetLength(1) != d)
                throw new ArgumentException("The matrix must be square.", nameof(matrix));

            // Work on a copy: a is driven towards diagonal while v accumulates the rotations.
            double[,] a = (double[,])matrix.Clone();
            double[,] v = new double[d, d];
            for (int i = 0; i < d; i++)
                v[i, i] = 1.0;

            // Jacobi converges quadratically and a small matrix settles in a few sweeps.
            // The cap only guards against a non-symmetric matrix arriving by mistake.
            for (int sweep = 0; sweep < 50; sweep++)
            {
                double off = 0.0;
                for (int p = 0; p < d - 1; p++)
                    for (int q = p + 1; q < d; q++)
                        off += a[p, q] * a[p, q];

                if (off <= 1e-30)
                    break;

                for (int p = 0; p < d - 1; p++)
                {
                    for (int q = p + 1; q < d; q++)
                    {
                        if (Math.Abs(a[p, q]) < 1e-300)
                            continue;

                        // The rotation that zeroes a[p, q]. Taking the smaller root keeps the angle
                        // under 45 degrees, which stops rounding error building up across sweeps.
                        double theta = (a[q, q] - a[p, p]) / (2.0 * a[p, q]);
                        double t = theta == 0.0 ? 1.0 : Math.Sign(theta) / (Math.Abs(theta) + Math.Sqrt(theta * theta + 1.0));
                        double c = 1.0 / Math.Sqrt(t * t + 1.0);
                        double s = t * c;

                        JacobiRotate(a, v, d, p, q, c, s);
                    }
                }
            }

            double[] values = new double[d];
            for (int i = 0; i < d; i++)
                values[i] = a[i, i];

            return SortEigenDescending(values, v, d);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static void JacobiRotate(double[,] a, double[,] v, int d, int p, int q, double c, double s)
        {
            double app = a[p, p];
            double aqq = a[q, q];
            double apq = a[p, q];

            a[p, p] = c * c * app - 2.0 * s * c * apq + s * s * aqq;
            a[q, q] = s * s * app + 2.0 * s * c * apq + c * c * aqq;
            a[p, q] = 0.0;
            a[q, p] = 0.0;

            for (int i = 0; i < d; i++)
            {
                if (i != p && i != q)
                {
                    double aip = a[i, p];
                    double aiq = a[i, q];
                    a[i, p] = c * aip - s * aiq;
                    a[p, i] = a[i, p];
                    a[i, q] = s * aip + c * aiq;
                    a[q, i] = a[i, q];
                }

                double vip = v[i, p];
                double viq = v[i, q];
                v[i, p] = c * vip - s * viq;
                v[i, q] = s * vip + c * viq;
            }
        }

        /***************************************************/

        // Sorts by descending eigenvalue and makes each eigenvector's largest-magnitude entry positive.
        // An eigenvector and its negation are equally valid, so without this rule the sign is whatever the arithmetic produced.
        private static (double[] Values, double[,] Vectors) SortEigenDescending(double[] values, double[,] vectors, int d)
        {
            int[] order = Enumerable.Range(0, d).OrderByDescending(i => values[i]).ToArray();

            double[] sortedValues = new double[d];
            double[,] sortedVectors = new double[d, d];

            for (int i = 0; i < d; i++)
            {
                int from = order[i];
                sortedValues[i] = values[from];

                int largest = 0;
                for (int r = 1; r < d; r++)
                    if (Math.Abs(vectors[r, from]) > Math.Abs(vectors[largest, from]))
                        largest = r;

                double sign = vectors[largest, from] < 0.0 ? -1.0 : 1.0;

                for (int r = 0; r < d; r++)
                    sortedVectors[r, i] = sign * vectors[r, from];
            }

            return (sortedValues, sortedVectors);
        }

        /***************************************************/
    }
}
