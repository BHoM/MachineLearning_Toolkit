using BH.Engine.Python;
using BH.oM.MachineLearning;
using Python.Runtime;
using System;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /*************************************/
        /**** Public Fields              ****/
        /*************************************/

        public static Type DType(this Tensor tensor)
        {
            string dtype = tensor.NumpyArray?.GetAttr("dtype")?.GetAttr("name")?.FromPython<string>();
            return dtype?.FromDType();
        }

        /*************************************/
    }
}
