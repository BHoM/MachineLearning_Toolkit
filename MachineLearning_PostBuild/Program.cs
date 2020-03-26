using System;
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

            // install python bhom package
            Console.WriteLine("Installing pyBHoM...");
            Compute.RunCommand("cd ../../../python && pip install -e .");
        }
    }
}
