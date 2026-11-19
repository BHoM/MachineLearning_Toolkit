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

        [Description("Checks that a FeatureScaling can be applied: a unique name for every fitted feature, one finite centre, positive scale and non-negative weight per feature, and at least one kept feature, each of them one of the fitted features.\n" +
                     "Records an error saying what is wrong when it cannot be used.")]
        [Input("scaling", "The FeatureScaling to check.")]
        [Output("isValid", "True if the FeatureScaling can be applied; false otherwise.")]
        public static bool IsValid(this FeatureScaling scaling)
        {
            if (scaling == null)
            {
                Engine.Base.Compute.RecordError("Cannot check a null FeatureScaling.");
                return false;
            }

            List<string> names = scaling.FeatureNames;
            if (names == null || names.Count == 0 || names.Any(n => string.IsNullOrWhiteSpace(n)) || names.Distinct().Count() != names.Count)
            {
                Engine.Base.Compute.RecordError("The FeatureScaling needs a unique, non-empty name for every feature it was fitted on.");
                return false;
            }

            int d = names.Count;
            if (scaling.Centre == null || scaling.Scale == null || scaling.Weights == null
                || scaling.Centre.Count != d || scaling.Scale.Count != d || scaling.Weights.Count != d)
            {
                Engine.Base.Compute.RecordError($"The FeatureScaling needs one centre, one scale and one weight for each of its {d} features.");
                return false;
            }

            for (int j = 0; j < d; j++)
            {
                if (double.IsNaN(scaling.Centre[j]) || double.IsInfinity(scaling.Centre[j])
                    || double.IsNaN(scaling.Scale[j]) || double.IsInfinity(scaling.Scale[j]) || scaling.Scale[j] < 0
                    || double.IsNaN(scaling.Weights[j]) || double.IsInfinity(scaling.Weights[j]) || scaling.Weights[j] < 0)
                {
                    Engine.Base.Compute.RecordError($"Feature '{names[j]}' has an unusable centre, scale or weight. Centres must be finite, and scales and weights finite and not negative.");
                    return false;
                }
            }

            List<string> kept = scaling.KeptFeatures;
            if (kept == null || kept.Count == 0)
            {
                Engine.Base.Compute.RecordError("The FeatureScaling keeps no features, so there is nothing to scale.");
                return false;
            }

            List<string> unknown = kept.Where(k => !names.Contains(k)).ToList();
            if (unknown.Count > 0 || kept.Distinct().Count() != kept.Count)
            {
                Engine.Base.Compute.RecordError("Every kept feature must be one of the fitted features, listed once" + (unknown.Count > 0 ? $"; these are not: {string.Join(", ", unknown)}." : "."));
                return false;
            }

            foreach (int j in scaling.KeptColumns())
            {
                if ((scaling.Standardise && scaling.Scale[j] == 0) || scaling.Weights[j] == 0)
                {
                    Engine.Base.Compute.RecordError($"Feature '{names[j]}' is kept but has a scale or weight of zero, which would divide by zero or erase it.");
                    return false;
                }
            }

            return true;
        }

        /***************************************************/

        [Description("Checks that a principal component analysis can be applied: a unique name and a finite mean for every fitted feature, at least one component with one finite loading per feature, and one finite, non-negative variance per component, positive when whitening.\n" +
                     "Records an error saying what is wrong when it cannot be used.")]
        [Input("principalComponents", "The principal component analysis to check.")]
        [Output("isValid", "True if the analysis can be applied; false otherwise.")]
        public static bool IsValid(this PrincipalComponents principalComponents)
        {
            if (principalComponents == null)
            {
                Engine.Base.Compute.RecordError("Cannot check a null principal component analysis.");
                return false;
            }

            List<string> names = principalComponents.FeatureNames;
            if (names == null || names.Count == 0 || names.Any(n => string.IsNullOrWhiteSpace(n)) || names.Distinct().Count() != names.Count)
            {
                Engine.Base.Compute.RecordError("The analysis needs a unique, non-empty name for every feature it was fitted on.");
                return false;
            }

            int d = names.Count;
            List<double> mean = principalComponents.Mean;
            if (mean == null || mean.Count != d || mean.Any(m => double.IsNaN(m) || double.IsInfinity(m)))
            {
                Engine.Base.Compute.RecordError($"The analysis needs one finite mean for each of its {d} features.");
                return false;
            }

            List<List<double>> components = principalComponents.Components;
            List<double> variance = principalComponents.ExplainedVariance;
            if (components == null || components.Count == 0 || variance == null || variance.Count != components.Count)
            {
                Engine.Base.Compute.RecordError("The analysis needs at least one component, and one explained variance per component.");
                return false;
            }

            for (int c = 0; c < components.Count; c++)
            {
                if (components[c] == null || components[c].Count != d || components[c].Any(v => double.IsNaN(v) || double.IsInfinity(v)))
                {
                    Engine.Base.Compute.RecordError($"Component {c + 1} needs one finite loading for each of the {d} features.");
                    return false;
                }

                if (double.IsNaN(variance[c]) || double.IsInfinity(variance[c]) || variance[c] < 0 || (principalComponents.Whiten && variance[c] == 0))
                {
                    Engine.Base.Compute.RecordError($"Component {c + 1} has an unusable variance ({variance[c]}). Variances must be finite and not negative, and above zero when whitening.");
                    return false;
                }
            }

            return true;
        }

        /***************************************************/
    }
}
