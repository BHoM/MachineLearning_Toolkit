/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

        public static SupportVectorRegression SupportVectorRegression(Tensor x, Tensor y, string kernel = "rbf", int degree = 3, double regularisation = 1.0)
        {
            PyObject model = BH.Engine.MachineLearning.Base.Compute.Invoke(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace, "SupportVectorRegression.fit", x, y, kernel, degree, regularisation);
            return new SupportVectorRegression(model);
        }

        /*************************************/

        public static Tensor Infer(SupportVectorRegression model, Tensor x)
        {
            return new Tensor(BH.Engine.MachineLearning.Base.Compute.Invoke(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace, "SupportVectorRegression.infer", model, x));
        }

        /*************************************/
    }
}


