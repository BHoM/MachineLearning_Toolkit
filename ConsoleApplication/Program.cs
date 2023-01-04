using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace Default
{
    class Program
    {
        static void sMain(string[] args)
        {

            Process process = Process.Start(@"C:\ProgramData\BHoM\Assemblies\netcoreapp3.1\MachineLearningConsole2.exe");
        }
        static void Main(string[] args)
        {
            if (args.Length == 0) { Console.WriteLine("nothing happens"); }
            
            if (args[0] == "TrainImageModel")
            {
                string zipfile = MLImages.MachineLearning.TrainImageModel(args[1], int.Parse(args[2]));
                Console.WriteLine($"Zip File Created, file can be found here {zipfile}");
            }

            if(args[0] == "EstimateImage") 
            {
                var predictionResult = MLImages.MachineLearning.EstimateImage(args[1], args[2]);
                Console.Out.Write(predictionResult);

            }

            if(args[0] == "NumberPatterns")
            {
                List<double> numbers = new List<double>();
                int num = 4;
                while(num < args.Length)
                {
                    numbers.Add(double.Parse(args[num]));
                    num++;
                }

                double output1 = MLNumbers.MachineLearning.NumberPatterns(args[1], args[2], numbers, bool.Parse(args[3]));
                Console.WriteLine(output1);
            }
            //-2.75 0.77 -0.61 0.14 1.39 0.38 -0.53 -0.50 -2.13 -0.39 0.46
        }
    }
}
