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
    public static partial class Query
    {
        /*************************************/
        /**** Public Fields              ****/
        /*************************************/

        [PreviousVersion("4.1", "BH.Engine.MachineLearning.Error(BH.oM.MachineLearning.LinearRegression, BH.oM.MachineLearning.Tensor, BH.oM.MachineLearningTensor)")]
        [PreviousVersion("4.1", "BH.Engine.MachineLearning.Error(BH.oM.MachineLearning.LogisticRegression, BH.oM.MachineLearning.Tensor, BH.oM.MachineLearningTensor)")]
        [Description("Finds the The coefficient of determination R^2 of the given regression model.")]
        [Input("model", "The regression model used for inference.")]
        [Input("x", "Training data as a list of 2-elements list.")]
        [Input("y", "Target values as a list of 2-elements list.")]
        [Output("r2", "The coefficient of determination R^2 of the prediction.")]
        public static Tensor Score(LinearRegression model, Tensor x, Tensor y)
        {
            return new Tensor(BH.Engine.MachineLearning.Base.Compute.Invoke(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace, model.GetType().Name.ToString() + ".score", model, x, y));
        }
    }
}

