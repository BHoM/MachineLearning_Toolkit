/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

namespace BH.Engine.MachineLearning.Structured
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Fields              ****/
        /*************************************/

        [PreviousVersion("4.1", "BH.Engine.MachineLearning.Query.Coefficients(BH.oM.MachineLearning.LinearRegression)")]
        [Description("Expose the attributes for the given regression model.")]
        [Input("model", "The regression model used for inference.")]
        [MultiOutput(0, "coefficients", "Estimated coefficients for the regression model. This is a 1D array of double.")]
        [MultiOutput(1, "intercept", "The independent term in the model.")]
        public static Output<Tensor, Tensor> Coefficients(IRegressionModel model)
        {
            Tensor coefficients = new Tensor(model.SkLearnModel.GetAttr("coef_"));
            Tensor intercept = new Tensor(model.SkLearnModel.GetAttr("intercept_")); 
            return new Output<Tensor, Tensor> { Item1 = coefficients, Item2 = intercept };
        }

        /*************************************/
    }
}

