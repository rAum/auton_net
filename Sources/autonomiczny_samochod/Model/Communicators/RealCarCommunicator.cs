using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using car_communicator;
using CarController.Model.Communicators;
using Helpers;

namespace CarController
{
    public class RealCarCommunicator : ICarCommunicator
    {
        public event SpeedInfoReceivedEventHander evSpeedInfoReceived;
        public event SteeringWheelAngleInfoReceivedEventHandler evSteeringWheelAngleInfoReceived;
        public event BrakePositionReceivedEventHandler evBrakePositionReceived;

        public ISpeedRegulator ISpeedRegulator { get { return ICar.SpeedRegulator; } }
        public ISteeringWheelAngleRegulator ISteeringWheelAngleRegulator { get { return ICar.SteeringWheelAngleRegulator; } }
        public IBrakeRegulator BrakeRegulator { get { return ICar.BrakeRegulator; } }
        public DeviceManager deviceManager { get { return ICar.deviceManager; } }

        public ICar ICar { get; private set; }

        //sub-communicators
        private USB4702 extentionCardCommunicator { get; set; }
        private ServoDriver servoDriver { get; set; }
        private SafeRS232Controller angleAndSpeedMeter { get; set; }
        private Speedometer speedometer { get; set; }

        public RealCarCommunicator(ICar parent)
        {
            ICar = parent;

            servoDriver = new ServoDriver();
            deviceManager.RegisterDevice(servoDriver);

            angleAndSpeedMeter = new SafeRS232Controller(this, "COM4");
            deviceManager.RegisterDevice(angleAndSpeedMeter);

            extentionCardCommunicator = new USB4702();
            deviceManager.RegisterDevice(extentionCardCommunicator);

            speedometer = new Speedometer(extentionCardCommunicator);
            deviceManager.RegisterDevice(speedometer);

            speedometer.evSpeedInfoReceived += speedometer_evSpeedInfoReceived;
        }

        void speedometer_evSpeedInfoReceived(object sender, SpeedInfoReceivedEventArgs args)
        {
            SpeedInfoReceivedEventHander temp = evSpeedInfoReceived;
            if (temp != null)
            {
                temp(this, new SpeedInfoReceivedEventArgs(args.GetSpeedInfo()));
            }
        }

        /// <summary>
        /// this has to be invoked before 1st use
        /// </summary>
        public void InitRegulatorsEventsHandling()
        {
            ISpeedRegulator.evNewSpeedSettingCalculated += new NewSpeedSettingCalculatedEventHandler(ISpeedRegulator_evNewSpeedSettingCalculated);
            ISteeringWheelAngleRegulator.evNewSteeringWheelSettingCalculated += new NewSteeringWheelSettingCalculatedEventHandler(ISteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated);
            BrakeRegulator.evNewBrakeSettingCalculated += new NewBrakeSettingCalculatedEventHandler(BrakeRegulator_evNewBrakeSettingCalculated);
        }

        void BrakeRegulator_evNewBrakeSettingCalculated(object sender, NewBrakeSettingCalculatedEventArgs args)
        {
            extentionCardCommunicator.SetBrake(args.GetBrakeSetting());
        }

        void ISteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated(object sender, NewSteeringWheelSettingCalculateddEventArgs args)
        {
            extentionCardCommunicator.SetSteeringWheel(args.getSteeringWheelAngleSetting());
        }

        void ISpeedRegulator_evNewSpeedSettingCalculated(object sender, NewSpeedSettingCalculatedEventArgs args)
        {
            if (args.getSpeedSetting() > 0)
            {
                servoDriver.setThrottle(args.getSpeedSetting());
            }
            else
            {
                servoDriver.setThrottle(0.0);
            }
        }

        internal void WheelAngleAcquired(double angle)
        {
            SteeringWheelAngleInfoReceivedEventHandler evWheelAngleReceived = evSteeringWheelAngleInfoReceived;
            if (evWheelAngleReceived != null)
            {
                evWheelAngleReceived(this, new SteeringWheelAngleInfoReceivedEventArgs(angle));
            }
        }

        internal void BrakePositionsAcquired(double position)
        {
            BrakePositionReceivedEventHandler brakePosReceived = evBrakePositionReceived;
            if (brakePosReceived != null)
            {
                brakePosReceived(this, new BrakePositionReceivedEventArgs(position));
            }
        }

        public void SetGear(Gear gear)
        {
            servoDriver.setGear(gear);
        }
    }
}
