using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace Auton.CarVision.Video.Filters //VisionFilters.Filters.Lane_Mark_Detector
{
    public class LineHistogram : ThreadSupplier<Image<Gray, Byte>, Image<Rgb, Byte>>
    {
        private Supplier<Image<Gray, Byte>> supplier;
        private Image<Rgb, Byte> hist;

        public enum HistogramOrientation { Horizontal, Vertical };
        public HistogramOrientation Orientation { get; set; }

        private void BuildHistogram(Image<Gray, Byte> img)
        {
            hist = new Image<Rgb, Byte>(img.Width, img.Height);

            for (int y = 0; y < img.Height; ++y)
            {
                int sum = 0;
                for (int x = 0; x < img.Width; ++x)
                {
                    if (img.Data[y, x, 0] > 100)
                        ++sum;
                }
                sum *= 4;
                hist.Draw(new LineSegment2D(new Point(0, y), new Point(sum, y)), new Rgb(255, 0, 0), 1);
            }

            for (int y = 0; y < img.Width; ++y)
            {
                int sum = 0;
                for (int x = 0; x < img.Height; ++x)
                {
                    if (img.Data[x, y, 0] > 100)
                        ++sum;
                }
                sum *= 4;
                hist.Draw(new LineSegment2D(new Point(y, img.Height - 1), new Point(y, img.Height - sum)), new Rgb(0, 255, 0), 1);
            }

            LastResult = hist;
            PostComplete();
        }

        public LineHistogram(Supplier<Image<Gray, Byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;
            
            Process += BuildHistogram;
        }
    }
}
