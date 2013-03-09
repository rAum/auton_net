using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace VisionFilters.Output
{

    public class KalmanFilter
    {
        private Kalman kalman;
        private Matrix<float> state;
        private Matrix<float> transitionMatrix;
        private Matrix<float> measurementMatrix;
        private Matrix<float> processNoise;
        private Matrix<float> measurementNoise;
        private Matrix<float> errorCovariancePost;

        private PointF currentEstimation;

        public KalmanFilter()
        {
            state = new Matrix<float>(new float[] { 0.0f, 0.0f, 0.0f, 0.0f });

            transitionMatrix = new Matrix<float>(new float[,]
            {
                { 1, 0, 1, 0 },
                { 0, 1, 0, 1 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            });
            measurementMatrix = new Matrix<float>(new float[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 }
            });
            processNoise = new Matrix<float>(4, 4);
            processNoise.SetIdentity(new MCvScalar(1.0e-4));
            measurementNoise = new Matrix<float>(2, 2);
            measurementNoise.SetIdentity(new MCvScalar(1.0e-1));
            errorCovariancePost = new Matrix<float>(4, 4);
            errorCovariancePost.SetIdentity();

            kalman = new Kalman(4, 2, 0);
            kalman.CorrectedState = state;
            kalman.TransitionMatrix = transitionMatrix;
            kalman.MeasurementNoiseCovariance = measurementNoise;
            kalman.ProcessNoiseCovariance = processNoise;
            kalman.ErrorCovariancePost = errorCovariancePost;
            kalman.MeasurementMatrix = measurementMatrix;
        }

        public PointF FeedPoint(PointF pt)
        {
            Matrix<float> prediction = kalman.Predict();
            PointF predictPt = new PointF(prediction[0, 0], prediction[1, 0]);

            Matrix<float> meas = new Matrix<float>(2, 1);
            meas[0, 0] = pt.X;
            meas[1, 0] = pt.Y;

            Matrix<float> estimation = kalman.Correct(meas);
            currentEstimation = new PointF(estimation[0, 0], estimation[1, 0]);

            return currentEstimation;
        }

        public PointF GetCurrentEstimation()
        {
            return currentEstimation;
        }
    }
}
