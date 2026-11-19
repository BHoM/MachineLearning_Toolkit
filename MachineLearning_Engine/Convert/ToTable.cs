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
using BH.oM.Data.Collections;
using BH.oM.MachineLearning;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace BH.Engine.MachineLearning
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Converts a FeatureTable to a BHoM Table with one row per sample, ready to write out with the Excel or CSV adapters.\n" +
                     "The Table has an id column and a group column when the FeatureTable has ids and groups, then one column of numbers per feature.")]
        [Input("featureTable", "The FeatureTable to convert.")]
        [Input("idColumn", "Name of the column holding each sample's id. Used only when the FeatureTable has ids.")]
        [Input("groupColumn", "Name of the column holding each sample's group. Used only when the FeatureTable has groups.")]
        [Output("table", "The Table, or null if the FeatureTable is not valid or a column name is taken by a feature.")]
        public static Table ToTable(this FeatureTable featureTable, string idColumn = "Id", string groupColumn = "Group")
        {
            if (featureTable == null)
            {
                Engine.Base.Compute.RecordError("Cannot convert a null FeatureTable to a Table.");
                return null;
            }

            if (!featureTable.IsValid())
                return null;

            bool hasIds = featureTable.Ids != null && featureTable.Ids.Count > 0;
            bool hasGroups = featureTable.Groups != null && featureTable.Groups.Count > 0;

            List<string> extra = new List<string>();
            if (hasIds)
                extra.Add(idColumn);
            if (hasGroups)
                extra.Add(groupColumn);

            foreach (string column in extra)
            {
                if (string.IsNullOrWhiteSpace(column))
                {
                    Engine.Base.Compute.RecordError("The id and group columns need a name.");
                    return null;
                }

                if (featureTable.FeatureNames.Contains(column))
                {
                    Engine.Base.Compute.RecordError($"'{column}' is already the name of a feature. Give the id or group column another name.");
                    return null;
                }
            }

            if (hasIds && hasGroups && idColumn == groupColumn)
            {
                Engine.Base.Compute.RecordError("The id and group columns need different names.");
                return null;
            }

            DataTable data = new DataTable(featureTable.Name ?? "");
            if (hasIds)
                data.Columns.Add(idColumn, typeof(string));
            if (hasGroups)
                data.Columns.Add(groupColumn, typeof(string));
            foreach (string name in featureTable.FeatureNames)
                data.Columns.Add(name, typeof(double));

            for (int i = 0; i < featureTable.Samples.Count; i++)
            {
                DataRow row = data.NewRow();
                if (hasIds)
                    row[idColumn] = featureTable.Ids[i];
                if (hasGroups)
                    row[groupColumn] = featureTable.Groups[i];
                for (int j = 0; j < featureTable.FeatureNames.Count; j++)
                    row[featureTable.FeatureNames[j]] = featureTable.Samples[i][j];
                data.Rows.Add(row);
            }

            return new Table { Data = data, Name = featureTable.Name };
        }

        /***************************************************/
    }
}
