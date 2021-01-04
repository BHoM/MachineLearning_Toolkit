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

using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace BH.Engine.MachineLearning.Base
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
            bool success = false;
            List<string> installedPackages = new List<string>();

            if (!run)
                return new Output<bool, List<string>> { Item1 = success, Item2 = installedPackages }; 

            // install the python toolkit if necessary
            if (!BH.Engine.Python.Query.IsPythonInstalled()) 
                BH.Engine.Python.Compute.InstallPythonToolkit(run, force);

            // the requirements.txt file is copied to the python home in the postbuil events - we only pick it up now at runtime to install the toolkit lazily
            string requirementsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BHoM", "Extensions", "Python", "src", "requirements.txt");

            // install from requirements.txt file
            Console.WriteLine("Installing requiremented packages..."); 
            Python.Compute.PipInstall($"-r {requirementsPath} -f https://download.pytorch.org/whl/torch_stable.html");

            // check if installed correctly
            string[] packages = File.ReadAllLines(requirementsPath);
            installedPackages = packages.Where(x => Python.Query.IsModuleInstalled(x)).ToList();

            // install pyBHoM
            Console.WriteLine("Installing MachineLearning_Toolkit...");
            string module = "MachineLearning_Toolkit";
            string mlPath = Path.Combine(Python.Query.EmbeddedPythonHome(), "src", module);
            Engine.Python.Compute.PipInstall($"-e {mlPath}", force: force);
            if (Python.Query.IsModuleInstalled(module))
                installedPackages.Add(module);

            success = true; 
            return new Output<bool, List<string>>() { Item1 = success, Item2 = installedPackages };
        }

        /*************************************/
    }
}

