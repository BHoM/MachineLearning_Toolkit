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

using Python.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MachineLearning
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static PyObject ToNumpy<T>(this IEnumerable<T> list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", Query.DType(typeof(T)) }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, kwargs);
        }

        /***************************************************/

        public static PyObject ToNumpy<T>(this IEnumerable<IEnumerable<T>> list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", Query.DType(typeof(T)) }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, null);
        }

        /***************************************************/

        public static PyObject ToNumpy<T>(this T[,] list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", Query.DType(typeof(T)) }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, null);
        }

        /***************************************************/

        public static PyObject ToNumpy<T>(this T[][] list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", Query.DType(typeof(T)) }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, null);
        }

        /***************************************************/

        public static PyObject ToNumpy<T>(this IEnumerable<float> list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", "float32" }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, kwargs);
        }

        /***************************************************/

        public static PyObject ToNumpy<T>(this IEnumerable<double> list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", "float64" }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, kwargs);
        }

        /***************************************************/

        public static PyObject ToNumpy<T>(this IEnumerable<decimal> list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", "float64" }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, kwargs);
        }

        /***************************************************/

        public static PyObject ToNumpy<T>(this IEnumerable<short> list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", "int16" }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, kwargs);
        }

        /***************************************************/

        public static PyObject ToNumpy<T>(this IEnumerable<int> list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", "int32" }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, kwargs);
        }

        /***************************************************/

        public static PyObject ToNumpy<T>(this IEnumerable<long> list)
        {
            List<object> args = new List<object> { list };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", "int64" }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, kwargs);
        }

        /***************************************************/
    }
}
