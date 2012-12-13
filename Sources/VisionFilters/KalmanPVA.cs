using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using System.Drawing;

using Emgu.CV.Structure;


namespace VisionFilters
{
    class KalmanRoadModel
    {
        KalmanPVA[,] kalman;

        public KalmanRoadModel()
        {
            kalman = new KalmanPVA[3, 2]; // three points in 2D

        }
    }

    /// <summary>
    /// Kalman for Position Velocity and Acceleration for 1D.
    /// </summary>
    class KalmanPVA
    {
        Kalman kalman;
        Matrix<Single> meas;
        float estimated;
        float predicted;

        public float Estimation
        {
            get { return estimated; }
        }

        public float Prediction
        {
            get { return predicted; }
        }

        public void foo(int i)
        {
            Matrix<float> x0 = new Matrix<float>(new float[] { 75, 126, 0, 0, 0, 0 });
            Matrix<float> A = new Matrix<float>(
                new float[,]{
                                         {1,0,0.04f,0,0.04f,0},
                                         {0,1,0,0.04f,0,0.04f},
                                         {0,0,1,0,1,0},
                                         {0,0,0,1,0,1},
                                         {0,0,0,0,1,0},
                                         {0,0,0,0,0,1}
                                        }
                                                                );

            Matrix<float> H = new Matrix<float>(
               new float[,]{
                                         {1,0,0,0,0,0},
                                         {0,1,0,0,0,0}                                         
                                        }
                                                );

            Matrix<float> Q = new Matrix<float>(new float[,] {{ 0, 0, 0, 0, 0, 0 },{ 0, 0, 0, 0, 0, 0 },{ 0, 0, 0, 0, 0, 0 },{ 0, 0, 0, 0, 0, 0 }});
            Matrix<float> R = new Matrix<float>(new float[,] {{ 0, 0, 0, 0, 0, 0 },{ 0, 0, 0, 0, 0, 0 },{ 0, 0, 0, 0, 0, 0 },{ 0, 0, 0, 0, 0, 0 }});

            Kalman temptkalman = new Kalman(x0, A, H, Q, R);

            Matrix<float> x1 = temptkalman.Predict();
        }

        public KalmanPVA()
        {
            foo(2);
      //      kalman = new Kalman(3, 1, 0);
      //      meas = new Matrix<float>(3, 1);
            
      //      kalman.MeasurementMatrix = meas;

      //      kalman.CorrectedState = new Matrix<float>(new float[]
      //{
      //      0.0f, 0.0f, 0.0f, 0.0f
      //});

      //      kalman.ProcessNoiseCovariance = new Emgu.CV.Matrix<Single>(3,3);
      //      kalman.ProcessNoiseCovariance.SetIdentity(new MCvScalar(0.05f));

      //      kalman.MeasurementNoiseCovariance = new Matrix<Single>(3, 3);
      //      kalman.MeasurementNoiseCovariance.SetIdentity(new MCvScalar(5.0f)); // 4 pixels accuracy

      //      kalman.ErrorCovariancePost = new Matrix<Single>(3,3);
      //      kalman.ErrorCovariancePost.SetIdentity();

      //      kalman.ControlMatrix = new Matrix<float>(1, 1);

      //      Measurment(0, 0);
        }

        public void Measurment(float m, float dt)
        {
            try
            {
	            Update(dt);
	            
	            var prediction = kalman.Predict();
	            predicted = prediction[0, 0];
	
	            meas[0, 0] = m;
	
	            var estimation = kalman.Correct(meas);
	            estimated = estimation[0, 0];
            }
            catch (Emgu.CV.Util.CvException ex)
            {
                System.Console.Out.WriteLine(ex);
            }
        }

        private void Update(float dt)
        {
            kalman.TransitionMatrix = new Matrix<Single>(
                new float[]{
                          1, dt, 0.5f * dt * dt,
                          0,  1,             dt,
                          0,  0,              1
                      }
                );
        }
    }

    class Kalman2D
    {
        private Kalman kal;
        private KalmanInit syntheticData;
        
        class KalmanInit
        {
            public 
        }

        void Init()
        {
            Matrix<float> state = new Matrix<float>(new float[]
            {
                0.0f, 0.0f, 0.0f, 0.0f
            });

            //kal.CorrectedState = state;
            //kal.TransitionMatrix = syntheticData.transitionMatrix;
            //kal.MeasurementNoiseCovariance = syntheticData.measurementNoise;
            //kal.ProcessNoiseCovariance = syntheticData.processNoise;
            //kal.ErrorCovariancePost = syntheticData.errorCovariancePost;
            //kal.MeasurementMatrix = syntheticData.measurementMatrix;
            }
        }
        
    }
}
