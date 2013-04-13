using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;

namespace VisionFilters.Filters.Image_Operations
{
    /// <summary>
    /// Gets one channel from RGB image.
    /// </summary>
    public class DrawColoredPointSets: ThreadSupplier<List<List<Point>>, Image<Bgr, byte>>
    {
        Supplier<List<List<Point>>> supplier;
        Image<Bgr, byte> image;
        bool active;
        static Bgr[] colors = new Bgr[]{
            new Bgr(255.0, 150.0, 150.0),
            new Bgr(150.0, 255.0, 150.0),
            new Bgr(150.0, 150.0, 255.0),
            new Bgr(255.0, 255.0, 150.0),
            new Bgr(255.0, 150.0, 255.0),
            new Bgr(150.0, 255.0, 255.0),
            new Bgr(255.0, 255.0, 255.0)
        };

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private void GetChannel(List<List<Point>> sets)
        {
            if (active == false)
            {
                LastResult = null;
                PostComplete();
            }
            image.SetValue(new Bgr(0.0, 0.0, 0.0));

            var c = image.Data;
            for (int setIdx = 0; setIdx < sets.Count; ++setIdx)
            {
                foreach (var p in sets[setIdx])
                    image.Draw(new CircleF(p, 4.0f), colors[setIdx], 0);
            }

            LastResult = image;
            PostComplete();
        }

        public DrawColoredPointSets(Supplier<List<List<Point>>> supplier_)
        {
            image = new Image<Bgr, byte>(CamModel.Width, CamModel.Height);
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;
            Process += GetChannel;
            active = false;
        }
    }
}
