using BH.oM.MachineLearning;
using Python.Runtime;
using BH.Engine.Python;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /*************************************/
        /**** Public Fields              ****/
        /*************************************/

        public static int Size(this Tensor tensor)
        {
            return (int)(Compute.InvokeNumpy("size", tensor.ToNumpy())).As<int>();
        }

        /*************************************/
    }
}
