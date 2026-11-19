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
    [Description("A fitted principal component analysis: the directions along which a set of samples varies most, strongest first.\n" +
                 "Fit it with Compute.PrincipalComponents, project samples onto the components with Modify.ApplyPrincipalComponents, and map them back with Modify.InversePrincipalComponents. " +
                 "Scale the features first with a FeatureScaling, or the features with the largest units will dominate the components.")]
    public class PrincipalComponents : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("The features the analysis was fitted on, in column order. A FeatureTable must have exactly these features, in this order, to be projected.")]
        public virtual List<string> FeatureNames { get; set; } = new List<string>();

        [Description("The mean of each feature over the fitted samples, one per feature in FeatureNames, subtracted before projecting.")]
        public virtual List<double> Mean { get; set; } = new List<double>();

        [Description("The kept components, strongest first. Each holds one loading per feature in FeatureNames: how much that feature contributes to the component.")]
        public virtual List<List<double>> Components { get; set; } = new List<List<double>>();

        [Description("The variance of the samples along each kept component, in the same order as Components.")]
        public virtual List<double> ExplainedVariance { get; set; } = new List<double>();

        [Description("The variance of the samples along every component, kept or not: the total the explained variances are a share of.")]
        public virtual double TotalVariance { get; set; } = 0.0;

        [Description("Whether projected samples are divided by each component's standard deviation, so every component has a variance of one.")]
        public virtual bool Whiten { get; set; } = false;

        /***************************************************/
    }
}
