using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using autonomiczny_samochod;
using System.Threading;
using Helpers;
using VisionFilters;

using Emgu.CV;
using Emgu.CV.Structure;
using Auton.CarVision.Video;
using Auton.CarVision.Video.Filters;

using VisionFilters.Output;
using Emgu.CV.UI;
using VisionFilters.Filters.Lane_Mark_Detector;
using VisionFilters.Filters.Image_Operations;
using VisionFilters;

namespace BrainProject
{
    

    public class FollowTheRoadBrainCentre
    {
        CarController carController;
        PIDRegulator regulator;

        public delegate void newTargetWheelAngeCalculatedEventHandler(object sender, double angle);
        public event newTargetWheelAngeCalculatedEventHandler evNewTargetWheelAngeCalculated;

        class Settings : PIDSettings
        {
            public Settings()
            {
                //P part settings
                P_FACTOR_MULTIPLER = 0.3;

                //I part settings
                I_FACTOR_MULTIPLER = 0; 
                I_FACTOR_SUM_MAX_VALUE = 100;
                I_FACTOR_SUM_MIN_VALUE = -100;
                I_FACTOR_SUM_SUPPRESSION_PER_SEC = 0.96; // = 0.88; //1.0 = suppresing disabled

                //D part settings
                D_FACTOR_MULTIPLER = 0;
                D_FACTOR_SUPPRESSION_PER_SEC = 0.0;
                D_FACTOR_SUM_MIN_VALUE = 100;
                D_FACTOR_SUM_MAX_VALUE = -100;

                //steering limits
                MAX_FACTOR_CONST = 30;
                MIN_FACTOR_CONST = -30;
            }
        }

        public FollowTheRoadBrainCentre(RoadCenterDetector _roadDetector)
        {
            roadDetector = _roadDetector;

            carController = new CarController();
            carController.SetTargetSpeed(0);
            carController.SetTargetWheelAngle(0);

            regulator = new PIDRegulator(new Settings(), "carSteeringRegulator");
            regulator.SetTargetValue(0.0); //we want to go straight with the road

            roadDetector.RoadCenterSupply += new RoadCenterHandler(roadDetector_RoadCenterSupply);
        }

        double[] pointsWages = new double[3] { 1.0, 1.0, 1.0 };
        const double MIDDLE_OF_THE_ROAD_IN_PIX = 320; //img_width/2
        void roadDetector_RoadCenterSupply(object sender, RoadCenterEvent e)
        {
            double currentValue = 0;
            for (int i = 0; i < 3; i++)
            {
                currentValue = (e.road[i].X - CamModel.Width / 2) * pointsWages[i] * -1;
            }
            if (Limiter.LimitAndReturnTrueIfLimitted(ref currentValue, -100, 100))
            {
                Logger.Log(this, "road value has been limmited", 1);
            }

            double steeringVal = regulator.ProvideObjectCurrentValueToRegulator(currentValue);

            carController.SetTargetWheelAngle(steeringVal);

            newTargetWheelAngeCalculatedEventHandler temp = evNewTargetWheelAngeCalculated;
            if (temp != null)
            {
                temp(this, steeringVal);
            }
        }

        private RoadCenterDetector roadDetector;


        void roadCenterDetector_RoadCenterSupply(object sender, VisionFilters.Output.RoadCenterEvent e)
        {
        }

    }
}
