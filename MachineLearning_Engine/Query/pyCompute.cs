using Python.Runtime;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /*************************************/
        /**** Public Fields              ****/
        /*************************************/

        public static PyObject pyCompute = Engine.Python.Query.Import("MachineLearning_Engine.Compute");

        /*************************************/
    }
}
