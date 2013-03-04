using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace VisionFilters.Filters.Image_Operations
{
    class LaneMarkFilter : ThreadSupplier<Image<Bgr, byte>, List<Point>>
    {
    }
}
