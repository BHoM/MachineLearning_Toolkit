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
using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.MachineLearning.Vision
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        [Description("Returns the type of object in the image, e.g. an apple")]
        [Input("imagePath", "The absolute path to the image to analyse")]
        [Input("gpu", "If true, computation is accelarated using a capable gpu")]
        [Output("probabilities", "The probabilities (score) for each class. Use the Query.ImageClasses() method to match ")]
        public static int RecogniseObject(string imagePath, bool gpu = false)
        {
            return BH.Engine.MachineLearning.Compute.Invoke("RecogniseObject.infer", imagePath, gpu).As<int>();
        }

        /*************************************/
    }
}
