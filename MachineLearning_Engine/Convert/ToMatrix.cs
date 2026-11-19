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

namespace BH.Engine.MachineLearning
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        [Description("Copies the samples of a FeatureTable into a rectangular array, one row per sample and one column per feature, for the numerical methods to work on.\n" +
                     "The FeatureTable must already have passed Query.IsValid.")]
        [Input("featureTable", "A valid FeatureTable.")]
        [Output("matrix", "The samples as an array of samples by features.")]
        internal static double[,] ToMatrix(this FeatureTable featureTable)
        {
            int rows = featureTable.Samples.Count;
            int columns = featureTable.FeatureNames.Count;

            double[,] matrix = new double[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    matrix[i, j] = featureTable.Samples[i][j];

            return matrix;
        }

        /***************************************************/
    }
}
