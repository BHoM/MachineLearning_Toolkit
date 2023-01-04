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
        public static string BuildImageModel(string TSVFilePath, bool retrain = false, int seed = 1)
        {
            if (!(Path.GetExtension(TSVFilePath) == ".tsv"))
            {
                throw new System.ArgumentException("Expected '.tsv' file.", "File Extension: ");
            }

            if(!retrain)
            {
                goto end;
            }

            string runLines = $" TrainImageModel {TSVFilePath} {seed}";


            MachineLearningConsole(runLines, false);

            
        end:
            string savePath = Path.ChangeExtension(TSVFilePath, ".zip");
            return savePath;
        }
    }
}
