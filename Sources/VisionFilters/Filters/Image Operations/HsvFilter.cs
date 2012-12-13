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
    /// <summary>
    /// Gets one channel from RGB image.
    /// </summary>
    public class HsvFilter : ThreadSupplier<Image<Rgb, byte>, Image<Gray, Byte>>
    {
        Supplier<Image<Rgb, byte>> supplier;
        public Hsv lower, upper;
        Image<Gray, byte> filtered;
        bool skip;

        private bool inRange(double v, double min, double max)
        {
            if (v >= min && v <= max) return true;
            return false;
        }

        private void GetChannel(Image<Rgb, byte> image)
        {
            if (!skip)
                LastResult = image.Convert<Hsv, byte>().InRange(lower, upper).Dilate(4).Erode(5); // filtered;
            else
                LastResult = image.Convert<Gray, byte>().Not();
            PostComplete();
        }

        public HsvFilter(Supplier<Image<Rgb, byte>> supplier_, Hsv lower_, Hsv upper_, bool skip_)
        {
            filtered = new Image<Gray, byte>(CamModel.Width, CamModel.Height);
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            lower = lower_;
            upper = upper_;
            skip = skip_;
            Process += GetChannel;
        }
    }
}
