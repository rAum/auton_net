using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using autonomiczny_samochod;
using System.Threading;
using Helpers;
using VisionFilters;

namespace BrainProject
{
    public class FollowTheRoadBrainCentre
    {
        CarController carController;
        PIDRegulator regulator;
        VisionFilters.Output.RoadCenterDetector roadCenterDetector;
        VisionFilters.Output.ColorVideoSource<byte> colorVideoSource;

        class Settings : PIDSettings
        {
            public Settings()
            {
                //P part settings
                P_FACTOR_MULTIPLER = 2;

                //I part settings
                I_FACTOR_MULTIPLER = 0; //hypys radzi, żeby to wyłączyć bo może być niestabilny (a tego baardzo nie chcemy)
                I_FACTOR_SUM_MAX_VALUE = 100;
                I_FACTOR_SUM_MIN_VALUE = -100;
                I_FACTOR_SUM_SUPPRESSION_PER_SEC = 0.96; // = 0.88; //1.0 = suppresing disabled

                //D part settings
                D_FACTOR_MULTIPLER = 1;
                D_FACTOR_SUPPRESSION_PER_SEC = 0.7;
                D_FACTOR_SUM_MIN_VALUE = 100;
                D_FACTOR_SUM_MAX_VALUE = -100;

                //steering limits
                MAX_FACTOR_CONST = 100; // = 100.0;
                MIN_FACTOR_CONST = -100; // = -100.0;
            }
        }


        public FollowTheRoadBrainCentre()
        {
            carController = new CarController();
            carController.SetTargetSpeed(0);
            carController.SetTargetWheelAngle(0);

            regulator = new PIDRegulator(new Settings(), "carSteeringRegulator");
            regulator.SetTargetValue(0.0); //we want to go straight with the road

            colorVideoSource = new VisionFilters.Output.ColorVideoSource<byte>();
            //roadCenterDetector = new VisionFilters.Output.RoadCenterDetector(colorVideoSource);

            roadCenterDetector.RoadCenterSupply += new VisionFilters.Output.RoadCenterHandler(roadCenterDetector_RoadCenterSupply);
        }

        double[] pointsWages = new double[3]{ 10.0, 2.0, 0.5 };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// e.road are points on road - 0 is the closest
        /// right now we get 3 point on the road
        /// </param>
        void roadCenterDetector_RoadCenterSupply(object sender, VisionFilters.Output.RoadCenterEvent e)
        {
            double currentValue = 0;
            for(int i = 0; i < 3; i++)
            {
                currentValue += e.road[i].X * pointsWages[i];
            }
            if(Limiter.LimitAndReturnTrueIfLimitted(ref currentValue, -100, 100))
            {
                Logger.Log(this, "road value has been limmited", 1);
            }

            double steeringVal = regulator.ProvideObjectCurrentValueToRegulator(currentValue);

            carController.SetTargetWheelAngle(steeringVal);    
        }

    }
}
