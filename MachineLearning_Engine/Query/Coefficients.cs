/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.oM.MachineLearning;
using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;
using BH.Engine.MachineLearning;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /*************************************/
        /**** Public Fields              ****/
        /*************************************/

        [Description("Expose the variables for the given regression model.")]
        [Input("model", "The linear regressor model used for inference")]
        [Output("")]
        public static Output<List<double>, double> Coefficients(LinearRegression model)
        {
            BH.Engine.Reflection.Compute.RecordNote(model.SkLearnModel.GetAttr("coef_").ToString());
            BH.Engine.Reflection.Compute.RecordNote(model.SkLearnModel.GetAttr("intercept_").ToString());
            List<double> coef = model.SkLearnModel.GetAttr("coef_").ToString().Trim(new Char[] { '[', ']' }).Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(x => double.Parse(x, System.Globalization.NumberStyles.Float)).ToList();
            double intercept = double.Parse(model.SkLearnModel.GetAttr("intercept_").ToString().Trim(new Char[] { '[', ']' }));
            return new Output<List<double>, double> { Item1 = coef, Item2 = intercept };
        }

        /*************************************/
    }
}
