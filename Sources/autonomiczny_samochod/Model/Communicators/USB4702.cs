using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Automation.BDaq;
using Helpers;

namespace car_communicator
{
    /// <summary>
    /// extension card connector
    /// </summary>
    public class USB4702
    {
        static int buffer;
        static InstantAoCtrl instantAoCtrl = new InstantAoCtrl(); //for initialize analog outputs
        static InstantDoCtrl instantDoCtrl = new InstantDoCtrl(); //for initialize digital outputs
        static EventCounterCtrl eventSpeedCounterCtrl = new EventCounterCtrl(); // for initialize counter

        const int STEERING_WHEEL_SET_PORT = 0;
        const double STEERING_WHEEL_MIN_SET_VALUE_IN_VOLTS = 1.2; //1.0 can cause error but its teoretical min
        const double STEERING_WHEEL_MID_SET_VALUE_IN_VOLTS = 2.5; // środek
        const double STEERING_WHEEL_MAX_SET_VALUE_IN_VOLTS = 3.8; //4.0 is teoretical max

        const int BRAKE_STRENGTH_SET_PORT = 1;
        const double BRAKE_MIN_SET_VALUE_IN_VOLTS = 0.4; //from 0
        const double BRAKE_MAX_SET_VALUE_IN_VOLST = 4.6; //from 5

        const int BRAKE_ENABLE_PORT_NO = 0;
        const byte BRAKE_ENABLE_ON_PORT_LEVEL = 0; 
        const byte BRAKE_ENABLE_OFF_PORT_LEVEL = 1;

        const int BRAKE_STOP_PORT_NO = 2;
        const byte BRAKE_STOP_ON_PORT_LEVEL = 0;
        const byte BRAKE_STOP_OFF_PORT_LEVEL = 1;

        const int BRAKE_DIRECTION_PORT_NO = 1;
        const int BRAKE_BACKWARD_PORT_LEVEL = 1; 
        const int BRAKE_FORWARD_PORT_LEVEL = 0;

        public void Initialize()
        {
            string deviceDescription = "USB-4702,BID#0"; // '0' -> 1st extension card

            try
            {
                //Analog outputs
                instantAoCtrl.SelectedDevice = new DeviceInformation(deviceDescription); // AO0

                //Digital output
                instantDoCtrl.SelectedDevice = new DeviceInformation(deviceDescription);

                //Counter
                eventSpeedCounterCtrl.SelectedDevice = new DeviceInformation(deviceDescription);

                eventSpeedCounterCtrl.Channel = 0;
                eventSpeedCounterCtrl.Enabled = true; //IMPORTANT: was ----> // false; // block counter

                setPortDO(BRAKE_ENABLE_PORT_NO, BRAKE_ENABLE_ON_PORT_LEVEL); //enabling brake engine
                setPortDO(BRAKE_STOP_PORT_NO, BRAKE_STOP_OFF_PORT_LEVEL);
            }
            catch (Exception e)
            {
                Logger.Log(this, "cannot initialize connection for USB4702", 2);
                Logger.Log(this, String.Format("Exception received: {0}", e.Message), 2);

                //throw; //TODO: IMPORTANT: TEMPORARY!!
            }
        }
        
        /// <summary>
        /// sets "value" voltage on channel no "channel"
        /// </summary>
        /// <param name="channel">channel no</param>
        /// <param name="value">0-5V (will be checked anyway - throws if bad)</param>
        private void setPortAO(int channel, double value)
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

        /// <summary>
        /// sets 0/1 "level" on port "port"
        /// </summary>
        /// <param name="port">port no</param>
        /// <param name="level">0/1</param>
        private void setPortDO(int port, byte level) //TODO: shouldn't it be just bool???
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
                return -666;
            }
            return eventSpeedCounterCtrl.Value;
            
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

            Helpers.ReScaller.ReScale(ref strength, 0, 100, BRAKE_MIN_SET_VALUE_IN_VOLTS, BRAKE_MAX_SET_VALUE_IN_VOLST);

            setPortAO(BRAKE_STRENGTH_SET_PORT, strength);
            //setPortDO(BRAKE_DIRECTION_PORT_NO, BRAKE_FORWARD_PORT_LEVEL);
            //setPortAO(BRAKE_STRENGTH_SET_PORT, 2);
        }
    }

}
