/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

namespace BH.Engine.MachineLearning
{
    public static partial class Compute
    {
        [Description("MachineLearning_Toolkit\nMethod used to create the Python environment used to run all Python scripts within this toolkit.")]
        [Input("run", "Starts the installation of the toolkit if true. Stays idle otherwise.")]
        [Output("pythonEnvironment", "The MachineLearning_Toolkit Python environment.")]
        public static PythonEnvironment InstallPythonEnv_ML(bool run = false)
        {
            return BH.Engine.Python.Compute.InstallVirtualenv(
                name: Query.ToolkitName(),
                BH.oM.Python.Enums.PythonVersion.v3_7_9,
                localPackage: $@"C:\ProgramData\BHoM\Extensions\PythonCode\{Query.ToolkitName()}",
                run: run
            );
        }
    }
}
