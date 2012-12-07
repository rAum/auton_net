using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using car_communicator;

namespace autonomiczny_samochod
{
    public class RealCarCommunicator : ICarCommunicator
    {
        public event SpeedInfoReceivedEventHander evSpeedInfoReceived;
        public event SteeringWheelAngleInfoReceivedEventHandler evSteeringWheelAngleInfoReceived;
        public event BrakePositionReceivedEventHandler evBrakePositionReceived;

        public ISpeedRegulator ISpeedRegulator 
        { 
            get
            {
                return ICar.SpeedRegulator;
            } 
        }
        public ISteeringWheelAngleRegulator ISteeringWheelAngleRegulator
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

        public ICar ICar { get; private set; }

        //CONST
        const int SPEED_MEASURING_TIMER_INTERVAL_IN_MS = 10; //in ms
        const int SPEED_TABLE_SIZE = 30;
        const double WHEEL_CIRCUIT_IN_M = 1.3822996; //44cm * pi //TODO: check it - its probably wrong
        const int NO_OF_HAAL_METERS = 5;
        const int TICKS_TO_RESTART = 10000; 

        //sub-communicators
       // private BrakePedalCommunicator brakePedalCommunicator { get; set; } //obsolete
        private USB4702 extentionCardCommunicator { get; set; }
        private ServoDriver servoDriver { get; set; }
        private RS232Controller angleAndSpeedMeter { get; set; }


        //car speed receiving
        System.Windows.Forms.Timer SpeedMeasuringTimer = new System.Windows.Forms.Timer();
        int [] lastTicksMeasurements = new int[SPEED_TABLE_SIZE];
        int tickTableIterator = 0;
        int lastTicks = 0;


        public RealCarCommunicator(ICar parent)
        {
            ICar = parent;

            extentionCardCommunicator = new USB4702();
            servoDriver = new ServoDriver();
            angleAndSpeedMeter = new RS232Controller(this);

            //TODO: make thread for every initialization //its actually done for angleAndSpeedMeter
            extentionCardCommunicator.Initialize();
            servoDriver.Initialize();
            angleAndSpeedMeter.Initialize();
            
            SpeedMeasuringTimer.Interval = SPEED_MEASURING_TIMER_INTERVAL_IN_MS;
            SpeedMeasuringTimer.Tick += new EventHandler(SpeedMeasuringTimer_Tick);
            SpeedMeasuringTimer.Start();
        }

        void SpeedMeasuringTimer_Tick(object sender, EventArgs e)
        {
            //read
 	        int ticks = extentionCardCommunicator.getSpeedCounterStatus();

            //calculations
            lastTicksMeasurements[tickTableIterator] = ticks - lastTicks;
            
            tickTableIterator = (tickTableIterator + 1) % SPEED_TABLE_SIZE;

            double speed = Convert.ToDouble(WHEEL_CIRCUIT_IN_M) / Convert.ToDouble(NO_OF_HAAL_METERS) * Convert.ToDouble(lastTicksMeasurements.Sum()) / (Convert.ToDouble(SPEED_TABLE_SIZE) * Convert.ToDouble(SPEED_MEASURING_TIMER_INTERVAL_IN_MS) / 1000.0);

            if(ticks > TICKS_TO_RESTART)
            {
                extentionCardCommunicator.RestartSpeedCounter();
                lastTicks = 0;
            }

            //sending event
            SpeedInfoReceivedEventHander SpeedEvent = evSpeedInfoReceived;
            if (SpeedEvent != null)
            {
                SpeedEvent(this, new SpeedInfoReceivedEventArgs(speed));
            }
            lastTicks = ticks;
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

        public void SendNewSpeedSettingMessage(double speedSetting)
        {
            throw new NotImplementedException();
        }

        public void SendNewSteeringWheelAngleSettingMessage(double angleSetting)
        {
            throw new NotImplementedException();
        }

        public bool IsInitiated()
        {
            return true; //TODO: fix it
           // return (brakePedalCommunicator.IsInitiated() && accelerationPedalCommunivator.IsInitiated() && steeringWheelCommunicator.IsInitiated());
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
