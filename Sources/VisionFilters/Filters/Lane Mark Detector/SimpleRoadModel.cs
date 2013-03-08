using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RANSAC.Functions;

namespace VisionFilters.Filters.Lane_Mark_Detector
{
    /// <summary>
    /// Simple class modeling a road which contains:
    /// - left & right lane
    /// - estimated center, virtual lane
    /// </summary>
    public class SimpleRoadModel
    {
        public Function leftLane;
        public Function rightLane;
        public Function center;

        public enum RoadModel
        {
            TwoLane,
            Invalid // not founded for example
        };

        RoadModel type = RoadModel.Invalid;
        public RoadModel Type
        {
            get
            {
                return type;
            }
        }

        public SimpleRoadModel(Function center_, Function left_, Function right_)
        {
            if (center_ == null || left_ == null || right_ == null)
                type = RoadModel.Invalid;

            center = center_;
            leftLane = left_;
            rightLane = right_;
            type = RoadModel.TwoLane;
        }

    }
}
