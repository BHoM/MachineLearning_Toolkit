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
using System.ComponentModel;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        [Description("Squared Euclidean distance between one row of a matrix and one row of another, over the columns of the first.\n" +
                     "Squared because most callers only compare distances, and the square root is the one expensive step in a loop that runs for every pair of samples. " +
                     "Every method measures distance through this one method, so results are summed in the same order everywhere.")]
        [Input("a", "The first matrix, one row per sample.")]
        [Input("rowA", "The row of the first matrix.")]
        [Input("b", "The second matrix, with at least as many columns as the first. May be the first matrix again.")]
        [Input("rowB", "The row of the second matrix.")]
        [Output("squaredDistance", "The sum of squared differences between the two rows.")]
        internal static double SquaredDistance(double[,] a, int rowA, double[,] b, int rowB)
        {
            int columns = a.GetLength(1);
            double sum = 0.0;
            for (int j = 0; j < columns; j++)
            {
                double delta = a[rowA, j] - b[rowB, j];
                sum += delta * delta;
            }

            return sum;
        }

        /***************************************************/
    }
}
