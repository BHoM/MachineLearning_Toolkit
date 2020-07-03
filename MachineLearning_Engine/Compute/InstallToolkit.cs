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
using System;
using System.Collections.Generic;
using System.IO;

namespace BH.Engine.MachineLearning
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

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

            // install tensorflow
            string module = "tensorflow";
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
            module = "MachineLearningEngine";
            string mlPath = Path.Combine(Python.Query.EmbeddedPythonHome(), "src");
            Engine.Python.Compute.PipInstall($"-e {mlPath}", force: force);
            if (Python.Query.IsModuleInstalled(module))
                installed.Add(module);

            return new Output<bool, List<string>>() { Item1 = true, Item2 = installed };
        }

        /*************************************/
    }
}
