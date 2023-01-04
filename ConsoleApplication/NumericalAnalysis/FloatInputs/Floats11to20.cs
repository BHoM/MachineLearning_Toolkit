using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using System;
using System.Collections.Generic;

namespace MachineLearning
{
    public sealed partial class MLPrediction
    {
        internal static double Floats11(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData11 inputData = new InputData11 { Feature = new float[11] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData11> inputEnumerable = new InputData11[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData11, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData11
        {
            [VectorType(11)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats12(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData12 inputData = new InputData12 { Feature = new float[12] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData12> inputEnumerable = new InputData12[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData12, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData12
        {
            [VectorType(12)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats13(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData13 inputData = new InputData13 { Feature = new float[13] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData13> inputEnumerable = new InputData13[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData13, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData13
        {
            [VectorType(13)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats14(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData14 inputData = new InputData14 { Feature = new float[14] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData14> inputEnumerable = new InputData14[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData14, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData14
        {
            [VectorType(14)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats15(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData15 inputData = new InputData15 { Feature = new float[15] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData15> inputEnumerable = new InputData15[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData15, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData15
        {
            [VectorType(15)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats16(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData16 inputData = new InputData16 { Feature = new float[16] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData16> inputEnumerable = new InputData16[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData16, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData16
        {
            [VectorType(16)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats17(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData17 inputData = new InputData17 { Feature = new float[17] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData17> inputEnumerable = new InputData17[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData17, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData17
        {
            [VectorType(7)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats18(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData18 inputData = new InputData18 { Feature = new float[18] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData18> inputEnumerable = new InputData18[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData18, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData18
        {
            [VectorType(18)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats19(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData19 inputData = new InputData19 { Feature = new float[19] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData19> inputEnumerable = new InputData19[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData19, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData19
        {
            [VectorType(19)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats20(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData20 inputData = new InputData20 { Feature = new float[20] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData20> inputEnumerable = new InputData20[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData20, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData20
        {
            [VectorType(20)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }
    }
}
