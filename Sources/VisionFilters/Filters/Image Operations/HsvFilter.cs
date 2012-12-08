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

        private bool inRange(double v, double min, double max)
        {
            if (v >= min && v <= max) return true;
            return false;
        }

        private byte clamp(int v)
        {
            if (v < 128) return 0;
            else // if (v > 255) return 255;
                return 255;// (byte)v;
        }
        private void GetChannel(Image<Rgb, byte> image)
        {
            var hsv = image;//.Convert<Hsv, byte>();

            byte[, ,] output = filtered.Data;
            for (int y = 0; y < hsv.Height; ++y)
                for (int x = 0; x < hsv.Width; ++x)
                {
                    output[y, x, 0] = clamp((2 * hsv.Data[y, x, 2] - hsv.Data[y, x, 0] - hsv.Data[y, x, 1]) + hsv.Data[y, x, 2] / 4);
                        //if (inRange(hsv[y,x].Hue, lower.Hue, upper.Hue) 
                        //    && inRange(hsv[y,x].Satuation, lower.Satuation, upper.Satuation)
                        //    && inRange(hsv[y,x].Value, lower.Value, upper.Value))
                        //    output[y, x, 0] = 255;
                        //else
                        //    output[y, x, 0] = 0;
                }

            LastResult = filtered;
            PostComplete();
        }

        public HsvFilter(Supplier<Image<Rgb, byte>> supplier_, Hsv lower_, Hsv upper_)
        {
            filtered = new Image<Gray, byte>(CamModel.Width, CamModel.Height);
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            lower = lower_;
            upper = upper_;

            Process += GetChannel;
        }
    }
}
