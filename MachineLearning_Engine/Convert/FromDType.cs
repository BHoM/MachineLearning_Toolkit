using Python.Runtime;
using System;

namespace BH.Engine.MachineLearning
{
    public static partial class Convert
    {
        /*************************************/
        /**** Public Fields              ****/
        /*************************************/

        public static Type FromDType(this string type)
        {
            switch (type.ToString())
            {
                case "int16":
                    return typeof(short);
                case "int32":
                    return typeof(int);
                case "int64":
                    return typeof(long);
                case "float16":
                    return typeof(float);
                case "float32":
                    return typeof(double);
                case "float128":
                    return typeof(decimal);
                default:
                    return typeof(double);
            }
        }

        /*************************************/
    }
}
