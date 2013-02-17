using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisionFilters.Filters.Lane_Mark_Detector;
using RANSAC.Functions;

namespace VisionFilters
{
    /// <summary>
    /// Class for simple road modeling over time.
    /// TODO: rethink this....
    /// </summary>
    class RoadModeling
    {
        const int TotalCapacity = 4; // how many last measurements.
        int toSkip = 13;                    // how many skips make
        Queue<SimpleRoadModel> models = new Queue<SimpleRoadModel>(TotalCapacity);
        SimpleRoadModel currentModel;

        public RoadModeling()
        {
        }

        public SimpleRoadModel Road
        {
            get
            {
                return models.Last();
            }
        }

        public void Add(SimpleRoadModel model)
        {
            if (toSkip != 0)
            {
                --toSkip;
            }
            else
            {
                if (models.Count == TotalCapacity)
                    models.Dequeue();

                currentModel = model;

                TestAndFixModel();

                models.Enqueue(model);
            }
        }

        /// <summary>
        /// Checks if probed points between current and reference parabola are close enough too each other (tolerance param) 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        private bool ProbeAndCheckTolerance(Parabola p, Parabola v, float tolerance = 4)
        {
            for (int i = 0; i <= 200; i += 30)
            {
                if (Math.Abs(p.at((double)i) - v.at((double)i)) < tolerance)
                {
                    return false;
                }
            }

            return true;
        }

        private void TestAndFixModel()
        {
            switch (currentModel.Type)
            {
                case SimpleRoadModel.RoadModel.CenterOnly:
                    break;
                case SimpleRoadModel.RoadModel.OneLane:
                    break;
                case SimpleRoadModel.RoadModel.TwoLane:
                    break;
            }
        }
    }
}
