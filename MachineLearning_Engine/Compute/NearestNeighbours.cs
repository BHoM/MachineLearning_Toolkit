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

namespace BH.Engine.MachineLearning
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        [Description("Finds the given number of nearest other rows to every row of a matrix, nearest first, with ties going to the lower row index.\n" +
                     "Exact, by comparing every pair: time grows with the square of the number of rows, but memory only with rows times neighbours, so no full distance matrix is built. " +
                     "A row is never its own neighbour.")]
        [Input("x", "The samples, one row each.")]
        [Input("count", "Neighbours to find for each row, between 1 and the number of rows minus one.")]
        [Output("neighbours", "Index[i, c] is the c-th nearest row to row i, and Distance[i, c] is its distance.")]
        internal static (int[,] Index, double[,] Distance) NearestNeighbours(double[,] x, int count)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));

            int n = x.GetLength(0);
            if (count < 1 || count > n - 1)
                throw new ArgumentOutOfRangeException(nameof(count), count, $"Need between 1 and {n - 1} neighbours for {n} samples.");

            int k = count;
            int[,] index = new int[n, k];
            double[,] distance = new double[n, k];
            double[] windowDistance = new double[k];
            int[] windowIndex = new int[k];

            for (int i = 0; i < n; i++)
            {
                for (int c = 0; c < k; c++)
                {
                    windowDistance[c] = double.MaxValue;
                    windowIndex[c] = -1;
                }

                for (int j = 0; j < n; j++)
                {
                    if (j == i)
                        continue;

                    double squared = Query.SquaredDistance(x, i, x, j);

                    // Strictly less, so an equal distance never displaces the lower index already held: j arrives in ascending order.
                    if (squared >= windowDistance[k - 1])
                        continue;

                    // Each row keeps a short sorted window rather than sorting every row, because k is small and n is not.
                    int position = k - 1;
                    while (position > 0 && windowDistance[position - 1] > squared)
                    {
                        windowDistance[position] = windowDistance[position - 1];
                        windowIndex[position] = windowIndex[position - 1];
                        position--;
                    }

                    windowDistance[position] = squared;
                    windowIndex[position] = j;
                }

                for (int c = 0; c < k; c++)
                {
                    index[i, c] = windowIndex[c];
                    distance[i, c] = Math.Sqrt(windowDistance[c]);
                }
            }

            return (index, distance);
        }

        /***************************************************/
    }
}
