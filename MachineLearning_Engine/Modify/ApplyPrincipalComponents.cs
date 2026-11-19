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

        [Description("Projects a FeatureTable onto the components of a fitted principal component analysis, returning a new FeatureTable with one feature per kept component, named PC1, PC2 and so on.\n" +
                     "The FeatureTable must have the features the analysis was fitted on, in the same order.")]
        [Input("featureTable", "The samples to project.")]
        [Input("principalComponents", "The fitted analysis, from Compute.PrincipalComponents.")]
        [Output("projected", "A new FeatureTable holding each sample's score on every kept component, with its ids and groups; or null if the features do not match the analysis.")]
        public static FeatureTable ApplyPrincipalComponents(this FeatureTable featureTable, PrincipalComponents principalComponents)
        {
            if (featureTable == null)
            {
                Engine.Base.Compute.RecordError("Cannot project a null FeatureTable.");
                return null;
            }

            if (!featureTable.IsValid() || !principalComponents.IsValid())
                return null;

            if (!featureTable.FeatureNames.SequenceEqual(principalComponents.FeatureNames))
            {
                Engine.Base.Compute.RecordError($"The analysis was fitted on [{string.Join(", ", principalComponents.FeatureNames)}], but the FeatureTable has [{string.Join(", ", featureTable.FeatureNames)}]. " +
                                                "Project a FeatureTable with the same features in the same order.");
                return null;
            }

            return new FeatureTable
            {
                Name = featureTable.Name,
                FeatureNames = Query.ComponentNames(principalComponents.Components.Count),
                Samples = Convert.FromMatrix(principalComponents.ApplyPrincipalComponents(featureTable.ToMatrix())),
                Ids = featureTable.Ids == null ? new List<string>() : featureTable.Ids.ToList(),
                Groups = featureTable.Groups == null ? new List<string>() : featureTable.Groups.ToList(),
            };
        }

        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        [Description("Projects a matrix of samples, one column per fitted feature, onto the kept components. The analysis must already have passed Query.IsValid.")]
        [Input("principalComponents", "A valid principal component analysis.")]
        [Input("x", "The samples, one row each, one column per fitted feature.")]
        [Output("z", "The scores, one column per kept component.")]
        internal static double[,] ApplyPrincipalComponents(this PrincipalComponents principalComponents, double[,] x)
        {
            int n = x.GetLength(0);
            int d = principalComponents.Mean.Count;
            int count = principalComponents.Components.Count;
            double[,] z = new double[n, count];

            for (int i = 0; i < n; i++)
            {
                for (int c = 0; c < count; c++)
                {
                    List<double> component = principalComponents.Components[c];
                    double sum = 0.0;
                    for (int j = 0; j < d; j++)
                        sum += (x[i, j] - principalComponents.Mean[j]) * component[j];

                    z[i, c] = principalComponents.Whiten ? sum / Math.Sqrt(principalComponents.ExplainedVariance[c]) : sum;
                }
            }

            return z;
        }

        /***************************************************/
    }
}
