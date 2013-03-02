using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Automation.BDaq;
using Helpers;
using CarController.Model.Communicators;
using System.Threading;

namespace car_communicator
{
    /// <summary>
    /// extension card connector
    /// </summary>
    public class USB4702 : Device
    {

        public override string ToString()
        {
            return "USB4702";    
        }

        static int buffer;
        static InstantAoCtrl instantAoCtrl = new InstantAoCtrl(); //for initialize analog outputs
        static InstantDoCtrl instantDoCtrl = new InstantDoCtrl(); //for initialize digital outputs
        static EventCounterCtrl eventSpeedCounterCtrl = new EventCounterCtrl(); // for initialize counter

        const string USB4702_DEVICE_DESCRIPTION_STRING = "USB-4702,BID#0"; // '0' -> 1st extension card

        const int MAX_TRIES_TO_INITIALIZE_BEFORE_ERROR = 60;
        const int SLEEP_BEETWEEN_TRIES_TO_INITIALIZE_IN_MS = 10; //needed, because initialization uses all the system resources when it is in initialization loop

        const int STEERING_WHEEL_SET_PORT = 0;
        const double STEERING_WHEEL_MIN_SET_VALUE_IN_VOLTS = 1.2; //1.0 can cause error but its teoretical min
        const double STEERING_WHEEL_MID_SET_VALUE_IN_VOLTS = 2.5; // środek
        const double STEERING_WHEEL_MAX_SET_VALUE_IN_VOLTS = 3.8; //4.0 is teoretical max

        const int BRAKE_STRENGTH_SET_PORT = 1;
        const double BRAKE_MIN_SET_VALUE_IN_VOLTS = 0.8; //from 0
        const double BRAKE_NEUTRAL_STRENGTH_IN_VOLTS = 2.5;
        const double BRAKE_MAX_SET_VALUE_IN_VOLST = 4.2; //from 5

        const double MIN_STRENGTH_FOR_BRAKE_TO_REACT_IN_PERCENTS = 20;

        const int BRAKE_ENABLE_PORT_NO = 0;
        const byte BRAKE_ENABLE_ON_PORT_LEVEL = 0; 
        const byte BRAKE_ENABLE_OFF_PORT_LEVEL = 1;

        const int BRAKE_STOP_PORT_NO = 2;
        const byte BRAKE_STOP_ON_PORT_LEVEL = 0;
        const byte BRAKE_STOP_OFF_PORT_LEVEL = 1;

        const int BRAKE_DIRECTION_PORT_NO = 1;
        const int BRAKE_BACKWARD_PORT_LEVEL = 1; 
        const int BRAKE_FORWARD_PORT_LEVEL = 0;

        const int BRAKE_SOFT_START_PORT_NO = 3; 
        const int BRAKE_SOFT_START_ON_PORT_LEVEL = 1;
        const int BRAKE_SOFT_START_OFF_PORT_LEVEL = 0;

        const int IGNITION_PORT_NO = 4; //zaplon
        const int IGNITION_ON_PORT_LEVEL = 0;
        const int IGNITION_OFF_PORT_LEVEL = 1;
        const int IGNTITION_OFF_TIME_IN_MS = 5000;
        const int IGNITION_ON_TIME_IN_MS = 3500;

        const int STARTER_PORT_NO = 5; //rozrusznik
        const int STARTER_ON_PORT_LEVEL = 1;
        const int STARTER_OFF_PORT_LEVEL = 0;
        const int STARTER_ON_TIME_IN_MS = 3500;

        const int STEERING_WHEEL_ENABLER_PORT_NO = 3; //if you dont turn it on you wouldn't be able to move wheel (it'll be set to 0 power (2,5V))
        const int STEERING_WHEEL_ENABLER_ON_PORT_LEVEL = 1;
        const int STEERING_WHEEL_ENABLER_OFF_PORT_LEVEL = 0;

        private bool carEngineEnabled = false;
        private bool effectorsActive = false;

        protected override void Initialize()
        {
            TryToInitializeUntillItSucceds();
        }

        private void TryToInitializeUntillItSucceds()
        {
            bool initializationFinished = false;
            int tries = 0;
            do
            {
                if (++tries > MAX_TRIES_TO_INITIALIZE_BEFORE_ERROR)
                {
                    Logger.Log(this, "USB4702 could not be initialized - max tries no hes been exceeded", 3);
                    this.overallState = DeviceOverallState.Error;
                    initializationFinished = true; //it failed, but its end of trying
                }
                try
                {
                    //Analog outputs
                    instantAoCtrl.SelectedDevice = new DeviceInformation(USB4702_DEVICE_DESCRIPTION_STRING); // AO0

                    //Digital output
                    instantDoCtrl.SelectedDevice = new DeviceInformation(USB4702_DEVICE_DESCRIPTION_STRING);

                    //Counter
                    eventSpeedCounterCtrl.SelectedDevice = new DeviceInformation(USB4702_DEVICE_DESCRIPTION_STRING);

                    eventSpeedCounterCtrl.Channel = 0;
                    eventSpeedCounterCtrl.Enabled = true; //IMPORTANT: was ----> // false; // block counter

                    setPortDO(BRAKE_ENABLE_PORT_NO, BRAKE_ENABLE_ON_PORT_LEVEL); //enabling brake engine
                    setPortDO(BRAKE_STOP_PORT_NO, BRAKE_STOP_OFF_PORT_LEVEL);

                    initializationFinished = true; //no exceptions and get here - everything is ok!
                }
                catch (Exception e)
                {
                    Logger.Log(this, String.Format("cannot initialize connection for USB4702, tries left: {0}", MAX_TRIES_TO_INITIALIZE_BEFORE_ERROR - tries), 2);
                    Logger.Log(this, String.Format("Exception received: {0}", e.Message), 2);
                }
            } while (!initializationFinished);
        }

        protected override void StartSensors()
        {
            //no sensors in here
        }

        protected override void StartEffectors()
        {
            EnableSteeringWheelSteering();
            TurnOnEngine();

            effectorsActive = true;
        }

        protected override void PauseEffectors()
        {
            DisableSteeringWheelSteering();
            TurnOffEngine();

            effectorsActive = false;
            SetSteeringWheel(0.0); //do not move steering wheel
        }

        protected override void EmergencyStop()
        {
            DisableSteeringWheelSteering();
            TurnOffEngine();

            effectorsActive = false;
            SetSteeringWheel(0.0); //do not move steering wheel
        }
        
        /// <summary>
        /// sets "value" voltage on channel no "channel"
        /// </summary>
        /// <param name="channel">channel no</param>
        /// <param name="value">0-5V (will be checked anyway - throws if bad)</param>
        private void setPortAO(int channel, double value)
        {
            if (effectorsActive)
            {
                if (value > 5 || value < 0)
                    throw new ArgumentException("value is not in range", "value");

                try
                {
                    instantAoCtrl.Write(channel, value);
                }
                catch (InvalidCastException)
                {
                    Logger.Log(this, "msg couldn't been send via USB4702 - probably because of no connection", 2);
                }
            }
            else
            {
                Logger.Log(this, "analog port was not set, because effectors are no active");
            }
        }

        /// <summary>
        /// sets 0/1 "level" on port "port"
        /// </summary>
        /// <param name="port">port no</param>
        /// <param name="level">0/1</param>
        private void setPortDO(int port, byte level)
        {
            if (effectorsActive)
            {
                try
                {
                    if (level == 1)
                    {
                        buffer |= (1 << port);
                        instantDoCtrl.Write(0, (byte)buffer);
                    }
                    else if (level == 0)
                    {
                        buffer &= ~(1 << port);
                        instantDoCtrl.Write(0, (byte)buffer);
                    }
                    else
                    {
                        throw new ArgumentException("wrong level value - it should be 0/1", "level");
                    }

                }
                catch (Exception)
                {
                    Logger.Log(this, "msg couldn't been send via USB4702 - probably because of no connection", 2);
                }
            }
            else
            {
                Logger.Log(this, "analog port was not set, because effectors are no active");
            }
        }

        //working
        public void RestartSpeedCounter()
        {
            eventSpeedCounterCtrl.Enabled = false;
            eventSpeedCounterCtrl.Enabled = true;
        }

        public int getSpeedCounterStatus()
        {
            if (!eventSpeedCounterCtrl.Initialized)
            {
                Logger.Log(this, "Speed counter is not initialized and cannot provide any data", 2);
                return 0;
            }
            return eventSpeedCounterCtrl.Value;
            
        }

        private void TurnOnEngine()
        {
            setPortDO(IGNITION_PORT_NO, IGNITION_ON_PORT_LEVEL);
            Thread.Sleep(IGNITION_ON_TIME_IN_MS);
            setPortDO(STARTER_PORT_NO, STARTER_ON_PORT_LEVEL);
            Thread.Sleep(STARTER_ON_TIME_IN_MS);
            setPortDO(STARTER_PORT_NO, STARTER_OFF_PORT_LEVEL);
        }

        private void TurnOffEngine()
        {
            setPortDO(IGNITION_PORT_NO, IGNITION_ON_PORT_LEVEL);
            Thread.Sleep(IGNTITION_OFF_TIME_IN_MS);
            setPortDO(IGNITION_PORT_NO, IGNITION_OFF_PORT_LEVEL);
        }

        private void DisableSteeringWheelSteering()
        {
            setPortDO(STEERING_WHEEL_ENABLER_PORT_NO, STEERING_WHEEL_ENABLER_OFF_PORT_LEVEL);
        }

        private void EnableSteeringWheelSteering()
        {
            setPortDO(STEERING_WHEEL_ENABLER_PORT_NO, STEERING_WHEEL_ENABLER_ON_PORT_LEVEL);
        }

        /// <summary>
        /// sets power send to steering wheel in percents of power 
        /// (minus values are counterclockwise, plus values are clockwise)
        /// </summary>
        /// <param name="strength">
        /// -100 = max counterclockwise [in percents]
        /// +100 = max clockwise [in percents]
        /// </param>
        public void SetSteeringWheel(double strength)
        {
            if (strength < -100 || strength > 100)
            {
                Logger.Log(this, "steering wheel strength is not in range", 2);
                throw new ArgumentException("strenght is not in range");
            }

            Helpers.ReScaller.ReScale(ref strength, -100, 100, STEERING_WHEEL_MIN_SET_VALUE_IN_VOLTS, STEERING_WHEEL_MAX_SET_VALUE_IN_VOLTS);

            setPortAO(STEERING_WHEEL_SET_PORT, strength);
        }

        /// <summary>
        /// sets power send to engine steering brake pedal
        /// </summary>
        /// <param name="strength">
        /// -100 = max pulling brake pedal
        /// +100 = max pushing brake pedal
        /// </param>
        public void SetBrake(double strength)
        {
            Logger.Log(this, String.Format("Brake steering value is being set to: {0}%", strength));

            if (strength < -100 || strength > 100)
            {
                Logger.Log(this, "strength is not in range", 2);
                throw new ArgumentException("strenght is not in range");
            }

            if (strength < 0) //backward
            {
                strength *= -1;
                setPortDO(BRAKE_DIRECTION_PORT_NO, BRAKE_BACKWARD_PORT_LEVEL);
            }
            else //forward
            {
                setPortDO(BRAKE_DIRECTION_PORT_NO, BRAKE_FORWARD_PORT_LEVEL);
            }

            //to dont waste engine - when steering value is low - we are stopping engine 
            if (strength < MIN_STRENGTH_FOR_BRAKE_TO_REACT_IN_PERCENTS) //strength should be always > 0 in here
            {
                setPortDO(BRAKE_STOP_PORT_NO, BRAKE_STOP_ON_PORT_LEVEL);
                setPortAO(BRAKE_STRENGTH_SET_PORT, BRAKE_NEUTRAL_STRENGTH_IN_VOLTS);
            }
            else
            {
                setPortDO(BRAKE_STOP_PORT_NO, BRAKE_STOP_OFF_PORT_LEVEL);
                Helpers.ReScaller.ReScale(ref strength, 0, 100, BRAKE_MIN_SET_VALUE_IN_VOLTS, BRAKE_MAX_SET_VALUE_IN_VOLST);

                setPortAO(BRAKE_STRENGTH_SET_PORT, strength);
            }
            //setPortDO(BRAKE_STOP_PORT_NO, BRAKE_STOP_OFF_PORT_LEVEL);
            //setPortDO(BRAKE_DIRECTION_PORT_NO, BRAKE_FORWARD_PORT_LEVEL);
            //setPortAO(BRAKE_STRENGTH_SET_PORT, 2);
        }

    }

}
