using Python.Runtime;
using System;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        /*************************************/
        /**** Public Fields              ****/
        /*************************************/

        public static string DType(this Type type)
        {
            switch (type.ToString())
            {
                case "System.Int16":
                    return "int16";
                case "System.Int32":
                    return "int32";
                case "System.Int64":
                    return "int64";
                case "System.Single":
                    return "float16";
                case "System.Double":
                    return "float32";
                case "System.Decimal":
                    return "float128";
                default:
                    return "float32";
            }
        }

        /*************************************/
    }
}
