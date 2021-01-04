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

using BH.Engine.Python;
using BH.oM.Geometry;
using BH.oM.MachineLearning;
using Python.Runtime;
using System.Collections.Generic;

namespace BH.Engine.MachineLearning.Vision
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static List<Polyline> FindContours(Tensor image, int level)
        {
            List<Polyline> polylines = new List<Polyline>();
            // returns a list of points as numpy arrays
            List<object> contours = (BH.Engine.MachineLearning.Base.Compute.Invoke(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace, "FindContours.infer", image, level).IFromPython()) as List<object>;
            foreach (List<List<double>> polyline in contours)
            {
                Polyline bhomPolyline = new Polyline();
                foreach(List<double> point in polyline)
                {
                    Point bhomPoint = new Point { X = point[0], Y = point[1] };
                    bhomPolyline.ControlPoints.Add(bhomPoint);
                }
                polylines.Add(bhomPolyline);
            }
            return polylines;
        }

        /*************************************/
    }
}

