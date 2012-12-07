using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using autonomiczny_samochod;
using Helpers;

namespace autonomiczny_samochod
{
    //TODO: provide some logs
    public class CarController
    {
        public ICar Model { get; private set; }
        public MainWindow MainWindow { get; private set; }
        private System.Threading.Thread mFakeSignalsSenderThread;

        //stats collecting
        private StatsCollector statsCollector = new StatsCollector();
        private int TICKS_TO_SAVE_STATS = 250;
        private System.Windows.Forms.Timer mStatsCollectorTimer = new System.Windows.Forms.Timer();
        private const int TIMER_INTERVAL_IN_MS = 10;

        public CarController(MainWindow window)
        {
            MainWindow = window;

            //Model = new ExampleFakeCar(this);
            Model = new autonomiczny_samochod.Model.Car.RealCar(this);
            //Model = new CarWithFakeRegulators(this);
            //Model = new CarWithFakeCommunicator(this);

            Model.SetTargetSpeed(0.0);
            Model.SetTargetWheelAngle(0.0);

            EventHandlingForStatsCollectingInit();

            //timer init
            mStatsCollectorTimer.Interval = TIMER_INTERVAL_IN_MS;
            mStatsCollectorTimer.Tick += new EventHandler(mStatsCollectorTimer_Tick);
            mStatsCollectorTimer.Start();

            //mFakeSignalsSenderThread = new System.Threading.Thread(new System.Threading.ThreadStart(mFakeSignalsSenderFoo));
            //mFakeSignalsSenderThread.Start();
        }

        private void EventHandlingForStatsCollectingInit()
        {
            Model.CarComunicator.evSpeedInfoReceived += new SpeedInfoReceivedEventHander(CarComunicator_evSpeedInfoReceived);
            Model.CarComunicator.evSteeringWheelAngleInfoReceived += new SteeringWheelAngleInfoReceivedEventHandler(CarComunicator_evSteeringWheelAngleInfoReceived);
            Model.CarComunicator.evBrakePositionReceived += new BrakePositionReceivedEventHandler(CarComunicator_evBrakePositionReceived);

            Model.SteeringWheelAngleRegulator.evNewSteeringWheelSettingCalculated += new NewSteeringWheelSettingCalculatedEventHandler(SteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated);
            Model.SpeedRegulator.evNewSpeedSettingCalculated += new NewSpeedSettingCalculatedEventHandler(SpeedRegulator_evNewSpeedSettingCalculated);
            Model.BrakeRegulator.evNewBrakeSettingCalculated += new NewBrakeSettingCalculatedEventHandler(BrakeRegulator_evNewBrakeSettingCalculated);
        }

        private void SteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated(object sender, NewSteeringWheelSettingCalculateddEventArgs args)
        {
            Model.CarInfo.WheelAngleSteering = args.getSteeringWheelAngleSetting();
        }

        private void BrakeRegulator_evNewBrakeSettingCalculated(object sender, NewBrakeSettingCalculatedEventArgs args)
        {
            Model.CarInfo.BrakeSteering = args.GetBrakeSetting();
        }
        private void SpeedRegulator_evNewSpeedSettingCalculated(object sender, NewSpeedSettingCalculatedEventArgs args)
        {
            Model.CarInfo.SpeedSteering = args.getSpeedSetting();
            Model.CarInfo.TargetBrake = args.getSpeedSetting() * -1; //TODO: check this
        }
        private void CarComunicator_evBrakePositionReceived(object sender, BrakePositionReceivedEventArgs args)
        {
            Model.CarInfo.CurrentBrake = args.GetPosition();
        }
        private void CarComunicator_evSteeringWheelAngleInfoReceived(object sender, SteeringWheelAngleInfoReceivedEventArgs args)
        {
            Model.CarInfo.CurrentWheelAngle = args.GetAngle();
        }
        private void CarComunicator_evSpeedInfoReceived(object sender, SpeedInfoReceivedEventArgs args)
        {
            Model.CarInfo.CurrentSpeed = args.GetSpeedInfo();
        }


        void mStatsCollectorTimer_Tick(object sender, EventArgs e)
        {
            //stats collecting
            statsCollector.PutNewStat("time", Time.GetTimeFromProgramBeginnig().TotalMilliseconds);
            statsCollector.PutNewStat("current speed", Model.CarInfo.CurrentSpeed);
            statsCollector.PutNewStat("target speed", Model.CarInfo.TargetSpeed);
            statsCollector.PutNewStat("speed steering", Model.CarInfo.SpeedSteering);
            statsCollector.PutNewStat("current angle", Model.CarInfo.CurrentWheelAngle);
            statsCollector.PutNewStat("target angle", Model.CarInfo.TargetWheelAngle);
            statsCollector.PutNewStat("angle steering", Model.CarInfo.WheelAngleSteering);
            statsCollector.PutNewStat("current brake", Model.CarInfo.CurrentBrake);
            statsCollector.PutNewStat("target brake", Model.CarInfo.TargetBrake);
            statsCollector.PutNewStat("brake steering", Model.CarInfo.BrakeSteering);

            //collecting speed regulator parameters
            var speedRegulatorParameters = Model.SpeedRegulator.GetRegulatorParameters();
            foreach (string key in speedRegulatorParameters.Keys)
            {
                statsCollector.PutNewStat(String.Format("SpeedRegulator_{0}", key), speedRegulatorParameters[key]);
            }
        }

        public void SaveStatsToFile(string fileName)
        {
            statsCollector.WriteStatsToFile(fileName);
            Logger.Log(this, "----------------------------------------------------------------");
            Logger.Log(this, "----------------------------------------------------------------");
            Logger.Log(this, "----------------------------------------------------------------");
            Logger.Log(this, "----------------------------------------------------------------");
            Logger.Log(this, String.Format("STATS HAS BEEN WRITTEN TO FILE: stats.txt"));
            Logger.Log(this, "----------------------------------------------------------------");
            Logger.Log(this, "----------------------------------------------------------------");
            Logger.Log(this, "----------------------------------------------------------------");
            Logger.Log(this, "----------------------------------------------------------------");
        }

        void mFakeSignalsSenderFoo()
        {
            System.Threading.Thread.Sleep(1000);

            Model.SetTargetSpeed(50.0);
            Model.SetTargetWheelAngle(60.0);

            System.Threading.Thread.Sleep(1000);

            Model.SetTargetSpeed(25.0);
            Model.SetTargetWheelAngle(30.0);
        }

        public CarInformations GetCarInformation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// sets target wheel angle in degrees
        ///     right -> angle > 0
        ///     left  -> angle < 0
        /// </summary>
        /// <param name="targetAngle"></param>
        public void SetTargetWheelAngle(double targetAngle)
        {
            Model.SetTargetWheelAngle(targetAngle);
        }

        /// <summary>
        /// sets target speed in km/h
        /// </summary>
        /// <param name="setTargetSpeed"></param>
        public void SetTargetSpeed(double targetSpeed)
        {
            Model.SetTargetSpeed(targetSpeed);
        }

        public void ChangeTargetSpeed(double change)
        {
            Model.SetTargetSpeed(Model.CarInfo.TargetSpeed + change);
        }

        public void ChangeTargetWheelAngle(double change)
        {
            Model.SetTargetWheelAngle(Model.CarInfo.TargetWheelAngle + change);
        }

        public void AlertBrake()
        {
            Model.ActivateAlertBrake();
        }
    }
}
