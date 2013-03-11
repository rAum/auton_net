using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using RANSAC.Functions;

namespace VisionFilters.Output
{
    public class InvalidVariablesCountException: Exception
    {
    }

    public class KalmanFilter
    {
        private Kalman kalman;
        private int variablesCount;
        private bool isInitialized; // true if any data has been fed

        public KalmanFilter(int variables)
        {
            variablesCount = variables;

            int measurementVariables = variables;
            int dynamicVariables = variables * 2;

            float[] state = new float[dynamicVariables];
            for (int i = 0; i < dynamicVariables; ++i)
                state[i] = 0.0f;

            Matrix<float> transitionMatrix = new Matrix<float>(dynamicVariables, dynamicVariables);
            transitionMatrix.SetZero();
            for (int i = 0; i < dynamicVariables; ++i)
            {
                transitionMatrix[i, i] = 1.0f;
                if (i >= measurementVariables)
                    transitionMatrix[i - measurementVariables, i] = 1;
            }

            Matrix<float> measurementMatrix = new Matrix<float>(measurementVariables, dynamicVariables);
            measurementMatrix.SetZero();
            for (int i = 0; i < measurementVariables; ++i)
                measurementMatrix[i, i] = 1.0f;

            Matrix<float> processNoise = new Matrix<float>(dynamicVariables, dynamicVariables);
            processNoise.SetIdentity(new MCvScalar(6));//1.0e-4));

            Matrix<float> measurementNoise = new Matrix<float>(measurementVariables, measurementVariables);
            measurementNoise.SetIdentity(new MCvScalar(10));//1.0e-1));

            Matrix<float> errorCovariancePost = new Matrix<float>(dynamicVariables, dynamicVariables);
            errorCovariancePost.SetIdentity();

            kalman = new Kalman(dynamicVariables, measurementVariables, 0);
            kalman.CorrectedState = new Matrix<float>(state);
            kalman.TransitionMatrix = transitionMatrix;
            kalman.MeasurementNoiseCovariance = measurementNoise;
            kalman.ProcessNoiseCovariance = processNoise;
            kalman.ErrorCovariancePost = errorCovariancePost;
            kalman.MeasurementMatrix = measurementMatrix;
        }

        public PointF FeedPoint(PointF pt)
        {
            if (variablesCount != 2)
                throw new InvalidVariablesCountException();

            if (!isInitialized)
            {
                kalman.CorrectedState[0, 0] = pt.X;
                kalman.CorrectedState[1, 0] = pt.X;

                isInitialized = true;
                return pt;
            }

            kalman.Predict();

            Matrix<float> meas = new Matrix<float>(2, 1);
            meas[0, 0] = pt.X;
            meas[1, 0] = pt.Y;

            Matrix<float> estimation = kalman.Correct(meas);
            return new PointF(estimation[0, 0], estimation[1, 0]);
        }

        public PointF PredictPoint()
        {
            if (variablesCount != 2)
                throw new InvalidVariablesCountException();

            Matrix<float> prediction = kalman.Predict();
            return new PointF(prediction[0, 0], prediction[1, 0]);
        }

        public float[] FeedValues(float[] vals)
        {
            if (variablesCount != vals.Length)
                throw new InvalidVariablesCountException();

            if (!isInitialized)
            {
                for (int i = 0; i < vals.Length; ++i)
                    kalman.CorrectedState[i, 0] = vals[i];

                isInitialized = true;
                return vals;
            }

            kalman.Predict();

            Matrix<float> estimation = kalman.Correct(new Matrix<float>(vals));
            float[] ret = new float[variablesCount];
            for (int i = 0; i < variablesCount; ++i)
                ret[i] = estimation[i, 0];

            return ret;
        }

        public float[] PredictValues()
        {
            Matrix<float> prediction = kalman.Predict();
            float[] ret = new float[variablesCount];
            for (int i = 0; i < variablesCount; ++i)
                ret[i] = prediction[i, 0];

            return ret;
        }

        public Parabola FeedParabola(Parabola parabola)
        {
            if (variablesCount != 3)
                throw new InvalidVariablesCountException();

            if (!isInitialized)
            {
                kalman.CorrectedState[0, 0] = (float)parabola.a;
                kalman.CorrectedState[1, 0] = (float)parabola.b;
                kalman.CorrectedState[2, 0] = (float)parabola.c;

                isInitialized = true;
                return parabola;
            }

            Matrix<float> meas = new Matrix<float>(3, 1);
            meas[0, 0] = (float)parabola.a;
            meas[1, 0] = (float)parabola.b;
            meas[2, 0] = (float)parabola.c;

            kalman.Predict();

            Matrix<float> estimation = kalman.Correct(meas);
            return new Parabola(estimation[0, 0], estimation[1, 0], estimation[2, 0]);
        }

        public Parabola PredictParabola()
        {
            if (variablesCount != 3)
                throw new InvalidVariablesCountException();

            if (!isInitialized)
                return null;

            Matrix<float> prediction = kalman.Predict();
            return new Parabola(prediction[0, 0], prediction[1, 0], prediction[2, 0]);
        }
    }
}