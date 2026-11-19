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
using BH.oM.MachineLearning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.Engine.MachineLearning
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Fits a principal component analysis to a FeatureTable: finds the directions along which the samples vary most, strongest first, and keeps the ones that matter.\n" +
                     "With a handful of features its main use is to remove the correlation between them and drop the directions the samples do not vary in at all, before clustering or another method. " +
                     "Scale the features first with a FeatureScaling, or the features with the largest units will dominate the components. " +
                     "Directions with essentially no variance are never kept, whatever is asked for.")]
        [Input("featureTable", "The samples to analyse, at least two.")]
        [Input("varianceThreshold", "Keep the fewest strongest components that together carry at least this share of the total variance, between 0 and 1. 0.99 keeps nearly everything while dropping noise; 1 keeps every component that is not degenerate. Ignored when componentCount is set.")]
        [Input("componentCount", "Keep exactly this many components instead of using the threshold, so the output has the same size every time. Zero to use the threshold. Fewer come back if the samples do not vary in that many directions.")]
        [Input("whiten", "Divide each projected component by its standard deviation, so every component has a variance of one. Useful before a method that assumes equal spread in every direction; it also magnifies whatever noise the weak components carry.")]
        [Output("principalComponents", "The fitted analysis, or null if the samples cannot be analysed; the error says why.")]
        public static PrincipalComponents PrincipalComponents(FeatureTable featureTable, double varianceThreshold = 0.99, int componentCount = 0, bool whiten = false)
        {
            if (featureTable == null)
            {
                Engine.Base.Compute.RecordError("Cannot analyse a null FeatureTable.");
                return null;
            }

            if (!featureTable.IsValid())
                return null;

            int n = featureTable.Samples.Count;
            int d = featureTable.FeatureNames.Count;
            if (n < 2)
            {
                Engine.Base.Compute.RecordError("Need at least two samples to measure how they vary.");
                return null;
            }

            if (componentCount < 0)
            {
                Engine.Base.Compute.RecordError("The component count cannot be negative. Use zero to choose the count by the variance threshold.");
                return null;
            }

            if (componentCount == 0 && !(varianceThreshold > 0.0 && varianceThreshold <= 1.0))
            {
                Engine.Base.Compute.RecordError("The variance threshold must be more than 0 and at most 1.");
                return null;
            }

            double[,] x = featureTable.ToMatrix();

            double[] mean = new double[d];
            for (int j = 0; j < d; j++)
            {
                double sum = 0.0;
                for (int i = 0; i < n; i++)
                    sum += x[i, j];
                mean[j] = sum / n;
            }

            // Sample covariance with denominator n - 1, as numpy and scikit-learn use.
            double[,] covariance = new double[d, d];
            for (int j = 0; j < d; j++)
            {
                for (int k = j; k < d; k++)
                {
                    double sum = 0.0;
                    for (int i = 0; i < n; i++)
                        sum += (x[i, j] - mean[j]) * (x[i, k] - mean[k]);

                    covariance[j, k] = sum / (n - 1);
                    covariance[k, j] = covariance[j, k];
                }
            }

            (double[] eigenvalues, double[,] eigenvectors) = SymmetricEigen(covariance);

            // Rounding can push a genuinely zero eigenvalue slightly negative.
            for (int c = 0; c < d; c++)
                if (eigenvalues[c] < 0.0)
                    eigenvalues[c] = 0.0;

            double total = eigenvalues.Sum();
            int keep = ComponentsToKeep(eigenvalues, total, componentCount > 0 ? 1.0 : varianceThreshold);
            if (componentCount > 0)
            {
                // A fixed count still never keeps a direction the samples do not vary in: its scores are all zero, and whitening would divide by zero.
                if (componentCount > keep)
                    Engine.Base.Compute.RecordWarning($"{componentCount} components were asked for, but the samples only vary in {keep} direction{(keep == 1 ? "" : "s")}, so {keep} {(keep == 1 ? "is" : "are")} kept.");
                keep = Math.Min(componentCount, keep);
            }

            PrincipalComponents result = new PrincipalComponents
            {
                Name = featureTable.Name,
                FeatureNames = featureTable.FeatureNames.ToList(),
                Mean = mean.ToList(),
                Components = Enumerable.Range(0, keep).Select(c => Enumerable.Range(0, d).Select(j => eigenvectors[j, c]).ToList()).ToList(),
                ExplainedVariance = eigenvalues.Take(keep).ToList(),
                TotalVariance = total,
                Whiten = whiten,
            };

            Engine.Base.Compute.RecordNote($"Kept {keep} of {d} components, carrying {result.ExplainedVarianceRatio():P1} of the variance.");
            return result;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        // The fewest leading components reaching the threshold. A direction carrying essentially no variance is never kept:
        // whitening it would divide by a number close to zero and turn rounding noise into a full axis.
        private static int ComponentsToKeep(double[] eigenvalues, double total, double threshold)
        {
            if (total <= 0.0)
                return 1;

            double floor = total * 1e-10;
            int usable = eigenvalues.Count(v => v > floor);
            if (usable == 0)
                return 1;

            if (threshold >= 1.0)
                return usable;

            double cumulative = 0.0;
            for (int c = 0; c < usable; c++)
            {
                cumulative += eigenvalues[c];
                if (cumulative / total >= threshold)
                    return c + 1;
            }

            return usable;
        }

        /***************************************************/
    }
}
