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

using BH.Engine.MachineLearning;
using BH.oM.MachineLearning;
using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.Engine.MachineLearning
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        [Description("Reads a csv file and loads it into a Tensor primitive.")]
        [Input("path", "The path to the csv file. If using a relative path, the root will be C:\\ProgramData\\BHoM")]
        [Input("separator", "The separator character of the csv, e,g, a tsv. Use \t for tabs.")]
        [MultiOutput(0, "headers", "A list of headers as string if headers are present in the csv. An empty list otherswise.")]
        [MultiOutput(1, "tensor", "A 2D tensor with dimensions (numberOfRow, numberOfColumns)")]
        public static Output<List<string>, Tensor> LoadCsv(string path, string separator = ",", bool hasHeaders = true)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BHoM", path);

            List<string> headers = new List<string>();
            if (!File.Exists(path))
            {
                Engine.Reflection.Compute.RecordError((new FileNotFoundException("", path)).Message);
                return Engine.Reflection.Create.Output<List<string>, Tensor>(headers, null);
            }

            char sep = separator.ToCharArray().First();
            using (StreamReader reader = new StreamReader(path))
            {
                List<string[]> matrix = new List<string[]>();
                // strip headers
                if (hasHeaders)
                    headers = reader.ReadLine().Split(sep).ToList();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(sep);
                    matrix.Add(values);
                }
                return Engine.Reflection.Create.Output(headers, Create.Tensor(matrix));
            }
        }

        /*************************************/
    }
}
