/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using BH.oM.Base.Attributes;
using BH.oM.MachineLearning;
using System.ComponentModel;
using System.Linq;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("The share of the samples' total variance that the kept components of a principal component analysis carry, between 0 and 1.")]
        [Input("principalComponents", "The fitted principal component analysis.")]
        [Output("ratio", "The kept share of the variance, or NaN for a null analysis.")]
        public static double ExplainedVarianceRatio(this PrincipalComponents principalComponents)
        {
            if (principalComponents == null || principalComponents.ExplainedVariance == null)
            {
                Engine.Base.Compute.RecordError("Cannot read the explained variance of a null principal component analysis.");
                return double.NaN;
            }

            return principalComponents.TotalVariance <= 0.0 ? 1.0 : principalComponents.ExplainedVariance.Sum() / principalComponents.TotalVariance;
        }

        /***************************************************/
    }
}
