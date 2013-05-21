using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Helpers;
using System.Timers;

namespace CarController
{
    public class FakeCarCommunicator : ICarCommunicator
    {
        public event SpeedInfoReceivedEventHander evSpeedInfoReceived;
        public event SteeringWheelAngleInfoReceivedEventHandler evSteeringWheelAngleInfoReceived;
        public event BrakePositionReceivedEventHandler evBrakePositionReceived;

        private CarModel model;

        public ICar ICar { get; private set; }
        public ISpeedRegulator SpeedRegulator
        {
            get
            {
                return ICar.SpeedRegulator;
            }
        }
        public ISteeringWheelAngleRegulator SteeringWheelAngleRegulator
        {
            get
            {
                return ICar.SteeringWheelAngleRegulator;
            }
        }
        public IBrakeRegulator BrakeRegulator
        {
            get
            {
                return ICar.BrakeRegulator;
            }
        }

        private System.Timers.Timer timer = new System.Timers.Timer();
        private const int TIMER_INTERVAL_IN_MS = 10;

        public FakeCarCommunicator(ICar car)
        {
            ICar = car;
            //mFakeThread = new System.Threading.Thread(new ThreadStart(mFakeThreadTasks));
            //mFakeThread.Start();

            model = new CarModel(this);

            timer.Interval = TIMER_INTERVAL_IN_MS;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        /// <summary>
        /// this has to be invoked before 1st use
        /// </summary>
        public void InitRegulatorsEventsHandling()
        {
            SpeedRegulator.evNewSpeedSettingCalculated += new NewSpeedSettingCalculatedEventHandler(ISpeedRegulator_evNewSpeedSettingCalculated);
            SteeringWheelAngleRegulator.evNewSteeringWheelSettingCalculated += new NewSteeringWheelSettingCalculatedEventHandler(ISteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated);
            BrakeRegulator.evNewBrakeSettingCalculated += new NewBrakeSettingCalculatedEventHandler(BrakeRegulator_evNewBrakeSettingCalculated);
        }

        void BrakeRegulator_evNewBrakeSettingCalculated(object sender, NewBrakeSettingCalculatedEventArgs args)
        {
            model.BrakeSteering = args.GetBrakeSetting();
        }

        void ISteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated(object sender, NewSteeringWheelSettingCalculateddEventArgs args)
        {
            model.WheelAngleSteering = args.getSteeringWheelAngleSetting();
        }

        void ISpeedRegulator_evNewSpeedSettingCalculated(object sender, NewSpeedSettingCalculatedEventArgs args)
        {
            model.SpeedSteering = args.getSpeedSetting();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SteeringWheelAngleInfoReceivedEventHandler tempAngleEvent = evSteeringWheelAngleInfoReceived;
            if (tempAngleEvent != null)
            {
                tempAngleEvent(this, new SteeringWheelAngleInfoReceivedEventArgs(model.WheelAngle));
            }

            SpeedInfoReceivedEventHander tempSpeedEvent = evSpeedInfoReceived;
            if (tempSpeedEvent != null)
            {
                tempSpeedEvent(this, new SpeedInfoReceivedEventArgs(model.Speed));
            }

            BrakePositionReceivedEventHandler temp = evBrakePositionReceived;
            if (temp != null)
            {
                temp(this, new BrakePositionReceivedEventArgs(model.BrakePosition));
            }
        }

        void mFakeThreadTasks()
        {
            System.Threading.Thread.Sleep(1000); //wait 1s
            evSpeedInfoReceived(this, new SpeedInfoReceivedEventArgs(25.0));
            evSteeringWheelAngleInfoReceived.Invoke(this, new SteeringWheelAngleInfoReceivedEventArgs(10.0));

            System.Threading.Thread.Sleep(1000); //wait 1s
            evSpeedInfoReceived(this, new SpeedInfoReceivedEventArgs(30.0));
            evSteeringWheelAngleInfoReceived.Invoke(this, new SteeringWheelAngleInfoReceivedEventArgs(5.0));

            System.Threading.Thread.Sleep(1000); //wait 1s
            evSpeedInfoReceived(this, new SpeedInfoReceivedEventArgs(35.0));
            evSteeringWheelAngleInfoReceived.Invoke(this, new SteeringWheelAngleInfoReceivedEventArgs(0.0));
        }

        public void SetGear(Gear gear)
        {
            model.Gear = gear;
        }
    }
}

