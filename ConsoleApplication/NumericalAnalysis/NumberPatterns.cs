using System;
using System.Collections.Generic;
using System.IO;
using MachineLearning;

namespace MLNumbers
{
    public partial class MachineLearning
    {
        public static double NumberPatterns(string csvFilePath, string delimintor, List<double> inputData, bool retrain = false)
        {
            string onnxPath = Path.ChangeExtension(csvFilePath, ".onnx");
            if(retrain || !File.Exists(onnxPath))
            {
                MLTraining.TrainAlgorithm(csvFilePath, ';');
            }
            double prediction = MLPrediction.PredictOutput(onnxPath, inputData.ToArray());
            return prediction;
        }
    }
}