using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Vision;

namespace MLImages
{
    public static class TSVBuilder
    {
        public static string CreateTSVfile(string folderPath)
        {
            
            string file = folderPath + folderPath.Split(Path.DirectorySeparatorChar)[folderPath.Split(Path.DirectorySeparatorChar).Length - 2] + ".tsv";

            using (StreamWriter tsvFile = new StreamWriter(file))
            {
                tsvFile.WriteLine("Label\tImageSource");

                foreach (string subfolder in Directory.GetDirectories(folderPath))
                {
                    foreach (string imageFilename in Directory.GetFiles(subfolder))
                    {
                        if (imageFilename.EndsWith(".jpg") || imageFilename.EndsWith(".png") || imageFilename.EndsWith(".jpeg") || imageFilename.EndsWith(".gif"))
                        {
                            tsvFile.WriteLine(Path.GetFileName(subfolder) + "\t" + imageFilename);
                        }
                    }
                }
            }
            return file;
        }
    }
}
