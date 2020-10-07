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

using BH.Engine.MachineLearning;
using BH.oM.MachineLearning;
using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;
using Python.Runtime;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.Engine.MachineLearning.Structured
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        [Description("Finds the slope and the intercept of a linear function that best fits the given set of bidimensional data.")]
        [Input("x", "Training data as a list of 2-elements list.")]
        [Input("y", "Target values as a list of 2-elements list.")]
        [Output("model", "The ordinary least squares linear regression with the given data.")]
        public static LinearRegression LinearRegression(Tensor x, Tensor y)
        {
            PyObject model = BH.Engine.MachineLearning.Base.Compute.Invoke(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace, "LinearRegression.fit", x, y);
            return new LinearRegression(model);
        }

        /*************************************/

        [Description("Projects the given input using a linear regression model.")]
        [Input("model", "The linear regressor model used for inference.")]
        [Input("x", "Data to project on.")]
        [Output("y", "The projected output for the given data using the linear model.")]
        public static Tensor Infer(LinearRegression model, Tensor x)
        {
            return new Tensor(BH.Engine.MachineLearning.Base.Compute.Invoke(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace, "LinearRegression.infer", model, x));
        }

        /*************************************/
    }
}
