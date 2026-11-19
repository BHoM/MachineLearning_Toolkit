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
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Checks that a FeatureTable can be used by the MachineLearning methods: at least one sample and one feature, a unique non-empty name for every feature, one finite number per feature in every sample, and Ids and Groups either empty or one per sample.\n" +
                     "Records an error saying what is wrong when it cannot be used.")]
        [Input("featureTable", "The FeatureTable to check.")]
        [Output("isValid", "True if the FeatureTable can be used; false otherwise.")]
        public static bool IsValid(this FeatureTable featureTable)
        {
            if (featureTable == null)
            {
                Engine.Base.Compute.RecordError("Cannot check a null FeatureTable.");
                return false;
            }

            List<string> names = featureTable.FeatureNames;
            List<List<double>> samples = featureTable.Samples;

            if (names == null || names.Count == 0)
            {
                Engine.Base.Compute.RecordError("The FeatureTable has no features: it needs at least one feature name.");
                return false;
            }

            if (samples == null || samples.Count == 0)
            {
                Engine.Base.Compute.RecordError("The FeatureTable has no samples.");
                return false;
            }

            int blank = names.FindIndex(n => string.IsNullOrWhiteSpace(n));
            if (blank >= 0)
            {
                Engine.Base.Compute.RecordError($"Feature {blank} has no name. Every feature needs a name.");
                return false;
            }

            List<string> repeated = names.GroupBy(n => n).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (repeated.Count > 0)
            {
                Engine.Base.Compute.RecordError($"Feature names must be unique, but these appear more than once: {string.Join(", ", repeated)}.");
                return false;
            }

            for (int i = 0; i < samples.Count; i++)
            {
                List<double> sample = samples[i];

                if (sample == null)
                {
                    Engine.Base.Compute.RecordError($"Sample {i} is null.");
                    return false;
                }

                if (sample.Count != names.Count)
                {
                    Engine.Base.Compute.RecordError($"Sample {i} has {sample.Count} values, but there are {names.Count} features. Every sample needs one value per feature.");
                    return false;
                }

                for (int j = 0; j < sample.Count; j++)
                {
                    if (double.IsNaN(sample[j]) || double.IsInfinity(sample[j]))
                    {
                        Engine.Base.Compute.RecordError($"Sample {i} has no usable value for '{names[j]}' ({sample[j]}). Every value must be a finite number.");
                        return false;
                    }
                }
            }

            if (featureTable.Ids != null && featureTable.Ids.Count != 0 && featureTable.Ids.Count != samples.Count)
            {
                Engine.Base.Compute.RecordError($"There are {featureTable.Ids.Count} ids for {samples.Count} samples. Give one id per sample, or none.");
                return false;
            }

            if (featureTable.Groups != null && featureTable.Groups.Count != 0 && featureTable.Groups.Count != samples.Count)
            {
                Engine.Base.Compute.RecordError($"There are {featureTable.Groups.Count} groups for {samples.Count} samples. Give one group per sample, or none.");
                return false;
            }

            return true;
        }

        /***************************************************/
    }
}
