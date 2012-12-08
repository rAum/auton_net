using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VisionFilters.Output
{
    /// <summary>
    /// This event is send when road center was estimated from lane marks.
    /// </summary>
    public class RoadCenterEvent : EventArgs
    {
        public PointF[] road;

        public RoadCenterEvent(PointF[] measurments)
        {
            road = measurments;
        }
    }

    public delegate void RoadCenterHandler(object sender, RoadCenterEvent e);
}
