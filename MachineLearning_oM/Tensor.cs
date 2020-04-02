using System;

namespace BH.oM.MachineLearning
{
    public class Tensor<T> where T : struct
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public IntPtr Handle { get; set; } = new IntPtr();

        /***************************************************/
    }
}
