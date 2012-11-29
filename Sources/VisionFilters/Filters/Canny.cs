using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;

namespace Auton.CarVision.Video.Filters
{
    public class Canny : ThreadSupplier<Image<Gray, Byte>, Image<Gray, Byte>>
    {
        private Supplier<Image<Gray, Byte>> supplier;

        private void FindEdges(Image<Gray, Byte> image)
        {
            LastResult = image.Canny(new Gray(100), new Gray(60));
            PostComplete();
        }

        public Canny(Supplier<Image<Gray, Byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Process += FindEdges;
        }
    }
}
