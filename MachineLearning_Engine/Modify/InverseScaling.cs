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
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Maps a scaled FeatureTable back to the units its features arrived in, so a result such as a cluster centre can be read in its original terms.\n" +
                     "Every fitted feature comes back. A dropped feature returns as its mean, which for a feature that did not vary is its only value. " +
                     "Row normalisation cannot be undone, because the size it divided out is gone: with it on, samples return as proportions between features rather than amounts.")]
        [Input("scaled", "Samples on the common scale, with the scaling's kept features in the same order, such as the output of Modify.ApplyScaling.")]
        [Input("scaling", "The FeatureScaling the samples were scaled with.")]
        [Output("featureTable", "A new FeatureTable in the original units with every fitted feature, or null if the features do not match the scaling.")]
        public static FeatureTable InverseScaling(this FeatureTable scaled, FeatureScaling scaling)
        {
            if (scaled == null)
            {
                Engine.Base.Compute.RecordError("Cannot reverse the scaling of a null FeatureTable.");
                return null;
            }

            if (!scaled.IsValid() || !scaling.IsValid())
                return null;

            if (!scaled.FeatureNames.SequenceEqual(scaling.KeptFeatures))
            {
                Engine.Base.Compute.RecordError($"The scaling keeps [{string.Join(", ", scaling.KeptFeatures)}], but the FeatureTable has [{string.Join(", ", scaled.FeatureNames)}]. " +
                                                "Reverse the scaling of a FeatureTable with the kept features in the same order.");
                return null;
            }

            if (scaling.NormaliseRows)
                Engine.Base.Compute.RecordWarning("Row normalisation cannot be undone: the samples come back as proportions between features, not their original amounts.");

            return new FeatureTable
            {
                Name = scaled.Name,
                FeatureNames = scaling.FeatureNames.ToList(),
                Samples = Convert.FromMatrix(scaling.InverseScaling(scaled.ToMatrix())),
                Ids = scaled.Ids == null ? new List<string>() : scaled.Ids.ToList(),
                Groups = scaled.Groups == null ? new List<string>() : scaled.Groups.ToList(),
            };
        }

        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        [Description("Maps a matrix of scaled samples, one column per kept feature, back to the original units with one column per fitted feature. " +
                     "The FeatureScaling must already have passed Query.IsValid.")]
        [Input("scaling", "A valid FeatureScaling.")]
        [Input("z", "Scaled samples, one row each, one column per kept feature.")]
        [Output("x", "The samples in the original units, one column per fitted feature.")]
        internal static double[,] InverseScaling(this FeatureScaling scaling, double[,] z)
        {
            int n = z.GetLength(0);
            int d = scaling.FeatureNames.Count;
            int[] kept = scaling.KeptColumns();
            double[,] x = new double[n, d];

            for (int i = 0; i < n; i++)
            {
                // A dropped feature returns as its mean: for a feature that did not vary, its only value.
                for (int j = 0; j < d; j++)
                    x[i, j] = scaling.LogTransform ? Math.Exp(scaling.Centre[j]) - 1.0 : scaling.Centre[j];

                for (int c = 0; c < kept.Length; c++)
                {
                    int j = kept[c];
                    double value = z[i, c] / scaling.Weights[j];
                    if (scaling.Standardise)
                        value = value * scaling.Scale[j] + scaling.Centre[j];
                    x[i, j] = scaling.LogTransform ? Math.Exp(value) - 1.0 : value;
                }
            }

            return x;
        }

        /***************************************************/
    }
}
