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

        [Description("Rescale the data with standard scaler.")]
        [Input("x", "Data to be rescaled.")]
        [Output("scaler", "The estimated scaler for the standard scale transformation.")]
        public static StandardScaler StandardScaler(Tensor x)
        {
            PyObject scaler = BH.Engine.MachineLearning.Compute.Invoke("StandardScaler.fit", x);
            return new StandardScaler(scaler);
        }

        /*************************************/

        [Description("Rescale the data with a given standard scaler.")]
        [Input("scaler", "The given scaler for the standard scale transformation.")]
        [Input("x", "Data to be rescaled.")]
        [Output("rescaledX", "The rescaled data after the given standard scale transformation.")]
        public static Tensor Infer(StandardScaler scaler, Tensor x)
        {
            return new Tensor(BH.Engine.MachineLearning.Compute.Invoke("StandardScaler.infer", scaler, x));
        }

        /*************************************/
        [Description("Inverse transformation of data with the given scaler.")]
        [Input("scaler", "The standard scaler used for inverse transformation.")]
        [Input("x", "Data to be inverse-transformed.")]
        [Output("inversedX", "The inverse transformed data using the standard scaler.")]
        public static Tensor InferInverse(StandardScaler scaler, Tensor x)
        {
            return new Tensor(BH.Engine.MachineLearning.Compute.Invoke("StandardScaler.infer_inverse", scaler, x));
        }

        /*************************************/

    }
}
