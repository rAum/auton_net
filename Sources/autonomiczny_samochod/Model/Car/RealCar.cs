using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using autonomiczny_samochod.Model.Regulators;

namespace autonomiczny_samochod.Model.Car
{
    public class RealCar : ICar
    {
        public event EventHandler evAlertBrake;
        public event TargetSpeedChangedEventHandler evTargetSpeedChanged;
        public event TargetSteeringWheelAngleChangedEventHandler evTargetSteeringWheelAngleChanged;

        public ICarCommunicator CarComunicator { get; private set; }
        public CarController Controller { get; private set; }
        public ISpeedRegulator SpeedRegulator { get; private set; }
        public ISteeringWheelAngleRegulator SteeringWheelAngleRegulator { get; private set; }
        public IBrakeRegulator BrakeRegulator { get; private set; }

        public bool IsAlertBrakeActive { get; private set; }
        public CarInformations CarInfo { get; private set; }

        public RealCar(CarController parent)
        {
            Controller = parent;
            CarInfo = new CarInformations();

            IsAlertBrakeActive = false;

            //regulators and communicator initiation 
            CarComunicator = new RealCarCommunicator(this); // = new RealCarCommunicator(this);

            SpeedRegulator = new PIDSpeedRegulator(this);
            //SpeedRegulator = new FakeSpeedRegulator();

            //SteeringWheelAngleRegulator = new SimpleSteeringWheelRegulator(this);
            SteeringWheelAngleRegulator = new PIDSteeringWheelAngleRegulator(this);
            //SteeringWheelAngleRegulator = new FakeSteeringWheelRegulator();

            BrakeRegulator = new PIDBrakeRegulator(this);

            CarComunicator.InitRegulatorsEventsHandling();  //TODO: REFACTOR THIS SHIT!!! //for now this is needed, because reagulators does not exists when communicator constructor is invoked

            //internal event handling initialization
            evAlertBrake += new EventHandler(ExampleFakeCar_evAlertBrake);
            evTargetSpeedChanged += new TargetSpeedChangedEventHandler(ExampleFakeCar_evTargetSpeedChanged);
            evTargetSteeringWheelAngleChanged += new TargetSteeringWheelAngleChangedEventHandler(ExampleFakeCar_evTargetSteeringWheelAngleChanged);
        }

        private void ExampleFakeCar_evTargetSteeringWheelAngleChanged(object sender, TargetSteeringWheelAngleChangedEventArgs args)
        {
            Logger.Log(this, String.Format("target wheel angle changed to: {0}", args.GetTargetWheelAngle()));
        }
        private void ExampleFakeCar_evTargetSpeedChanged(object sender, TargetSpeedChangedEventArgs args)
        {
            Logger.Log(this, String.Format("target speed changed to: {0}", args.GetTargetSpeed()));
        }
        private void ExampleFakeCar_evAlertBrake(object sender, EventArgs e)
        {
            Logger.Log(this, "ALERT BRAKE!");
        }

        public void ActivateAlertBrake()
        {
            EventHandler temp = evAlertBrake;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }
        public void SetTargetSpeed(double speed)
        {
            CarInfo.TargetSpeed = speed;

            TargetSpeedChangedEventHandler temp = evTargetSpeedChanged;
            if (temp != null)
            {
                temp(this, new TargetSpeedChangedEventArgs(speed));
            }
        }
        public void SetTargetWheelAngle(double angle)
        {
            CarInfo.TargetWheelAngle = angle;

            TargetSteeringWheelAngleChangedEventHandler temp = evTargetSteeringWheelAngleChanged;
            if (temp != null)
            {
                temp(this, new TargetSteeringWheelAngleChangedEventArgs(angle));
            }
        }

    }
}
