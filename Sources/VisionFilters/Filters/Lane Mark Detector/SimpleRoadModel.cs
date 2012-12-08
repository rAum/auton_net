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

        public SimpleRoadModel(Parabola center_, Parabola left_ = null, Parabola right_ = null)
        {
            center = center_;
            leftLane = left_;
            rightLane = right_;
        }
    }
}
