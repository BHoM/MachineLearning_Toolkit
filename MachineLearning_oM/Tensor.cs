using System;

namespace BH.oM.MachineLearning
{
    public class Tensor<T>
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public IntPtr Handle { get; set; } = new IntPtr();

        /***************************************************/
    }
}
