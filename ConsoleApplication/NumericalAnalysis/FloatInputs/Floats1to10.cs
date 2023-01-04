using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using System;
using System.Collections.Generic;

namespace MachineLearning
{
    sealed public partial class MLPrediction
    {
        internal static double Floats1(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData1 inputData = new InputData1 { Feature = new float[1] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData1> inputEnumerable = new InputData1[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData1, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData1
        {
            [VectorType(1)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats2(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData2 inputData = new InputData2 { Feature = new float[2] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData2> inputEnumerable = new InputData2[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData2, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData2
        {
            [VectorType(2)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats3(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData3 inputData = new InputData3 { Feature = new float[3] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData3> inputEnumerable = new InputData3[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData3, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData3
        {
            [VectorType(3)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats4(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData4 inputData = new InputData4 { Feature = new float[4] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData4> inputEnumerable = new InputData4[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData4, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData4
        {
            [VectorType(4)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats5(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData5 inputData = new InputData5 { Feature = new float[5] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData5> inputEnumerable = new InputData5[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData5, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData5
        {
            [VectorType(5)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats6(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData6 inputData = new InputData6 { Feature = new float[6] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData6> inputEnumerable = new InputData6[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData6, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData6
        {
            [VectorType(6)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats7(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData7 inputData = new InputData7 { Feature = new float[7] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData7> inputEnumerable = new InputData7[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData7, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData7
        {
            [VectorType(7)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats8(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData8 inputData = new InputData8 { Feature = new float[8] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData8> inputEnumerable = new InputData8[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData8, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData8
        {
            [VectorType(8)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats9(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData9 inputData = new InputData9 { Feature = new float[9] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData9> inputEnumerable = new InputData9[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData9, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData9
        {
            [VectorType(9)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }

        internal static double Floats10(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            InputData10 inputData = new InputData10 { Feature = new float[10] };
            inputData.Feature = inputParameterValues;
            IEnumerable<InputData10> inputEnumerable = new InputData10[] { inputData };
            IDataView prediction = mlContext.Data.LoadFromEnumerable(inputEnumerable);
            var model = estimator.Fit(prediction);
            var predictionFunction = mlContext.Model.CreatePredictionEngine<InputData10, OutputData>(model);
            OutputData outputData = predictionFunction.Predict(inputData);
            float[] result = outputData.Score;

            return (double)result[0];

        }
        internal class InputData10
        {
            [VectorType(10)]
            public float[] Feature { get; set; }

            public float Target { get; set; }
        }
    }
}
