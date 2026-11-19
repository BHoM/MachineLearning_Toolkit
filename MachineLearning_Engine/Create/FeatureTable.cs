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

using BH.oM.Base;
using BH.oM.Base.Attributes;
using BH.oM.Data.Collections;
using BH.oM.MachineLearning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;

namespace BH.Engine.MachineLearning
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Creates a FeatureTable from numbers, one list per sample. Every sample must have one value per feature.")]
        [Input("samples", "One list of numbers per sample, each holding one value per feature in the same order.")]
        [Input("featureNames", "Optional name of each feature, in column order. Defaults to Feature 0, Feature 1, ... when left empty.")]
        [Input("ids", "Optional identifier of each sample. Leave empty, or give one per sample.")]
        [Input("groups", "Optional group of each sample, such as the project or model it came from. Leave empty, or give one per sample.")]
        [Output("featureTable", "The FeatureTable, or null if the numbers cannot be used; the error says why.")]
        public static FeatureTable FeatureTable(List<List<double>> samples, List<string> featureNames = null, List<string> ids = null, List<string> groups = null)
        {
            if (samples == null || samples.Count == 0)
            {
                Engine.Base.Compute.RecordError("Cannot create a FeatureTable with no samples.");
                return null;
            }

            if (featureNames == null || featureNames.Count == 0)
            {
                List<double> first = samples.FirstOrDefault(s => s != null);
                int count = first == null ? 0 : first.Count;
                featureNames = Enumerable.Range(0, count).Select(i => "Feature " + i).ToList();
            }

            FeatureTable featureTable = new FeatureTable
            {
                FeatureNames = featureNames.ToList(),
                Samples = samples.Select(s => s == null ? null : s.ToList()).ToList(),
                Ids = ids == null ? new List<string>() : ids.ToList(),
                Groups = groups == null ? new List<string>() : groups.ToList(),
            };

            return featureTable.IsValid() ? featureTable : null;
        }

        /***************************************************/

        [Description("Creates a FeatureTable from any objects, reading one feature from each named property. Works for objects from any discipline: name the properties that describe them.\n" +
                     "Property names may reach into nested objects with a dot, such as \"SectionProperty.Area\". Every named property must hold a number on every object.")]
        [Input("objects", "The objects to describe, one sample per object.")]
        [Input("propertyNames", "The properties to read as features, one feature per property, in column order.")]
        [Input("groupProperty", "Optional property whose value is each object's group, such as the project or model it came from. Leave empty for no groups.")]
        [Output("featureTable", "The FeatureTable, one sample per object, with each BHoM object's BHoM_Guid as its id; or null if a property is missing or not a number, which the error names.")]
        public static FeatureTable FeatureTable(List<IObject> objects, List<string> propertyNames, string groupProperty = "")
        {
            if (objects == null || objects.Count == 0)
            {
                Engine.Base.Compute.RecordError("Cannot create a FeatureTable from no objects.");
                return null;
            }

            if (propertyNames == null || propertyNames.Count == 0)
            {
                Engine.Base.Compute.RecordError("Name at least one property to read as a feature.");
                return null;
            }

            bool hasGroups = !string.IsNullOrWhiteSpace(groupProperty);
            List<List<double>> samples = new List<List<double>>();
            List<string> ids = new List<string>();
            List<string> groups = new List<string>();
            List<string> problems = new List<string>();
            int problemCount = 0;

            for (int i = 0; i < objects.Count; i++)
            {
                IObject obj = objects[i];
                if (obj == null)
                {
                    Engine.Base.Compute.RecordError($"Object {i} is null.");
                    return null;
                }

                List<double> sample = new List<double>();
                foreach (string property in propertyNames)
                {
                    object value = Engine.Base.Query.PropertyValue(obj, property);
                    double number;
                    if (TryToNumber(value, out number))
                    {
                        sample.Add(number);
                    }
                    else
                    {
                        problemCount++;
                        if (problems.Count < 5)
                            problems.Add($"object {i} '{property}' is {Describe(value)}");
                    }
                }
                samples.Add(sample);

                IBHoMObject bhomObject = obj as IBHoMObject;
                ids.Add(bhomObject != null ? bhomObject.BHoM_Guid.ToString() : i.ToString(CultureInfo.InvariantCulture));

                if (hasGroups)
                {
                    object group = Engine.Base.Query.PropertyValue(obj, groupProperty);
                    if (group == null)
                    {
                        Engine.Base.Compute.RecordError($"Object {i} has no value for the group property '{groupProperty}'.");
                        return null;
                    }
                    groups.Add(System.Convert.ToString(group, CultureInfo.InvariantCulture));
                }
            }

            if (problemCount > 0)
            {
                Engine.Base.Compute.RecordError($"Every named property must hold a number on every object, but {problemCount} do not: {string.Join("; ", problems)}{(problemCount > problems.Count ? $"; and {problemCount - problems.Count} more" : "")}.");
                return null;
            }

            return FeatureTable(samples, propertyNames, ids, groups);
        }

        /***************************************************/

        [Description("Creates a FeatureTable from a BHoM Table, such as one built from data read with the Excel or CSV adapters. Each row becomes a sample.")]
        [Input("table", "The table to read, one row per sample.")]
        [Input("featureColumns", "Optional columns to read as features, in column order. When left empty, every column whose cells are all numbers is used, except the id and group columns.")]
        [Input("idColumn", "Optional column holding each sample's identifier. Leave empty for no ids.")]
        [Input("groupColumn", "Optional column holding each sample's group, such as the project or model it came from. Leave empty for no groups.")]
        [Output("featureTable", "The FeatureTable, or null if a feature column is missing or holds a cell that is not a number, which the error names.")]
        public static FeatureTable FeatureTable(Table table, List<string> featureColumns = null, string idColumn = "", string groupColumn = "")
        {
            if (table == null || table.Data == null)
            {
                Engine.Base.Compute.RecordError("Cannot create a FeatureTable from a null table.");
                return null;
            }

            DataTable data = table.Data;
            if (data.Rows.Count == 0)
            {
                Engine.Base.Compute.RecordError("Cannot create a FeatureTable from a table with no rows.");
                return null;
            }

            foreach (string column in new[] { idColumn, groupColumn }.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                if (!data.Columns.Contains(column))
                {
                    Engine.Base.Compute.RecordError($"The table has no column '{column}'.");
                    return null;
                }
            }

            List<string> columns;
            if (featureColumns == null || featureColumns.Count == 0)
            {
                List<string> candidates = data.Columns.Cast<DataColumn>().Select(c => c.ColumnName)
                    .Where(c => c != idColumn && c != groupColumn).ToList();
                columns = candidates.Where(c => data.Rows.Cast<DataRow>().All(r => TryToNumber(r[c], out double _))).ToList();

                List<string> skipped = candidates.Except(columns).ToList();
                if (columns.Count == 0)
                {
                    Engine.Base.Compute.RecordError("No column of the table holds only numbers, so there is nothing to use as a feature.");
                    return null;
                }
                if (skipped.Count > 0)
                    Engine.Base.Compute.RecordNote($"These columns hold cells that are not numbers and were left out: {string.Join(", ", skipped)}.");
            }
            else
            {
                List<string> missing = featureColumns.Where(c => c == null || !data.Columns.Contains(c)).ToList();
                if (missing.Count > 0)
                {
                    Engine.Base.Compute.RecordError($"The table has no column for these features: {string.Join(", ", missing.Select(c => c ?? "(null)"))}.");
                    return null;
                }
                columns = featureColumns.ToList();
            }

            List<List<double>> samples = new List<List<double>>();
            List<string> ids = new List<string>();
            List<string> groups = new List<string>();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];
                List<double> sample = new List<double>();
                foreach (string column in columns)
                {
                    double number;
                    if (!TryToNumber(row[column], out number))
                    {
                        Engine.Base.Compute.RecordError($"Row {i} of column '{column}' is {Describe(row[column])}. Every feature cell must be a number.");
                        return null;
                    }
                    sample.Add(number);
                }
                samples.Add(sample);

                if (!string.IsNullOrWhiteSpace(idColumn))
                    ids.Add(System.Convert.ToString(row[idColumn], CultureInfo.InvariantCulture));
                if (!string.IsNullOrWhiteSpace(groupColumn))
                    groups.Add(System.Convert.ToString(row[groupColumn], CultureInfo.InvariantCulture));
            }

            FeatureTable featureTable = FeatureTable(samples, columns, ids, groups);
            if (featureTable != null)
                featureTable.Name = table.Name;

            return featureTable;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        // Numbers of any numeric type, and text that reads as a number regardless of the machine's culture.
        private static bool TryToNumber(object value, out double number)
        {
            number = double.NaN;
            if (value == null || value is DBNull || value is bool)
                return false;

            if (value is string)
                return double.TryParse((string)value, NumberStyles.Float, CultureInfo.InvariantCulture, out number);

            if (value is double || value is float || value is decimal || value is int || value is long || value is short
                || value is byte || value is sbyte || value is uint || value is ulong || value is ushort)
            {
                number = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
                return true;
            }

            return false;
        }

        /***************************************************/

        private static string Describe(object value)
        {
            if (value == null || value is DBNull)
                return "empty";

            return $"'{System.Convert.ToString(value, CultureInfo.InvariantCulture)}' ({value.GetType().Name})";
        }

        /***************************************************/
    }
}
