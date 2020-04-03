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

        public static int Size<T>(this Tensor<T> tensor)
        {
            return (Compute.InvokeNumpy("size", tensor) as PyInt).FromPython();
        }

        /*************************************/
    }
}
