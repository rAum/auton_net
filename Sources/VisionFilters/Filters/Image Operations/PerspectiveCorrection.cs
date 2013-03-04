using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu;

namespace Auton.CarVision.Video.Filters
{
        /// <summary>
        /// This filter makes perspective correction.
        /// </summary>
    public class PerspectiveCorrection : ThreadSupplier<Image<Gray, Byte>, Image<Gray, Byte>>
    {
        private Supplier<Image<Gray, Byte>> supplier;
        private PointF[] srcPoints; // input points
        private PointF[] dstPoints; // output points
        private HomographyMatrix transformationMatrix;

        #region Getters & Setters
        public PointF[] SrcPoints
        {
            get
            {
                return srcPoints; 
            }
            set 
            { 
                srcPoints = value;
                CalculateTransformation();
            }
        }

        public PointF[] DstPoints
        {
            get
            {
                return dstPoints;
            }
            set
            {
                dstPoints = value;
                CalculateTransformation();
            }

        }
    #endregion

        private void CalculateTransformation()
        {
            transformationMatrix = CameraCalibration.GetPerspectiveTransform(srcPoints, dstPoints);
        }

        private void DoPerspectiveCorrection(Image<Gray, Byte> img)
        {
            LastResult = img.WarpPerspective(transformationMatrix, INTER.CV_INTER_CUBIC, WARP.CV_WARP_DEFAULT, new Gray(0));
            PostComplete();
        }

        public PerspectiveCorrection(Supplier<Image<Gray, Byte>> supplier_, PointF[] src, PointF[] dst)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;
                
            srcPoints = src;
            dstPoints = dst;
            CalculateTransformation();

            Process += DoPerspectiveCorrection;
        }
    }
    public class PerspectiveCorrectionRgb : ThreadSupplier<Image<Bgr, byte>, Image<Bgr, byte>>
    {
        private Supplier<Image<Bgr, byte>> supplier;
        private PointF[] srcPoints; // input points
        private PointF[] dstPoints; // output points
        private HomographyMatrix transformationMatrix;

        #region Getters & Setters
        public PointF[] SrcPoints
        {
            get
            {
                return srcPoints;
            }
            set
            {
                srcPoints = value;
                CalculateTransformation();
            }
        }

        public PointF[] DstPoints
        {
            get
            {
                return dstPoints;
            }
            set
            {
                dstPoints = value;
                CalculateTransformation();
            }

        }
        #endregion

        private void CalculateTransformation()
        {
            transformationMatrix = CameraCalibration.GetPerspectiveTransform(srcPoints, dstPoints);
        }

        private void DoPerspectiveCorrection(Image<Bgr, byte> img)
        {
            LastResult = img.WarpPerspective(transformationMatrix, INTER.CV_INTER_CUBIC, WARP.CV_WARP_DEFAULT, new Bgr(0,0,0));
            PostComplete();
        }

        public PerspectiveCorrectionRgb(Supplier<Image<Bgr, byte>> supplier_, PointF[] src, PointF[] dst)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            srcPoints = src;
            dstPoints = dst;
            CalculateTransformation();

            Process += DoPerspectiveCorrection;
        }
    }

}
