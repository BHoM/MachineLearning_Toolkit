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
    [Description("A table of numbers for machine learning: one row per sample and one column per feature, with an optional id and group for each sample.\n" +
                 "Every method in the MachineLearning toolkit takes its data as a FeatureTable. Build one with Create.FeatureTable from numbers, from BHoM objects and their property names, or from a BHoM Table.")]
    public class FeatureTable : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("The name of each feature, one per column and in column order. Names must be unique.")]
        public virtual List<string> FeatureNames { get; set; } = new List<string>();

        [Description("The values, one list per sample. Each list holds one number per feature, in the order of FeatureNames.")]
        public virtual List<List<double>> Samples { get; set; } = new List<List<double>>();

        [Description("Optional identifier of each sample, such as the BHoM_Guid of the object it was read from. Either empty or one per sample.")]
        public virtual List<string> Ids { get; set; } = new List<string>();

        [Description("Optional group of each sample, such as the project or model it came from. Methods that hold samples back to score a model hold back whole groups, so the score is not inflated by near-copies of the training samples. Either empty or one per sample.")]
        public virtual List<string> Groups { get; set; } = new List<string>();

        /***************************************************/
    }
}
