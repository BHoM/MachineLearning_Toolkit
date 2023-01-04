using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;

namespace MachineLearning
{
    public sealed partial class MLPrediction
    {
        internal static double PredictFromFloats(float[] inputParameterValues, MLContext mlContext, OnnxScoringEstimator estimator)
        {
            if (inputParameterValues.Length == 1) { return Floats1(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 2) { return Floats2(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 3) { return Floats3(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 4) { return Floats4(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 5) { return Floats5(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 6) { return Floats6(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 7) { return Floats7(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 8) { return Floats8(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 9) { return Floats9(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 10) { return Floats10(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 11) { return Floats11(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 12) { return Floats12(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 13) { return Floats13(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 14) { return Floats14(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 15) { return Floats15(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 16) { return Floats16(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 17) { return Floats17(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 18) { return Floats18(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 19) { return Floats19(inputParameterValues, mlContext, estimator); }
            if (inputParameterValues.Length == 20) { return Floats20(inputParameterValues, mlContext, estimator); }
            return 0;
        }
    }
}
