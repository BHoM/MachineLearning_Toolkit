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

        [Description("Fits a FeatureScaling to a FeatureTable: measures the mean and spread of every feature so they can all be brought onto one scale.\n" +
                     "Most machine learning methods compare samples by distance, so without this a length in millimetres beside an angle in radians would only ever look at the length. " +
                     "Which other steps suit the data is a question about what the features mean, so every other switch is off unless asked for. " +
                     "Features with no variation carry no information and are dropped, with a note naming them.")]
        [Input("featureTable", "The samples to fit the scaling to.")]
        [Input("standardise", "Subtract each feature's mean and divide by its standard deviation, so all features share one scale. On by default; turn it off only when the features already share a scale that means something.")]
        [Input("logTransform", "Take log(1 + x) of every value first. Suits non-negative magnitudes with a few very large values and a long tail of small ones. Every value must be zero or more.")]
        [Input("normaliseRows", "Scale each sample to unit length first, so samples group by the proportions between their features rather than by their size. Changes the answer more than any other switch.")]
        [Input("weights", "Optional multiplier per feature, applied after standardising. One counts a feature the same as the others, two counts it double, and zero leaves it out. Leave empty for all ones.")]
        [Output("scaling", "The fitted FeatureScaling, or null if the samples cannot be scaled; the error says why.")]
        public static FeatureScaling FitScaling(FeatureTable featureTable, bool standardise = true, bool logTransform = false, bool normaliseRows = false, List<double> weights = null)
        {
            if (featureTable == null)
            {
                Engine.Base.Compute.RecordError("Cannot fit a scaling to a null FeatureTable.");
                return null;
            }

            if (!featureTable.IsValid())
                return null;

            List<string> names = featureTable.FeatureNames;
            int n = featureTable.Samples.Count;
            int d = names.Count;

            if (weights != null && weights.Count != 0)
            {
                if (weights.Count != d)
                {
                    Engine.Base.Compute.RecordError($"There are {weights.Count} weights for {d} features. Give one weight per feature, or none.");
                    return null;
                }

                if (weights.Any(w => double.IsNaN(w) || double.IsInfinity(w) || w < 0))
                {
                    Engine.Base.Compute.RecordError("Every weight must be a finite number of zero or more.");
                    return null;
                }
            }

            double[,] staged = ScalingStage(featureTable.ToMatrix(), logTransform, normaliseRows, names);
            if (staged == null)
                return null;

            List<double> centre = new List<double>(d);
            List<double> scale = new List<double>(d);
            for (int j = 0; j < d; j++)
            {
                double sum = 0.0;
                for (int i = 0; i < n; i++)
                    sum += staged[i, j];
                double mean = sum / n;

                double squares = 0.0;
                for (int i = 0; i < n; i++)
                {
                    double delta = staged[i, j] - mean;
                    squares += delta * delta;
                }

                centre.Add(mean);
                // Population standard deviation, as scikit-learn's StandardScaler uses.
                scale.Add(Math.Sqrt(squares / n));
            }

            List<double> effectiveWeights = weights != null && weights.Count != 0 ? weights.ToList() : Enumerable.Repeat(1.0, d).ToList();

            // A feature is constant when its spread is negligible next to its own size, so the test means the same in any units.
            List<string> kept = new List<string>();
            List<string> constant = new List<string>();
            List<string> excluded = new List<string>();
            for (int j = 0; j < d; j++)
            {
                double reference = Math.Max(1.0, Math.Abs(centre[j]));
                if (scale[j] <= 1e-12 * reference)
                    constant.Add(names[j]);
                else if (effectiveWeights[j] == 0.0)
                    excluded.Add(names[j]);
                else
                    kept.Add(names[j]);
            }

            if (kept.Count == 0)
            {
                Engine.Base.Compute.RecordError("Every feature is constant or has a weight of zero, so nothing is left to scale.");
                return null;
            }

            if (constant.Count > 0)
                Engine.Base.Compute.RecordNote($"These features do not vary, carry no information and were dropped: {string.Join(", ", constant)}.");
            if (excluded.Count > 0)
                Engine.Base.Compute.RecordNote($"These features have a weight of zero and were dropped: {string.Join(", ", excluded)}.");

            return new FeatureScaling
            {
                Name = featureTable.Name,
                FeatureNames = names.ToList(),
                KeptFeatures = kept,
                Centre = centre,
                Scale = scale,
                Weights = effectiveWeights,
                Standardise = standardise,
                LogTransform = logTransform,
                NormaliseRows = normaliseRows,
            };
        }

        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        [Description("The two steps that come before a feature's mean and spread are measured: each sample scaled to unit length, then log(1 + x) of every value.")]
        [Input("x", "The samples, one row each.")]
        [Input("logTransform", "Take log(1 + x) of every value.")]
        [Input("normaliseRows", "Scale each sample to unit length first.")]
        [Input("featureNames", "Optional feature names, one per column, used to name a feature in an error.")]
        [Output("staged", "The samples after both steps, or null, with an error recorded, when the log transform meets a negative value.")]
        internal static double[,] ScalingStage(double[,] x, bool logTransform, bool normaliseRows, List<string> featureNames = null)
        {
            int n = x.GetLength(0);
            int d = x.GetLength(1);
            double[,] staged = new double[n, d];

            for (int i = 0; i < n; i++)
            {
                double factor = 1.0;
                if (normaliseRows)
                {
                    double squares = 0.0;
                    for (int j = 0; j < d; j++)
                        squares += x[i, j] * x[i, j];

                    // An all-zero row has no direction, so it stays at the origin.
                    double norm = Math.Sqrt(squares);
                    factor = norm > 0.0 ? 1.0 / norm : 1.0;
                }

                for (int j = 0; j < d; j++)
                {
                    double value = x[i, j] * factor;
                    if (logTransform)
                    {
                        if (value < 0.0)
                        {
                            string feature = featureNames != null && j < featureNames.Count ? $"'{featureNames[j]}'" : $"feature {j}";
                            Engine.Base.Compute.RecordError($"The log transform needs values of zero or more, but sample {i} has {x[i, j]} in {feature}. Take magnitudes first, or turn the log transform off.");
                            return null;
                        }
                        value = Math.Log(1.0 + value);
                    }
                    staged[i, j] = value;
                }
            }

            return staged;
        }

        /***************************************************/
    }
}
