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
            return (Compute.InvokeNumpy("size", tensor.ToPython())).FromPython<int>();
        }

        /*************************************/
    }
}
