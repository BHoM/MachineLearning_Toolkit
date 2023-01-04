using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Engine.MachineLearning
{
    public static partial class Create
    {
        public static string CreateTSVfile(string folderPath, bool run)
        {

            string file = folderPath + folderPath.Split(Path.DirectorySeparatorChar)[folderPath.Split(Path.DirectorySeparatorChar).Length - 2] + ".tsv";

            if (!run) { goto end; }
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
        end:
            return file;
        }
    }
}
