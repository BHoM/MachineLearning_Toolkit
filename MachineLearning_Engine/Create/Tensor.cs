using BH.oM.MachineLearning;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.MachineLearning
{
    public static partial class Create
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static Tensor<T> Tensor<T>(T[] array)
        {
            return new Tensor<T> { Handle = array.ToNumpy().Handle };
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(List<T> list1D)
        {
            return new Tensor<T> { Handle = list1D.ToNumpy().Handle };
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(IEnumerable<T> list1D)
        {
            return new Tensor<T> { Handle = list1D.ToNumpy().Handle };
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(IEnumerable<IEnumerable<T>> list2D)
        {
            return new Tensor<T> { Handle = list2D.ToNumpy().Handle };
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(T[,] array2D)
        {
            return new Tensor<T> { Handle = array2D.ToNumpy().Handle };
        }

        /*************************************/

        public static Tensor<T> Tensor<T>(T[][] arrayJagged)
        {
            return new Tensor<T> { Handle = arrayJagged.ToNumpy().Handle };
        }

        /*************************************/
    }
}
