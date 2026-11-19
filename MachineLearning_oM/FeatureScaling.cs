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

using BH.oM.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.oM.MachineLearning
{
    [Description("A fitted transform that brings every feature of a FeatureTable onto a common scale, so that no feature counts for more just because of its units.\n" +
                 "Fit it once with Compute.FitScaling, then apply the same transform to any FeatureTable with the same features using Modify.ApplyScaling, and map results back to the original units with Modify.InverseScaling.\n" +
                 "In order, the steps are: scale each sample to unit length (NormaliseRows), take log(1 + x) (LogTransform), subtract the mean and divide by the standard deviation (Standardise), then multiply by the weight. Features with no variation, or a weight of zero, are dropped.")]
    public class FeatureScaling : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("The features the transform was fitted on, in column order. A FeatureTable must have exactly these features, in this order, to be scaled.")]
        public virtual List<string> FeatureNames { get; set; } = new List<string>();

        [Description("The features that remain after scaling, in column order. Features that did not vary, or were given a weight of zero, are left out.")]
        public virtual List<string> KeptFeatures { get; set; } = new List<string>();

        [Description("The mean of each feature, one per feature in FeatureNames, measured after NormaliseRows and LogTransform. Subtracted when Standardise is on, and the value a dropped feature returns as when the transform is reversed.")]
        public virtual List<double> Centre { get; set; } = new List<double>();

        [Description("The standard deviation of each feature, one per feature in FeatureNames, measured after NormaliseRows and LogTransform. Divided by when Standardise is on.")]
        public virtual List<double> Scale { get; set; } = new List<double>();

        [Description("The multiplier applied to each feature after standardising, one per feature in FeatureNames. A weight of one counts a feature the same as the others; a weight of zero drops it.")]
        public virtual List<double> Weights { get; set; } = new List<double>();

        [Description("Whether each feature has its mean subtracted and is divided by its standard deviation, so all features share one scale.")]
        public virtual bool Standardise { get; set; } = true;

        [Description("Whether log(1 + x) is taken of every value first. Suits non-negative magnitudes with a few very large values and a long tail of small ones.")]
        public virtual bool LogTransform { get; set; } = false;

        [Description("Whether each sample is first scaled to unit length, so only the proportions between its features count and not its overall size.")]
        public virtual bool NormaliseRows { get; set; } = false;

        /***************************************************/
    }
}
