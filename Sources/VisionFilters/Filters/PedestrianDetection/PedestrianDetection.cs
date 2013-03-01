using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace VisionFilters.Filters
{
    /// <summary>
    /// Simple class for pedestrian detection based on Histogram of Gradients
    /// with built in in EMGU CV people detector.
    /// </summary>
    class PedestrianDetection : ThreadSupplier<Image<Rgb,byte>, Rectangle[]>
    {
        Supplier<Image<Rgb, byte>> supplier;
        HOGDescriptor hogDes;
        UInt64 skip;
        UInt64 frame = 0;
        Rectangle[] empty = new Rectangle[] { };

        private void DetectPedestrian(Image<Rgb,byte> image)
        {
            if (frame % skip == 0)
                LastResult = hogDes.DetectMultiScale(image.Convert<Bgr, byte>());
            else
                LastResult = empty;
            PostComplete();
        }

        public PedestrianDetection(Supplier<Image<Rgb, byte>> supplier_, UInt64 skip_ = 1)
        {
            hogDes = new HOGDescriptor();
            hogDes.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            skip = skip_;
            Process += DetectPedestrian;
        }
    }
}
