using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using VisionFilters.Filters;
using VisionFilters;
using VisionFilters.Filters.Lane_Mark_Detector;
using System.Diagnostics;
using RANSAC.Functions;

namespace Auton.CarVision.Video.Filters
{
    /// <summary>
    /// This class finds pixel which may be white road lane.
    /// </summary>
    public class LaneMarkDetector : ThreadSupplier<Image<Gray, byte>, List<Point>>
    {
        private Supplier<Image<Gray, byte>> supplier;
        private int tau;

        private const int DefaultAllocation = 4000;

        public int VerticalOffset { get; set; }

        public int Tau
        {
            get { return tau;  }
            set { tau = value < 1 ? 1 : value; }
        }

        public byte Threshold { get; set; }

        private void DetectLaneMark(Image<Gray, byte> img)
        {
            List<Point> candidates = new List<Point>(DefaultAllocation);

            int aux;
            int x, y;
            int w = img.Width - tau - 1;
            byte[, ,] raw = img.Data;

            for (y = VerticalOffset; y < img.Height; ++y)
            {
                for (x = tau; x < w; ++x)
                {
                    aux = 2 * raw[y, x, 0];
                    aux -= raw[y, x - tau, 0];
                    aux -= raw[y, x + tau, 0];

                    aux -= Math.Abs(raw[y, x - tau, 0] - raw[y, x + tau, 0]);
                    
                    aux *= 2;// more contrast

                    if (aux >= Threshold)
                        candidates.Add(new Point(x, y));
                }
            }

            LastResult = candidates;
            PostComplete();
        }

        public LaneMarkDetector(Supplier<Image<Gray, byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Tau = 6;
            Threshold = 100;
            VerticalOffset = 0;

            Process += DetectLaneMark;
        }

    }
}
