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

using System;
using System.IO;
using BH.Engine.Python;

namespace BH.PostBuild.MachineLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            string module;

            // install pillow
            module = "pillow";
            Console.WriteLine($"Installing {module}...");
            Compute.PipInstall(module);

            // install pymongo
            module = "pymongo";
            Console.WriteLine($"Installing {module}...");
            Compute.PipInstall(module);

            // install numpy
            module = "numpy";
            Console.WriteLine($"Installing {module}...");
            Compute.PipInstall(module);

            // install matplotlib
            module = "matplotlib";
            Console.WriteLine($"Installing {module}...");
            Compute.PipInstall(module);

            // install pandas
            module = "pandas";
            Console.WriteLine($"Installing {module}...");
            Compute.PipInstall(module);

            // install scikit-learn
            module = "scikit-learn";
            Console.WriteLine($"Installing {module}...");
            Compute.PipInstall(module);

            // install tensorflow
            module = "tensorflow";
            Console.WriteLine($"Installing {module}...");
            Compute.PipInstall(module, version: "2");

            // install tensorflow
            module = "jax";
            Console.WriteLine($"Installing {module}...");
            Compute.PipInstall(module);

            // install pytorch
            Console.WriteLine("Installing pytorch");
            Compute.PipInstall("torch", version: "1.4.0", findLinks: "https://download.pytorch.org/whl/torch_stable.html");
            Compute.PipInstall("torchvision", version: "0.5.0", findLinks: "https://download.pytorch.org/whl/torch_stable.html");

            // install pyBHoM
            Console.WriteLine("Installing MachineLearning_Engine...");
            string mlPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
            Compute.PipInstall($"-e {mlPath}");  // Note: The PostBuilds are run from the MachineLearning_PostBuild/bin/Debug
        }
    }
}
