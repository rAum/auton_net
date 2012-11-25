using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;

namespace CarVision.Filters
{
    class Smoothener : ThreadSupplier<Image<Gray, Byte>, Image<Gray, Byte>>
    {
        private Supplier<Image<Gray, Byte>> supplier;

        private void SmoothenImage(Image<Gray, Byte> image)
        {
            LastResult = image.SmoothBlur(10, 10);
            PostComplete();
        }

        public Smoothener(Supplier<Image<Gray, Byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Process += SmoothenImage;
        }
    }
}
