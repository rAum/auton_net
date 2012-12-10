using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace VisionFilters.Filters.Lane_Mark_Detector
{
    public class VisualiseSimpleRoadModel : ThreadSupplier<SimpleRoadModel, Image<Rgb, byte>>
    {
        private Supplier<SimpleRoadModel> supplier;
        Image<Rgb, byte> output;

        private void CreateImage(SimpleRoadModel model)
        {
            output = new Image<Rgb, byte>(CamModel.Width, CamModel.Height);
            var one = model.leftLane;
            var two = model.rightLane;
            var cen = model.center;

            if (one != null)
            {
                for (int y = 0; y < output.Height; y += 8)
                {
                    output.Draw(
                        new CircleF(new PointF((float)one.value(y), (float)y), 3.0f)
                        , new Rgb(210, 140, 183),
                        0);
                }
            }

            if (two != null)
            {
                for (int y = 0; y < output.Height; y += 8)
                {
                    output.Draw(
                        new CircleF(new PointF((float)two.value(y), (float)y), 3.0f)
                        , new Rgb(170, 210, 143),
                        0);
                }
            }

            if (cen != null)
            {
                for (int y = 0; y < output.Height; y += 4)
                {
                    output.Draw(
                        new CircleF(new PointF((float)cen.value(y), (float)y), 1.0f)
                        , new Rgb(230, 230, 230),
                        0);
                }
            }

            LastResult = output;
            PostComplete();
        }

        public VisualiseSimpleRoadModel(Supplier<SimpleRoadModel> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Process += CreateImage;
        }

    }
}
