using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RANSAC.Functions;

namespace VisionFilters.Filters.Lane_Mark_Detector
{
    public class SimpleRoadModel
    {
        public Parabola leftLane;
        public Parabola rightLane;
        public Parabola center;

        public enum RoadModel
        {
            TwoLane,
            OneLane,
            CenterOnly
        };

        RoadModel type;
        public RoadModel Type
        {
            get
            {
                return type;
            }
        }

        public static double TooNearThreshold = 20;

        /// <summary>
        /// Tries to estimate which one is left and which is right and provide center
        /// </summary>
        /// <param name="a1">first parabola</param>
        /// <param name="a2">second parabola</param>
        public SimpleRoadModel(Parabola a1, Parabola a2)
        {
            if (Parabola.distance(a1, a2) <= TooNearThreshold)
            {
                Parabola merge = new Parabola((a1.a + a2.a * 0.5), (a1.b + a2.b * 0.5), (a1.c + a2.c * 0.5));
                leftLane = merge;
                rightLane = merge;
                center = merge;
                type = RoadModel.OneLane;
            }
            else
            {
                if (a2.value(100) < a1.value(100)) // estimate which one is on the right/left
                {
                    leftLane = a2;
                    rightLane = a1;
                }
                else
                {
                    leftLane = a1;
                    rightLane = a2;
                }

                center = new Parabola((leftLane.a + rightLane.a * 0.5), (leftLane.b + rightLane.b * 0.5), (leftLane.c + rightLane.c * 0.5));
                type = RoadModel.TwoLane;
            }
        }

        public SimpleRoadModel(Parabola center_)
        {
            type = RoadModel.CenterOnly;
            center = center_;
            leftLane = rightLane = null;
        }

        public SimpleRoadModel(Parabola center_, Parabola left_, Parabola right_)
        {
            center = center_;
            leftLane = left_;
            rightLane = right_;
            type = RoadModel.TwoLane;
        }
    }
}
