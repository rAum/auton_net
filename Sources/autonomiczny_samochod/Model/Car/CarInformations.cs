using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace autonomiczny_samochod
{
    public class CarInformations
    {
        //speed
        public double CurrentSpeed { get; set; }
        public double TargetSpeed { get; set; }
        public double SpeedSteering { get; set; }
        
        //brake
        public double CurrentBrake { get; set; }
        public double TargetBrake { get; set; }
        public double BrakeSteering { get; set; }

        //wheel angle
        public double CurrentWheelAngle { get; set; }
        public double TargetWheelAngle { get; set; }
        public double WheelAngleSteering{ get; set; }

        //alert brake
        public bool AlertBrakeActive { get; set; }

        public CarInformations()
        {
            CurrentSpeed = double.NaN;
            TargetSpeed = double.NaN;
            SpeedSteering = double.NaN;

            CurrentBrake = double.NaN;
            TargetBrake = double.NaN;
            BrakeSteering = double.NaN;

            CurrentWheelAngle = double.NaN;
            TargetSpeed = double.NaN;
            WheelAngleSteering = double.NaN;

            AlertBrakeActive = false;
        }
    }
}
