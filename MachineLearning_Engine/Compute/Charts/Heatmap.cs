/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using System.Collections.Generic;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.MachineLearning.Charts
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static string PlotHeatmap(
            List<double> annualValues, 
            string savePath, 
            string title = null, 
            string unit = null, 
            List<double> vRange = null, 
            string cmap = "viridis", 
            string toneColor = "black", 
            bool invertY = true, 
            bool transparency = false,
            bool run = false
            )
        {
            if (!run)
                return null;

            return BH.Engine.MachineLearning.Base.Compute.Invoke(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace, 
                "Heatmap.heatmap", 
                annualValues, 
                savePath, 
                title, 
                unit, 
                vRange, 
                cmap, 
                toneColor, 
                invertY, 
                transparency
                ).ToString();
        }

        /*************************************/
    }
}

