using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Auton.CarVision.Video.Filters
{
    class CamUndistort : ThreadSupplier<Image<Rgb, Byte>, Image<Rgb, Byte>>
    {
        private Supplier<Image<Rgb, Byte>> supplier;

        private void Undistort(Image<Rgb, Byte> image)
        {
            LastResult = image;
            PostComplete();
        }
        public CamUndistort(Supplier<Image<Rgb, Byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            IntrinsicCameraParameters cap = new IntrinsicCameraParameters();
            
            Process += Undistort;
        }
    }
}
