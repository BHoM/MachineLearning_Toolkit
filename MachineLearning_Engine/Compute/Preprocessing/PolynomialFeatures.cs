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

namespace BH.Engine.MachineLearning.Preprocessing
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        [Description("Create a trnasformer for polynomial and interaction features of the data.")]
        [Input("degree", "The degree of the polynomial features. Default as 2.")]
        [Input("interactionOnly", "The option to generate only interaction features. Default as false.")]
        [Output("transformer", "The transformer for the polynomial and interaction features of the data.")]
        public static PolynomialFeatures PolynomialFeatures(int degree = 2, bool interaction_only = false)
        {
            PyObject transformer = BH.Engine.MachineLearning.Compute.Invoke("PolynomialFeatures.fit", degree, interaction_only);
            return new PolynomialFeatures(transformer);
        }

        /*************************************/

        [Description("Generate polynomial and interaction features of the data.")]
        [Input("transformer", "The transformer for the polynomial and interaction features of the given data.")]
        [Input("x", "Data to be transformed.")]
        [Output("transformedX", "The polynomial and interaction features of the data.")]
        public static Tensor Infer(PolynomialFeatures transformer, Tensor x)
        {
            return new Tensor(BH.Engine.MachineLearning.Compute.Invoke("PolynomialFeatures.infer", transformer, x));
        }

        /*************************************/
    }
}
