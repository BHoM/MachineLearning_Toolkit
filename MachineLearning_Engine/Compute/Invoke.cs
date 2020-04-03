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

using BH.oM.Vision;
using BH.oM.DeepLearning.Models;
using Python.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MachineLearning
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static PyObject Invoke(string method, Dictionary<string, object> args)
        {
            return BH.Engine.Python.Compute.Invoke(Query.pyCompute, method, args);
        }

        /***************************************************/

        public static PyObject InvokeNumpy(string method, IEnumerable<object> args = null, Dictionary<string, object> kwargs = null)
        {
            return BH.Engine.Python.Compute.Invoke(Query.numpy, method, args, kwargs);
        }

        /***************************************************/

        public static PyObject InvokeNumpy(string method, params object[] args)
        {
            return BH.Engine.Python.Compute.Invoke(Query.numpy, method, args, null);
        }

        /***************************************************/
    }
}
