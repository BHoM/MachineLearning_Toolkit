using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.MachineLearning
{
    public static partial class Create
    {
        public static double NumberPatterns(string csvFilePath, string delimintor, List<double> inputData, bool retrain = false)
        {
            string runLines = $"NumberPatterns {csvFilePath} {delimintor} {retrain}";
            int num = 0;
            while(num < inputData.Count)
            {
                runLines += $" {inputData[num]}";
                num++;
            }

            string output = MachineLearningConsole(runLines, true);
            return double.Parse(output);
        }
    }
}
