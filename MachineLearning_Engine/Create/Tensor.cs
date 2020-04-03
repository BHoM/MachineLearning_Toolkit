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
using System.Collections.Generic;

namespace BH.Engine.MachineLearning
{
    public static partial class Create
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static Tensor<T> Tensor<T>(T[] array)
        {
            return new Tensor<T>(array.ToNumpy().Handle);
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(List<T> list1D)
        {
            return new Tensor<T>(list1D.ToNumpy().Handle);
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(IEnumerable<T> list1D)
        {
            return new Tensor<T>(list1D.ToNumpy().Handle);
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(IEnumerable<IEnumerable<T>> list2D)
        {
            return new Tensor<T>(list2D.ToNumpy().Handle);
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(T[,] array2D)
        {
            return new Tensor<T>(array2D.ToNumpy().Handle);
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(T[][] arrayJagged)
        {
            return new Tensor<T>(arrayJagged.ToNumpy().Handle);
        }

        /*************************************/
    }
}
