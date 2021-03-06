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

using Python.Runtime;
using System.Windows.Forms;
using System;
using System.Diagnostics;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /*************************************/
        /**** Public Fields               ****/
        /*************************************/

        public static PyObject Import(string module)
        {
            bool moduleInstalled = Python.Query.IsModuleInstalled(module);
            if (moduleInstalled)
                return Python.Query.Import(module);

            // if module is not installed ask for automatic installation
            string installMessage = $"Cannot import module {module}, because the MachineLearning_Toolkit is not installed. Do you want to install the module?";
            DialogResult confirmResult = MessageBox.Show(installMessage, "BHoM package manager", MessageBoxButtons.YesNoCancel);

            if (confirmResult == DialogResult.Yes)
            {
                if (!Python.Query.IsModuleInstalled("MachineLearning_Engine"))
                {
                    // InstallMachineLearningToolkit also installs python if it is not found
                    bool mlToolkitSuccess = Base.Compute.InstallMachineLearningToolkit(true).Item1;
                }
            }
            else
            {
                string errorMessage = $"Cannot import module {module} because no valid version of Python for the BHoM has been found.\n" +
                                       "Try installing Python and the MachineLearning_Toolkit using the Compute.InstallMachineLearningToolkit component.\n" +
                                       "If the installation process fails, pleae consider reporting a bug at " +
                                       "https://github.com/BHoM/MachineLearning_Toolkit/issues/new?labels=type%3Abug&template=00_bug.md";
                throw new Exception(errorMessage);
            }

            return Python.Query.Import(module);
        }

        /*************************************/
    }
}

