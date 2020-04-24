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
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MachineLearning
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static PyObject IToPython(this object obj)
        {
            return ToPython(obj as dynamic);
        }

        /***************************************************/

        public static PyObject ToPython(this Tensor tensor)
        {
            return tensor.NumpyArray;
        }

        /***************************************************/

        public static PyObject ToPython(this IEnumerable<object> list, Type dtype = null)
        {
            return ToNumpyArray(list, dtype);
        }

        /***************************************************/

        public static PyObject ToPython(this IEnumerable<IEnumerable<object>> listOfLists, Type dtype = null)
        {
            return ToNumpyArray(listOfLists, dtype);
        }

        /***************************************************/

        public static PyObject ToPython(this LinearRegression model)
        {
            return model.SkLearnModel;
        }

        /***************************************************/

        public static PyObject ToPython(this MinMaxScaler scaler)
        {
            return scaler.SkLearnScaler;
        }

        public static PyObject ToPython(this StandardScaler scaler)
        {
            return scaler.SkLearnScaler;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static PyObject ToNumpyArray(this object collection, Type dtype)
        {
            if (dtype == null)
                dtype = typeof(double);
            List<object> args = new List<object> { collection };
            Dictionary<string, object> kwargs = new Dictionary<string, object>
            {
                { "dtype", dtype.ToDType() }
            };
            return Engine.MachineLearning.Compute.InvokeNumpy("array", args, kwargs);
        }

        /***************************************************/

        private static PyObject ToPython(this object obj)
        {
            return Engine.Python.Convert.IToPython(obj);
        }

        /***************************************************/
    }
}
