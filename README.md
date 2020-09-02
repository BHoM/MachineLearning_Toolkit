[![License: LGPL v3](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](https://www.gnu.org/licenses/lgpl-3.0)
## Install me
To install the MachineLearning_Toolkit:
1. Compile:  
    1. [BHoM](https://github.com/BHoM/BHoM)  
    1. [BHoM_Engine](https://github.com/BHoM/BHoM_Engine)  
    1. [BHoM_Adapter](https://github.com/BHoM/BHoM_Adapter)   
    1. [BHoM_UI](https://github.com/BHoM/BHoM_UI)  
	  1. [Rhinoceros_Toolkit](https://github.com/BHoM/Rhinoceros_Toolkit)  
	  1. [Grasshopper_Toolkit](https://github.com/BHoM/Grasshopper_Toolkit)  
	  1. [Python_Toolkit](https://github.com/BHoM/Python_Toolkit)
    1. MachineLearning_Toolkit (this repo)
1. Open a UI of your choice (e.g. Grasshopper)
1. Run the `BH.Engine.Python.Compute.InstallPythonToolkit` component and wait for the installation to finish.
1. The installation has succeeded if the install packages include:
	  - Python 3.7
  	- jupyter
  	- matplotlib
  	- Python_Toolkit
1. Restart your UI - if using Grasshopper, restart Rhinoceros and Grasshopper
1. Reopen the UI and now install the python bindings of the MachineLearning_Toolkit by running
   the `BH.Engine.MachineLearning.Compute.InstallMachineLearningToolkit` component
1. Restart your UI
   
Note that the intallation will take a while - around 15 minutes.

## Notes for developers
- The python source files used by Python are stored in `C:\ProgramData\BHoM\Extensions\Python\src`. This folder is populated by the Visual Studio post-build events, which copy all the `*.py` files in the Toolkit into the destination folder.
- If you update a python file in your repo, to update the BHoM you just need to recompile the solution - this will copy the relevant files as described above.
- If you update a `.cs` file, you need to recompile the solution as usual.
- The component `InstallMachineLearningToolkit` installs the additional packages required by the python bindings of the BHoM. You will seldom use this method - usually only the first time.

Also, take a look at the [Python_Toolkit](https://github.com/BHoM/Python_Toolkit) for additional information: 


## Adding a new algorithm using python
If you want to add a new algorithm (e.g. KMeans) that requires python, below are the required steps to enable automatic deployment of the mehtod.
1. Add an oM object that stores the model - usually ML algorithms are parameterised functions, whose parameters are stored in a _model_.
   The object must be `IImmutable` and usually contains only one field that is a `PyObject`.
1. Add a `Engine.Convert.ToPython` method that takes the new object in and returns a `PyObject`
1. Add your algorithm in the `Engine.Compute` class. The algorithm usually contains three main methods: `Create`, which creates or loads the model; `Fit` that runs the learning routine; and `Infer`, which allows inference on a learnt model.
1. Note that all the python files must be contained into a `youralgorithmname.py` file, in the `Compute` folder, in order for it to work.


## Building the BHoM and the Toolkits from Source ##
You will need the following to build BHoM:

- Microsoft Visual Studio 2013 or higher
- Microsoft .NET Framework 4.0 and above (included with Visual Studio 2013)
- Note that there are no software - specific dependencies (only operating system relevant), this is specific: BHoM is a software agnostic object model.


## Want to contribute? ##

BHoM is an open-source project and would be nothing without its community. Take a look at our contributing guidelines and tips [here](https://github.com/BHoM/BHoM/blob/master/CONTRIBUTING.md).


## Licence ##

BHoM is free software licenced under GNU Lesser General Public Licence - [https://www.gnu.org/licenses/lgpl-3.0.html](https://www.gnu.org/licenses/lgpl-3.0.html)  
Each contributor holds copyright over their respective contributions.
The project versioning (Git) records all such contribution source information.
See [LICENSE](https://github.com/BHoM/BHoM/blob/master/LICENSE) and [COPYRIGHT_HEADER](https://github.com/BHoM/BHoM/blob/master/COPYRIGHT_HEADER.txt).
