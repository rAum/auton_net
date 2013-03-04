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
    public class VisualiseSimpleRoadModel : ThreadSupplier<SimpleRoadModel, Image<Bgr, byte>>
    {
        private Supplier<SimpleRoadModel> supplier;
        Image<Bgr, byte> output;
        Bgr color1 = new Bgr(183, 100, 100),
            color2 = new Bgr(100, 170, 103),
            color3 = new Bgr(100, 100, 200);

        private void CreateImage(SimpleRoadModel model)
        {
            output = new Image<Bgr, byte>(CamModel.Width, CamModel.Height);
            var one = model.leftLane;
            var two = model.rightLane;
            var cen = model.center;

            if (one != null)
            {
                for (int y = 0; y < output.Height; y += 20)
                {
                    output.Draw(
                        new CircleF(new PointF((float)one.value(y), (float)y), 4.0f)
                        , color1 
                        , 0);
                }
            }

            if (two != null)
            {
                for (int y = 0; y < output.Height; y += 20)
                {
                    output.Draw(
                        new CircleF(new PointF((float)two.value(y), (float)y), 4.0f)
                        , color2
                        , 0);
                }
            }

            if (cen != null)
            {
                for (int y = 0; y < output.Height; y += 17)
                {
                    output.Draw(
                        new CircleF(new PointF((float)cen.value(y), (float)y), 5.0f)
                        , color3
                        , 0);
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
