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
        internal static string MachineLearningConsole(string runLines, bool hidden)
        {
            Process console = new Process();
            console.StartInfo.CreateNoWindow = false;
            console.StartInfo.UseShellExecute = false;
            console.StartInfo.FileName = @"C:\ProgramData\BHoM\Assemblies\netcoreapp3.1\MachineLearningConsole";
            console.StartInfo.Arguments = runLines;
            if (hidden)
            {
                console.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            if (!hidden)
            {
                console.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            }
            console.StartInfo.RedirectStandardOutput = true;
            console.Start();
            string output = console.StandardOutput.ReadToEnd();

            return output;
        }
    }
}
