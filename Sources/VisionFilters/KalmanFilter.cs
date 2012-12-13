using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using System.Drawing;

using Emgu.CV.Structure;


namespace VisionFilters
{
    /// <summary>
    /// Kalman filter for 2D point.
    /// TODO: add acceleration
    /// </summary>
    class Kalman2D
    {
        Kalman kal;

        Matrix<float> state;
        Matrix<float> transitionMatrix;
        Matrix<float> measurementMatrix;
        Matrix<float> nomeasurementMatrix;
        Matrix<float> processNoise;
        Matrix<float> measurementNoise;
        Matrix<float> errorCovariancePost;

        PointF pred; // predicted
        PointF est; // estimation

        public Kalman2D()
        {
            PrepareMatrixes();

            kal = new Kalman(4, 2, 0);

            kal.CorrectedState = state;
            kal.TransitionMatrix = transitionMatrix;
            kal.ProcessNoiseCovariance = processNoise;
            kal.ErrorCovariancePost = errorCovariancePost;
            kal.MeasurementMatrix = nomeasurementMatrix; // measurmentMatrix
            kal.MeasurementNoiseCovariance = measurementNoise;

            pred = new PointF();
            est = new PointF();
        }

        Matrix<float> DeltaTransition(float dt)
        {
            return new Matrix<float>(new float[,]
                {
                    {1, 0, dt, 0}, // x' = x + dt * vx
                    {0, 1, 0, dt}, // y' = y + dt * vy
                    {0, 0, 1, 0},  // vx' = vx
                    {0, 0, 0, 1}   // vy' = vy
                });
        }

        /// <summary>
        /// Update with taking delta time into account
        /// </summary>
        /// <param name="dt">delta time</param>
        public void Update(float dt)
        {
            kal.TransitionMatrix = DeltaTransition(dt);
        }

        /// <summary>
        /// Update to next state [dt = 1]
        /// </summary>
        public void Update()
        {
            kal.TransitionMatrix = transitionMatrix;
        }

        public void Measurment(PointF p)
        {
            Matrix<float>  m = new Matrix<float>(2, 1);
            m[0, 0] = p.X;
            m[1, 0] = p.Y;

            Matrix<float> prediction = kal.Predict();
            Matrix<float> estimation = kal.Correct(state);

            pred.X = prediction[0, 0];
            pred.Y = prediction[1, 0];

            est.X = estimation[0, 0];
            est.Y = estimation[1, 0];
        }

        // no measurement so just try to predict data.
        public void Measurment()
        {
            kal.MeasurementMatrix = nomeasurementMatrix; // should be enough [bug in opencv 2.3.1]
            Measurment(new PointF(0, 0));
            kal.MeasurementMatrix = measurementMatrix;
        }

        public PointF Estimation
        {
            get { return est; }
        }

        public PointF Prediction
        {
            get { return pred; }
        }

        void PrepareMatrixes()
        {
            state = new Matrix<float>(4, 1);
            state[0, 0] = 0.0f; // x
            state[1, 0] = 0.0f; // y
            state[2, 0] = 0.0f; // vx
            state[3, 0] = 0.0f; // vy

            transitionMatrix = new Matrix<float>(new float[,]
            {
                {1, 0, 1, 0 }, // x' = x + vx
                {0, 1, 0, 1 }, // y' = y + vy
                {0, 0, 1, 0 }, // vx
                {0, 0, 0, 1}   // vy
            });

            measurementMatrix = new Matrix<float>(new float[,]
            {
                {1, 0, 0, 0}, // x
                {0, 1, 0, 0} //  y
            });

            nomeasurementMatrix = new Matrix<float>(new float[,] { // for bug in opencv 2.3.1 - when no measurment is readed.
                {0, 0, 0, 0},
                {0, 0, 0, 0} 
            });

            processNoise = new Matrix<float>(4, 4); // [size of transition matrix]
            processNoise.SetIdentity(new MCvScalar(0.5)); // smaller value -> more resistance to noise (how much process is noisy)

            measurementNoise = new Matrix<float>(2, 2); // [size of input data]
            measurementNoise.SetIdentity(new MCvScalar(4)); // value of noise in measurements 

            errorCovariancePost = new Matrix<float>(4, 4); // [size of transition matrix]
            errorCovariancePost.SetIdentity();
        }
        
    }
}
