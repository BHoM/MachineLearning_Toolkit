using BH.oM.Geometry;
using System.Collections.Generic;

namespace BH.oM.Vision
{
    public class DetectionResults
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public List<Polyline> Boxes { get; set; } = new List<Polyline>();

        public List<int> Categories { get; set; } = new List<int>();

        public List<double> Scores { get; set; } = new List<double>();

        /***************************************************/
    }
}
