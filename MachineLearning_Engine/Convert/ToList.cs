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

using BH.oM.MachineLearning;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace BH.Engine.MachineLearning
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Convert a Tensor to a list of data.")]
        [Input("tensor", "A Tensor to be converted.")]
        [Output("data", "A list of data contained in the Tensor.")]
        public static List<object> ToList(this Tensor tensor)
        {
            // TODO: For the moment we only provide data as double or int back to C#
            // It would be good to find a way to return different types
            // The obstacle is that Marshal.Copy does not work on generic types
            if (tensor.NumpyArray.IsIterable())
            {
                if (Query.DType(tensor) == typeof(int))
                    return ToListInt(tensor).Cast<object>().ToList();
                if (Query.DType(tensor) == typeof(double))
                    return ToListDouble(tensor).Cast<object>().ToList();
            }
            else
            {
                if (Query.DType(tensor) == typeof(int))
                    return new List<object> { ToInt(tensor) };
                if (Query.DType(tensor) == typeof(double))
                    return new List<object> { ToDouble(tensor) };
            }
            return null;
        }
        /***************************************************/
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        [Description("Convert a Tensor to a list of data.")]
        [Input("tensor", "A Tensor to be converted.")]
        [Output("data", "A list of data contained in the Tensor.")]
        private static List<double> ToListDouble(this Tensor tensor)
        {
            long ptr = tensor.NumpyArray.GetAttr("ctypes").GetAttr("data").As<long>();
            int size = tensor.Size();

            double[] array = new double[size];
            Marshal.Copy(new IntPtr(ptr), array, 0, array.Length);

            return array.ToList();
        }

        [Description("Convert a Tensor to a list of data.")]
        [Input("tensor", "A Tensor to be converted.")]
        [Output("data", "A list of data contained in the Tensor.")]
        private static List<int> ToListInt(this Tensor tensor)
        {
            long ptr = tensor.NumpyArray.GetAttr("ctypes").GetAttr("data").As<long>();
            int size = tensor.Size();

            int[] array = new int[size];
            Marshal.Copy(new IntPtr(ptr), array, 0, array.Length);

            return array.ToList();
        }

        [Description("Convert a Tensor to a list of data.")]
        [Input("tensor", "A Tensor to be converted.")]
        [Output("data", "A list of data contained in the Tensor.")]
        private static int ToInt(this Tensor tensor)
        {
            return System.Convert.ToInt32(tensor.NumpyArray.ToString());
        }

        [Description("Convert a Tensor to a list of data.")]
        [Input("tensor", "A Tensor to be converted.")]
        [Output("data", "A list of data contained in the Tensor.")]
        private static double ToDouble(this Tensor tensor)
        {
            return System.Convert.ToDouble(tensor.NumpyArray.ToString());
        }
        /***************************************************/
    }
}


