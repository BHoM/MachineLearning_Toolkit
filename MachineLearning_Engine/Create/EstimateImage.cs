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
        public static string EstimateImage(string imageFilePath, string modelFilePath)
        {
            string extension = Path.GetExtension(imageFilePath);
            if (!(extension == ".jpg") && !(extension == ".jpeg") && !(extension == ".png") && !(extension == ".gif"))
            {
                throw new System.ArgumentException($"Expected image file (jpg, jpeg, png, gif), file type of {extension} is not recognised.", "File Extension: ");
            }
            if (!(Path.GetExtension(modelFilePath) == ".zip"))
            {
                throw new System.ArgumentException("Expected '.zip' file.", "File Extension: ");
            }

            string runLines = $" EstimateImage {imageFilePath} {modelFilePath}";

            string output = MachineLearningConsole(runLines, true);

            return output;
        }
    }
}
