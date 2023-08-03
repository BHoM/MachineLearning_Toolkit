/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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


using BH.oM.Python;
using System.ComponentModel;

using BH.oM.Base.Attributes;
using BH.Engine.Python;
using BH.oM.Python.Enums;
using System.IO;

namespace BH.Engine.MachineLearning
{
    public static partial class Compute
    {
        [Description("MachineLearning_Toolkit\nMethod used to create the Python environment used to run all Python scripts within this toolkit.")]
        [Input("run", "Starts the installation of the toolkit if true. Stays idle otherwise.")]
        [Input("reinstall", "Reinstalls the toolkit if true.")]
        [Output("pythonEnvironment", "The MachineLearning_Toolkit Python environment.")]
        public static PythonEnvironment InstallPythonEnv_ML(bool run = false, bool reinstall = false)
        {
            if (!run)
                return null;

            // find out whether this environment already exists
            bool exists = Python.Query.VirtualEnvironmentExists(Query.ToolkitName());

            if (reinstall)
                Python.Compute.RemoveVirtualEnvironment(Query.ToolkitName());

            // obtain python version
            PythonVersion pythonVersion = PythonVersion.v3_7_9;

            // create virtualenvironment
            PythonEnvironment env = Python.Compute.VirtualEnvironment(version: pythonVersion, name: Query.ToolkitName(), reload: true);

            // return null if environment could not be created/loaded
            if (env == null)
                return null;

            // install packages if this is a reinstall, or the environment did not originally exist
            if (reinstall || !exists)
            {
                // install local package
                env.InstallPackageLocal(Path.Combine(Python.Query.DirectoryCode(), Query.ToolkitName()));
            }

            return env;
        }
    }
}

