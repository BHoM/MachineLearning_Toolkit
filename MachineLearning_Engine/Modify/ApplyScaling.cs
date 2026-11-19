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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.Engine.MachineLearning
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Applies a fitted FeatureScaling to a FeatureTable, returning a new FeatureTable on the common scale with only the kept features.\n" +
                     "Use the same FeatureScaling for every FeatureTable that will be compared, so new samples go through exactly the transform the fitted ones did. " +
                     "The FeatureTable must have the features the scaling was fitted on, in the same order.")]
        [Input("featureTable", "The samples to scale.")]
        [Input("scaling", "The FeatureScaling to apply, from Compute.FitScaling.")]
        [Output("scaled", "A new FeatureTable holding the scaled samples and their ids and groups, or null if the features do not match the scaling.")]
        public static FeatureTable ApplyScaling(this FeatureTable featureTable, FeatureScaling scaling)
        {
            if (featureTable == null)
            {
                Engine.Base.Compute.RecordError("Cannot scale a null FeatureTable.");
                return null;
            }

            if (!featureTable.IsValid() || !scaling.IsValid())
                return null;

            if (!featureTable.FeatureNames.SequenceEqual(scaling.FeatureNames))
            {
                Engine.Base.Compute.RecordError($"The scaling was fitted on [{string.Join(", ", scaling.FeatureNames)}], but the FeatureTable has [{string.Join(", ", featureTable.FeatureNames)}]. " +
                                                "Scale a FeatureTable with the same features in the same order.");
                return null;
            }

            double[,] scaled = scaling.ApplyScaling(featureTable.ToMatrix());
            if (scaled == null)
                return null;

            return new FeatureTable
            {
                Name = featureTable.Name,
                FeatureNames = scaling.KeptFeatures.ToList(),
                Samples = Convert.FromMatrix(scaled),
                Ids = featureTable.Ids == null ? new List<string>() : featureTable.Ids.ToList(),
                Groups = featureTable.Groups == null ? new List<string>() : featureTable.Groups.ToList(),
            };
        }

        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        [Description("Applies a FeatureScaling to a matrix of samples, one column per fitted feature, returning one column per kept feature. " +
                     "The FeatureScaling must already have passed Query.IsValid and the matrix must have one column per fitted feature.")]
        [Input("scaling", "A valid FeatureScaling.")]
        [Input("x", "The samples, one row each, one column per fitted feature.")]
        [Output("scaled", "The scaled samples, one column per kept feature; or null, with an error recorded, if the log transform meets a negative value.")]
        internal static double[,] ApplyScaling(this FeatureScaling scaling, double[,] x)
        {
            double[,] staged = Compute.ScalingStage(x, scaling.LogTransform, scaling.NormaliseRows, scaling.FeatureNames);
            if (staged == null)
                return null;

            int n = staged.GetLength(0);
            int[] kept = scaling.KeptColumns();
            double[,] z = new double[n, kept.Length];

            for (int i = 0; i < n; i++)
            {
                for (int c = 0; c < kept.Length; c++)
                {
                    int j = kept[c];
                    double value = staged[i, j];
                    if (scaling.Standardise)
                        value = (value - scaling.Centre[j]) / scaling.Scale[j];
                    z[i, c] = value * scaling.Weights[j];
                }
            }

            return z;
        }

        /***************************************************/
    }
}
