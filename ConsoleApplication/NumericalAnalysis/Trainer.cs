using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace MachineLearning
{
    public sealed class MLTraining
    {
        public static void TrainAlgorithm(string filepath, char deliminator)
        {
            string[] lines = File.ReadAllLines(filepath);
            int numberOfColumns = lines[0].Split(deliminator).Length - 1;
            if (lines.Length < 2) { throw new System.ArgumentException("File contains insufficient rows of data.", "Numeric Data: "); }
            if (numberOfColumns > 20){ throw new System.ArgumentException("Files with more than 20 columns of data are not supported at this time.", "Columns: ");}
            if (numberOfColumns < 1) { throw new System.ArgumentException("An insufficent number of data columns has been included.", "Columns: "); }
            if (!IsDigitsOnly(lines[1].Replace(deliminator, '0').Replace(' ', '0').Replace('-', '0').Replace('.', '0')))
            {
                throw new System.ArgumentException("File contains non-numeric characters where numeric characters are expected.", "Numeric Data: ");
            }

            var mlContext = new MLContext();
            var loader = mlContext.Data.CreateTextLoader(new[] 
            {
                new TextLoader.Column("Feature", DataKind.Single, new[] { new TextLoader.Range(0, numberOfColumns - 1) }),
                new TextLoader.Column("Target", DataKind.Single, numberOfColumns)
            },  
            separatorChar: deliminator);

            var data = loader.Load(filepath);
            var pipeline = mlContext.Transforms.NormalizeMinMax("Feature")
                .AppendCacheCheckpoint(mlContext)
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Target", featureColumnName: "Feature"));


            var model = pipeline.Fit(data);
            string savePath = Path.ChangeExtension(filepath, ".onnx");
            using FileStream stream = File.Create(savePath);
            mlContext.Model.ConvertToOnnx(model, data, stream);
        }
        internal class InputData
        {
            [VectorType(20)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }
        internal class OutputData
        {
            public float[] Score { get; set; }
        }

        internal static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
