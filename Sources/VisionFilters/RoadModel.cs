using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RANSAC.Functions;
using Emgu.CV;

namespace VisionFilters
{
    public class RoadModel
    {
        internal class CyclicBuffer
        {
            private Parabola[] models;
            private int tail;
            private int head;


        }

        private Kalman kalman;

        public RoadModel()
        {
            kalman = new Kalman(6, 3, 0);

            kalman.TransitionMatrix = SetTransitionMatrix(1f);
        }

        private Matrix<Single> SetTransitionMatrix(float dt)
        {
            float[] transition = new float[] { dt, 0, 0,
                                               0, dt, 0,
                                               0, 0, dt };
            return new Matrix<Single>(transition);
        }
    }
}
