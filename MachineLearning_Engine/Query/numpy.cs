using Python.Runtime;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /*************************************/
        /**** Public Fields              ****/
        /*************************************/

        public static PyObject numpy = Engine.Python.Query.Import("numpy");

        /*************************************/
    }
}
