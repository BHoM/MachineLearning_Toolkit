using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Default;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Vision;

namespace MLImages
{
    sealed public class MachineLearning
    {    
        public static string TrainImageModel(string TSVFilePath, int seed)
        {
            if(!(Path.GetExtension(TSVFilePath) == ".tsv")) 
            {
                throw new System.ArgumentException("Expected '.tsv' file.", "File Extension: ");
            }

            string savePath = Path.ChangeExtension(TSVFilePath, ".zip");

            MLContext mlContext = new MLContext(seed: seed);
            IDataView trainingData = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: TSVFilePath,
                                            hasHeader: true,
                                            separatorChar: '\t',
                                            allowQuoting: true,
                                            allowSparse: false);

            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", "Label")
                                        .Append(mlContext.Transforms.LoadRawImageBytes("ImageSource_featurized", null, "ImageSource"))
                                        .Append(mlContext.Transforms.CopyColumns("Features", "ImageSource_featurized"));

            var trainer = mlContext.MulticlassClassification.Trainers.ImageClassification(new ImageClassificationTrainer.Options() { LabelColumnName = "Label", FeatureColumnName = "Features" })
                                        .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer mlModel = trainingPipeline.Fit(trainingData);

            // Save model
            mlContext.Model.Save(mlModel, trainingData.Schema, GetPath(savePath));
            return savePath;
        }
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
            ModelInput input = new ModelInput()
            {
                ImageSource = imageFilePath,
            };
            PredictionEngine<ModelInput, ModelOutput> PredictionEngine = CreatePredictionEngine(modelFilePath);
            ModelOutput result = PredictionEngine.Predict(input);
            string output = $"Estimated Image: '{result.Prediction}' .\nEstimated Accuracy: {((int)(result.Score[0] * 1000))/10.0}% .";
            return output;
        }

        internal static string GetPath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }

        internal static PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine(string filepath)
        {
            MLContext mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(filepath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            return predEngine;
        }

        internal class ModelInput
        {
            [ColumnName("Label"), LoadColumn(0)]
            public string Label { get; set; }


            [ColumnName("ImageSource"), LoadColumn(1)]
            public string ImageSource { get; set; }
        }
        internal class ModelOutput
        {
            [ColumnName("PredictedLabel")]
            public String Prediction { get; set; }
            public float[] Score { get; set; }
        }
    }
}
