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

using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace BH.Engine.MachineLearning
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        [Description("Install the python bindings of the Machine Learning Toolkit. This includes standard machine learning packages \n" +
            "as the BHoM methods developed in python. Note that this depends on the Python_Toolkit.")]
        [Input("run", "Starts the installation of the toolkit if true. Stays idle otherwise.")]
        [Input("force", "If the toolkit is already installed it forces a reinstall of all the packages. It does not force a reinstall of Python.")]
        [MultiOutput(0, "success", "True if installation is successful, false otherwise.")]
        [MultiOutput(1, "packages", "The list of succesfully installed packages")]
        public static Output<bool, List<string>> InstallMachineLearningToolkit(bool run = false, bool force = false)
        {
            if (!run)
                return new Output<bool, List<string>>();

            // basic modules (no additional parameters to pip install)
            List<string> modules = new List<string> { "pillow", "pymongo", "numpy", "matplotlib", "pandas", "scikit-learn" };
            List<string> installed = new List<string>();

            // install basic modules
            for (int i = 0; i < modules.Count; i++)
            {
                Console.WriteLine($"Installing {modules[i]}...");
                Engine.Python.Compute.PipInstall(modules[i], force: force);
                if (Python.Query.IsModuleInstalled(modules[i]))
                    installed.Add(modules[i]);
            }

            // install windrose
            string module = "windrose";
            Console.WriteLine($"Installing {module}...");
            Engine.Python.Compute.PipInstall("git+https://github.com/python-windrose/windrose");
            if (Python.Query.IsModuleInstalled(module))
                installed.Add(module);

            // install tensorflow
            module = "tensorflow";
            Console.WriteLine($"Installing {module}...");
            Engine.Python.Compute.PipInstall(module, force: force, version: "2");
            if (Python.Query.IsModuleInstalled(module))
                installed.Add(module);

            // install pytorch
            Console.WriteLine("Installing pytorch");
            module = "torch";
            Engine.Python.Compute.PipInstall(module, force: force, version: "1.4.0", findLinks: "https://download.pytorch.org/whl/torch_stable.html");
            if (Python.Query.IsModuleInstalled(module))
                installed.Add(module);

            // installing torchvision
            module = "torchvision";
            Engine.Python.Compute.PipInstall(module, force: force, version: "0.5.0", findLinks: "https://download.pytorch.org/whl/torch_stable.html");
            if (Python.Query.IsModuleInstalled(module))
                installed.Add(module);

            // install pyBHoM
            Console.WriteLine("Installing MachineLearning_Engine...");
            module = "MachineLearning_Engine";
            string mlPath = Path.Combine(Python.Query.EmbeddedPythonHome(), "src", "MachineLearning_Toolkit");
            Engine.Python.Compute.PipInstall($"-e {mlPath}", force: force);
            if (Python.Query.IsModuleInstalled(module))
                installed.Add(module);

            return new Output<bool, List<string>>() { Item1 = true, Item2 = installed };
        }

        /*************************************/
    }
}
