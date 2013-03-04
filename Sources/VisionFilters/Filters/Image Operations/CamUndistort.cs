using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Auton.CarVision.Video.Filters
{
    class CamUndistort : ThreadSupplier<Image<Bgr, byte>, Image<Bgr, byte>>
    {
        private Supplier<Image<Bgr, byte>> supplier;

        private void Undistort(Image<Bgr, byte> image)
        {
            LastResult = image;
            PostComplete();
        }
        public CamUndistort(Supplier<Image<Bgr, byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            IntrinsicCameraParameters cap = new IntrinsicCameraParameters();
            
            Process += Undistort;
        }
    }
}
