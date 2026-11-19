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

        [Description("Maps component scores back to the features a principal component analysis was fitted on, so a result such as a cluster centre can be read in terms of the original features.\n" +
                     "Exact only when every component was kept. Otherwise whatever lay along the dropped components is gone, and a sample comes back as its nearest point in the space the kept components span.")]
        [Input("projected", "Component scores, with the analysis's components in order as features PC1, PC2 and so on, such as the output of Modify.ApplyPrincipalComponents.")]
        [Input("principalComponents", "The analysis the scores were projected with.")]
        [Output("featureTable", "A new FeatureTable with the fitted features, or null if the scores do not match the analysis.")]
        public static FeatureTable InversePrincipalComponents(this FeatureTable projected, PrincipalComponents principalComponents)
        {
            if (projected == null)
            {
                Engine.Base.Compute.RecordError("Cannot map a null FeatureTable back from components.");
                return null;
            }

            if (!projected.IsValid() || !principalComponents.IsValid())
                return null;

            List<string> expected = Query.ComponentNames(principalComponents.Components.Count);
            if (!projected.FeatureNames.SequenceEqual(expected))
            {
                Engine.Base.Compute.RecordError($"The analysis has components [{string.Join(", ", expected)}], but the FeatureTable has [{string.Join(", ", projected.FeatureNames)}].");
                return null;
            }

            return new FeatureTable
            {
                Name = projected.Name,
                FeatureNames = principalComponents.FeatureNames.ToList(),
                Samples = Convert.FromMatrix(principalComponents.InversePrincipalComponents(projected.ToMatrix())),
                Ids = projected.Ids == null ? new List<string>() : projected.Ids.ToList(),
                Groups = projected.Groups == null ? new List<string>() : projected.Groups.ToList(),
            };
        }

        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        [Description("Maps a matrix of component scores, one column per kept component, back to one column per fitted feature. The analysis must already have passed Query.IsValid.")]
        [Input("principalComponents", "A valid principal component analysis.")]
        [Input("z", "The scores, one row per sample, one column per kept component.")]
        [Output("x", "The samples in terms of the fitted features.")]
        internal static double[,] InversePrincipalComponents(this PrincipalComponents principalComponents, double[,] z)
        {
            int n = z.GetLength(0);
            int d = principalComponents.Mean.Count;
            int count = principalComponents.Components.Count;
            double[,] x = new double[n, d];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    double sum = 0.0;
                    for (int c = 0; c < count; c++)
                    {
                        double score = principalComponents.Whiten ? z[i, c] * Math.Sqrt(principalComponents.ExplainedVariance[c]) : z[i, c];
                        sum += score * principalComponents.Components[c][j];
                    }

                    x[i, j] = sum + principalComponents.Mean[j];
                }
            }

            return x;
        }

        /***************************************************/
    }
}
