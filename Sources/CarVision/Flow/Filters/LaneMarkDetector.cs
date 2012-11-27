using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace CarVision.Filters
{
    /// <summary>
    /// This class finds pixel which may be white road lane.
    /// </summary>
    class LaneMarkDetector : ThreadSupplier<Image<Gray, Byte>, Image<Gray, Byte>>
    {
        private Supplier<Image<Gray, Byte>> supplier;
        private int tau;

        public int Tau
        {
            get { return tau;  }
            set { tau = value < 1 ? 1 : value; }
        }

        public double Threshold { get; set; }

        // TODO: rewrite this by using Data, or better - as native.
        private void DetectLaneMark(Image<Gray, Byte> img)
        {
            Image<Gray, Byte> dst = new Image<Gray, byte>(img.Width, img.Height);

            double aux;
            int i, j;
            int w = img.Width - tau - 1;
            for (j = 0; j < img.Height; ++j)
            {
                for (i = tau; i < w; ++i)
                {
                    aux = 2.0 * img[j, i].Intensity;
                    aux -= img[j, i - tau].Intensity;
                    aux -= img[j, i + tau].Intensity;
                        
                    aux -= Math.Abs(img[j, i - tau].Intensity - img[j, i + tau].Intensity);
                    
                    aux *= 2.0;// more contrast

                    if (aux > 255.0) aux = 255.0;
                    else if (aux < Threshold) aux = 0.0;

                    dst[j, i] = new Gray(aux);
                }
            }

            LastResult = dst;
            PostComplete();
        }

        public LaneMarkDetector(Supplier<Image<Gray, Byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Tau = 5;
            Threshold = 175.0;

            Process += DetectLaneMark;
        }

    }
}
