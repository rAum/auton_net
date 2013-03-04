using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Auton.CarVision.Video;
using System.Drawing;

namespace VisionFilters.Filters.Image_Operations
{
    /// <summary>
    /// Gets one channel from RGB image.
    /// </summary>
    public class DrawPoints : ThreadSupplier<List<Point>, Image<Gray, byte>>
    {
        Supplier<List<Point>> supplier;
        Image<Gray, byte> image;
        bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private void GetChannel(List<Point> input)
        {
            if (active == false)
            {
                LastResult = null;
                PostComplete();
            }
            image.SetValue(new Gray(0));

            Point[] points = input.ToArray();
            var c = image.Data;
            foreach (var p in points) {
                c[p.Y, p.X, 0] = 255;
            }

            LastResult = image;
            PostComplete();
        }

        public DrawPoints(Supplier<List<Point>> supplier_)
        {
            image = new Image<Gray, byte>(CamModel.Width, CamModel.Height);
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;
            Process += GetChannel;
            active = false;
        }
    }
}
