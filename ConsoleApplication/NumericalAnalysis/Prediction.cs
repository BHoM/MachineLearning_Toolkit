using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using System;
using System.Collections.Generic;

namespace MachineLearning
{
    sealed public partial class MLPrediction
    {
        public static double PredictOutput(string onnxFilePath, double[] inputParameterValues)
        {
            // Set up the MLContext
            var mlContext = new MLContext();

            // Load the ONNX model
            OnnxScoringEstimator estimator = mlContext.Transforms.ApplyOnnxModel(onnxFilePath);


            return PredictFromFloats(doubleToFloat(inputParameterValues), mlContext, estimator);
        }

        internal interface InputData
        {
            public float[] Feature { get; set; }
            public float Target { get; set; }
        }

        internal class OutputData
        {
            public float[] Score { get; set; }
        }

        internal static float[] doubleToFloat(double[] array)
        {
            List<float> output = new List<float>(array.Length);
            foreach (double num in array)
            {
                output.Add((float)num);
            }
            return output.ToArray();
        }
    }
}
