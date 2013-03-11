using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using RANSAC.Functions;
using System.Diagnostics;
using VisionFilters.Output;

namespace VisionFilters.Filters.Lane_Mark_Detector
{
    /// <summary>
    /// Road modelling using Bezier
    /// </summary>
    public class ClusterLanes_BezierExperimental : ThreadSupplier<List<Point>, SimpleRoadModel> 
    {
        private Supplier<List<Point>> supplier;
        private double roadCenterDistAvg = 184; // estimated relative road distance [half of width]
        
        const double ROAD_CENTER_MIN = 175;
        const double ROAD_CENTER_MAX = 210;
        const int CENTER_PROBE_OFFSET = 10;
        const int MIN_POINTS_FOR_EACH = 280;
        const int MIN_POINTS_FOR_ONLY_ONE = 300;
        int imgWidth  = CamModel.Width;
        int imgHeight = CamModel.Height;
        int centerProbePoint,
            carCenter;
        
        private void ObtainSimpleModel(List<Point> lanes)
        {
            Bezier leftLane   = null;
            Bezier rightLane  = null;
            Bezier roadCenter = null;

//             if (lanes.Count > MIN_POINTS_FOR_ONLY_ONE)
//                 roadCenter = RANSAC.RANSAC.fitBezier(100, 3, (int)(lanes.Count * 0.75), 5, lanes);
// 
//             if (roadCenter != null) 
//                 create_model_from_single_line(ref roadCenter, ref leftLane, ref rightLane);
//             else if (lanes.Count > MIN_POINTS_FOR_EACH)// no one line mark can be matched. trying to find left and right and then again trying to find model.
//             {
//                 // try to cluster data to distinguish left and right lane
//                 List<Point> first = new List<Point>(6048);
//                 List<Point> second = new List<Point>(6048);
// 
//                 VisionToolkit.Two_Means_Clustering(lanes, ref first, ref second);
// 
//                 ////////////////////////////////////////////////////////////////
// 
//                 if (first.Count > MIN_POINTS_FOR_EACH)
//                     leftLane = RANSAC.RANSAC.fitBezier(100, 8, (int)(first.Count * 0.75), 15, first);
// 
//                 if (second.Count > MIN_POINTS_FOR_EACH)
//                     rightLane = RANSAC.RANSAC.fitBezier(100, 8, (int)(second.Count * 0.75), 15, second);
// 
//                 create_model_from_two_lanes(ref leftLane, ref rightLane, ref roadCenter);
//             }


            LastResult = new SimpleRoadModel(roadCenter, leftLane, rightLane);
            PostComplete();
        }

        private void create_model_from_two_lanes(ref Bezier leftLane, ref Bezier rightLane, ref Bezier roadCenter)
        {
            if (leftLane != null && rightLane != null)
            {
                // swap lanes if necessary
                if (leftLane.at(centerProbePoint) > rightLane.at(centerProbePoint))
                {
                    var t     = leftLane;
                    leftLane  = rightLane;
                    rightLane = leftLane;
                }

                // center is between left and right lane
                roadCenter = Bezier.merge(leftLane, rightLane);
            }
            else if (leftLane != null) // check if this is really a left lane
            {
                if (leftLane.at(centerProbePoint) > carCenter + 5) // this is right lane!!
                {
                    rightLane   = leftLane;
                    leftLane    = null;
                    roadCenter  = Bezier.Moved(rightLane, -roadCenterDistAvg);
                }
                else roadCenter = Bezier.Moved(leftLane, roadCenterDistAvg);
            }
            else if (rightLane != null) // check if this is really a left lane
            {
                if (rightLane.at(centerProbePoint) < carCenter + 5) // this is left lane!!
                {
                    leftLane    = rightLane;
                    rightLane   = null;
                    roadCenter  = Bezier.Moved(leftLane, roadCenterDistAvg);
                }
                else roadCenter = Bezier.Moved(rightLane, -roadCenterDistAvg);
            }
        }

        private void create_model_from_single_line(ref Bezier roadCenter, ref Bezier leftLane, ref Bezier rightLane)
        {
            double x = roadCenter.at(imgHeight - CENTER_PROBE_OFFSET);
            if (x < carCenter) {
                leftLane   = roadCenter;
                roadCenter = Bezier.Moved(leftLane, roadCenterDistAvg);
                rightLane  = Bezier.Moved(roadCenter, roadCenterDistAvg);
            }
            else {
                rightLane  = roadCenter;
                roadCenter = Bezier.Moved(rightLane, -roadCenterDistAvg);
                leftLane   = Bezier.Moved(roadCenter, -roadCenterDistAvg);
            }
        }


        public ClusterLanes_BezierExperimental(Supplier<List<Point>> supplier_)
        {
            supplier = supplier_;
            centerProbePoint = imgHeight - CENTER_PROBE_OFFSET;
            carCenter = imgWidth / 2;

            supplier.ResultReady += MaterialReady;
            Process += ObtainSimpleModel;
        }
    }

    /// <summary>
    /// Road modelling using parabolic equation
    /// </summary>
    public class ClusterLanes : ThreadSupplier<List<Point>, SimpleRoadModel> 
    {
        private Supplier<List<Point>> supplier;
        private double roadCenterDistAvg = 190; // estimated relative road distance [half of width]
        
        const double ROAD_CENTER_MIN = 174;
        const double ROAD_CENTER_MAX = 224;
        const int CENTER_PROBE_OFFSET = 10;
        const int MIN_POINTS_FOR_EACH = 450;
        const int MIN_POINTS_FOR_ONLY_ONE = 320;

        const int RANSAC_ITERATIONS = 650;
        const int RANSAC_MODEL_SIZE = 7;
        const int RANSAC_ERROR_THRESHOLD = 5;
        const double RANSAC_INLINERS = 0.65;

        int imgWidth  = CamModel.Width;
        int imgHeight = CamModel.Height;
        int centerProbePoint,
            carCenter;

        private KalmanFilter leftLaneKalmanFilter;
        private KalmanFilter rightLaneKalmanFilter;
        private KalmanFilter roadCenterKalmanFilter;

        private void ObtainSimpleModel(List<Point> lanes)
        {
            Parabola leftLane   = null;
            Parabola rightLane  = null;
            Parabola roadCenter = null;

            if (lanes.Count > MIN_POINTS_FOR_ONLY_ONE)
                roadCenter = RANSAC.RANSAC.fitParabola(RANSAC_ITERATIONS + 450, RANSAC_MODEL_SIZE, (int)(lanes.Count * 0.45), RANSAC_ERROR_THRESHOLD + 2, lanes);

            if (roadCenter != null) 
                create_model_from_single_line(ref roadCenter, ref leftLane, ref rightLane);
            else if (lanes.Count > MIN_POINTS_FOR_EACH)// no one line mark can be matched. trying to find left and right and then again trying to find model.
            {
                // try to cluster data to distinguish left and right lane
                List<Point> first = new List<Point>(2048);
                List<Point> second = new List<Point>(2048);

                VisionToolkit.Two_Means_Clustering(lanes, ref first, ref second);

                ////////////////////////////////////////////////////////////////

                if (first.Count > MIN_POINTS_FOR_EACH)
                    leftLane = RANSAC.RANSAC.fitParabola(RANSAC_ITERATIONS, RANSAC_MODEL_SIZE, (int)(first.Count * RANSAC_INLINERS), RANSAC_ERROR_THRESHOLD, first);

                if (second.Count > MIN_POINTS_FOR_EACH)
                    rightLane = RANSAC.RANSAC.fitParabola(RANSAC_ITERATIONS, RANSAC_MODEL_SIZE, (int)(second.Count * RANSAC_INLINERS), RANSAC_ERROR_THRESHOLD, second);

                create_model_from_two_lanes(ref leftLane, ref rightLane, ref roadCenter);
            }
            else
            {
                leftLane = leftLaneKalmanFilter.PredictParabola();
                rightLane = rightLaneKalmanFilter.PredictParabola();
                create_model_from_two_lanes(ref leftLane, ref rightLane, ref roadCenter);
            }

            LastResult = new SimpleRoadModel(roadCenter, leftLane, rightLane);
            PostComplete();
        }

        private void create_model_from_two_lanes(ref Parabola leftLane, ref Parabola rightLane, ref Parabola roadCenter)
        {
            //////////////////////////////////////////////////////////////////////////
            // KALMAN
            if (leftLane != null)
                leftLane = leftLaneKalmanFilter.FeedParabola(leftLane);
            else
                leftLane = leftLaneKalmanFilter.PredictParabola();

            if (rightLane != null)
                rightLane = rightLaneKalmanFilter.FeedParabola(rightLane);
            else
                rightLane = rightLaneKalmanFilter.PredictParabola();
            //////////////////////////////////////////////////////////////////////////
            false_signal:
            if (leftLane != null && rightLane != null)
            {
                // swap lanes if necessary
                if (leftLane.at(centerProbePoint) > rightLane.at(centerProbePoint))
                {
                    var t     = leftLane;
                    leftLane  = rightLane;
                    rightLane = leftLane;
                }

                if (Math.Abs(rightLane.c - leftLane.c) <= ROAD_CENTER_MIN - 10)
                {
                    leftLane  = Parabola.merge(rightLane, leftLane);
                    rightLane = null; 
                    goto false_signal;
                }

                // center is between left and right lane
                roadCenter = Parabola.merge(leftLane, rightLane);
                roadCenter = roadCenterKalmanFilter.FeedParabola(roadCenter);

                // reestimate road center
                double new_road_width = ((rightLane.c - roadCenter.c) + (roadCenter.c - leftLane.c)) * 0.5 * 0.1 + roadCenterDistAvg * 0.9;
                roadCenterDistAvg = Math.Max(Math.Min(new_road_width, ROAD_CENTER_MAX), ROAD_CENTER_MAX);
            }
            else if (leftLane != null) // check if this is really a left lane
            {
                if (leftLane.at(centerProbePoint) > carCenter) // this is right lane!!
                {
                    rightLane   = leftLane;
                    leftLane    = null;
                    roadCenter  = Parabola.Moved(rightLane, -roadCenterDistAvg);
                }
                else roadCenter = Parabola.Moved(leftLane, roadCenterDistAvg);
            }
            else if (rightLane != null) // check if this is really a left lane
            {
                if (rightLane.at(centerProbePoint) <= carCenter) // this is left lane!!
                {
                    leftLane    = rightLane;
                    rightLane   = null;
                    roadCenter  = Parabola.Moved(leftLane, roadCenterDistAvg);
                }
                else roadCenter = Parabola.Moved(rightLane, -roadCenterDistAvg);
            }
        }

        private void create_model_from_single_line(ref Parabola roadCenter, ref Parabola leftLane, ref Parabola rightLane)
        {
            double x = roadCenter.at(imgHeight - CENTER_PROBE_OFFSET);

            if (x < carCenter) {
                leftLane = leftLaneKalmanFilter.FeedParabola(roadCenter);
                if (rightLane == null)
                    rightLane  = Parabola.Moved(leftLane, 2 * roadCenterDistAvg);

                create_model_from_two_lanes(ref leftLane, ref rightLane, ref roadCenter);
            }
            else {
                rightLane = rightLaneKalmanFilter.FeedParabola(roadCenter);
                if (leftLane == null)
                    leftLane  = Parabola.Moved(rightLane, -2 * roadCenterDistAvg);

                create_model_from_two_lanes(ref leftLane, ref rightLane, ref roadCenter);
            }
        }
        

        public ClusterLanes(Supplier<List<Point>> supplier_)
        {
            supplier = supplier_;
            centerProbePoint = imgHeight - CENTER_PROBE_OFFSET;
            carCenter = imgWidth / 2;

            supplier.ResultReady += MaterialReady;
            Process += ObtainSimpleModel;

            leftLaneKalmanFilter = new KalmanFilter(3);
            rightLaneKalmanFilter = new KalmanFilter(3);
            roadCenterKalmanFilter = new KalmanFilter(3);
        }
    }
}
