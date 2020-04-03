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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<short> Data(this Tensor<short> tensor)
        {
            IntPtr ptr = tensor.Handle;
            int size = tensor.Size();

            short[] array = new short[size];
            Marshal.Copy(ptr, array, 0, array.Length);

            return array.ToList();
        }

        /***************************************************/

        public static List<int> Data(this Tensor<int> tensor)
        {
            IntPtr ptr = tensor.Handle;
            int size = tensor.Size();

            int[] array = new int[size];
            Marshal.Copy(ptr, array, 0, array.Length);

            return array.ToList();
        }

        /***************************************************/

        public static List<long> Data(this Tensor<long> tensor)
        {
            IntPtr ptr = tensor.Handle;
            int size = tensor.Size();

            long[] array = new long[size];
            Marshal.Copy(ptr, array, 0, array.Length);

            return array.ToList();
        }

        /***************************************************/

        public static List<float> Data(this Tensor<float> tensor)
        {
            IntPtr ptr = tensor.Handle;
            int size = tensor.Size();

            float[] array = new float[size];
            Marshal.Copy(ptr, array, 0, array.Length);

            return array.ToList();
        }

        /***************************************************/

        public static List<double> Data(this Tensor<double> tensor)
        {
            IntPtr ptr = tensor.Handle;
            int size = tensor.Size();

            double[] array = new double[size];
            Marshal.Copy(ptr, array, 0, array.Length);

            return array.ToList();
        }

        /***************************************************/
    }
}
