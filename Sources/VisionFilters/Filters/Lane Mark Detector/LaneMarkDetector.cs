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
    public class LaneMarkDetector : ThreadSupplier<Image<Gray, Byte>, Image<Gray, Byte>>
    {
        private Supplier<Image<Gray, Byte>> supplier;
        private int tau;
        private Image<Gray, Byte> dst = null;

        public int VerticalOffset { get; set; }

        public int Tau
        {
            get { return tau;  }
            set { tau = value < 1 ? 1 : value; }
        }

        public byte Threshold { get; set; }

        private void DetectLaneMark(Image<Gray, Byte> img)
        {
            if (dst == null)
                dst = new Image<Gray, byte>(img.Width, img.Height);

            int aux;
            int x, y;
            int w = img.Width - tau - 1;
            byte[, ,] raw = img.Data;
            byte[, ,] dstRaw = dst.Data;

            for (y = VerticalOffset; y < img.Height; ++y)
            {
                for (x = tau; x < w; ++x)
                {
                    aux = 2 * raw[y, x, 0];
                    aux -= raw[y, x - tau, 0];
                    aux -= raw[y, x + tau, 0];
                        
                    aux -= Math.Abs(raw[y, x - tau, 0] - raw[y, x + tau, 0]);
                    
                    aux *= 2;// more contrast

                    if (aux < Threshold || y < VerticalOffset) aux = 0;
                    else aux = 255;
                    dstRaw[y, x, 0] = (byte)aux;
                }
            }

            LastResult = dst;
            PostComplete();
        }

        public LaneMarkDetector(Supplier<Image<Gray, Byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Tau = 5;
            Threshold = 40;
            VerticalOffset = 0;

            Process += DetectLaneMark;
        }

    }
}
