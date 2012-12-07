using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pololu.UsbWrapper;
using Pololu.Usc;
using Helpers;
using autonomiczny_samochod;

//Created by Mateusz Nowakowski
//Refactored and merged by Maciej (Spawek) Oziebly 
namespace car_communicator
{
    public class ServoDriver
    {
        public const int GEARBOX_CHANNEL = 0;
        public const int THROTTLE_CHANNEL = 1;

        public const int MAX_THROTTLE = 7000;
        public const int MIN_THROTTLE = 3900;

        public const int GEAR_P = 4000; //IMPORTANT: NOT WORKING
        public const int GEAR_R = 5246;
        public const int GEAR_N = 6125;
        public const int GEAR_D = 7355;

        Usc Driver = null;

        public void Initialize()
        {
            List<DeviceListItem> list = Usc.getConnectedDevices();

            if (list.Count == 1)
            {
                Driver = new Usc(list[0]);
            }
            else if (list.Count == 0)
            {
                Logger.Log(this, "there are no connected USC devices - servo driver can't start", 2);
            }
            else //more than 1 device
            {
                Logger.Log(this, "there are more than 1 USC devices - trying to connect last of them", 2);
                Driver = new Usc(list[list.Count - 1]); //last device
                //TODO: add device recognising
            }
        }

        private void setTarget(byte channel, ushort target)
        {
            if (Driver != null) //checking if ServoDriver is Initialized
            {
                if (channel == GEARBOX_CHANNEL)
                {
                    if (!(target == GEAR_P || target == GEAR_R || target == GEAR_N || target == GEAR_D))
                    {
                        throw new ApplicationException("wrong target");
                    }
                }
                else if (channel == THROTTLE_CHANNEL)
                {
                    if (target < MIN_THROTTLE || target > MAX_THROTTLE)
                    {
                        throw new ApplicationException("wrong target");
                    }
                }
                else
                {
                    throw new ApplicationException("unknown channel");
                }

                try
                {
                    Driver.setTarget(channel, target);
                }
                catch (Exception)
                {
                    Logger.Log(this, "couldnt send msg to servo!", 2);
                }
            }
            else
            {
                Logger.Log(this, "target was not set, because ServoDriver is not initialized", 1);
            }
        }

        public void setThrottle(double valueInPercents)
        {
            if(valueInPercents < 0 || valueInPercents > 100)
                throw new ApplicationException("wrong values - it should be in range 0 to 100%");

            Helpers.ReScaller.ReScale(ref valueInPercents, 0, 100, (double)MIN_THROTTLE, (double)MAX_THROTTLE);

            setTarget(THROTTLE_CHANNEL, (ushort)valueInPercents);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gear">
        /// p - parking
        /// r - reverse
        /// n - neutral
        /// d - tylko 1 bieg
        /// </param>
        public void setGear(Gear gear) //TODO: make some enum
        {
            switch(gear)
            {
                case Gear.parking:
                    setTarget(GEARBOX_CHANNEL, GEAR_P);
                    break;

                case Gear.reverse:
                    setTarget(GEARBOX_CHANNEL, GEAR_R);
                    break;

                case Gear.neutral:
                    setTarget(GEARBOX_CHANNEL, GEAR_N);
                    break;
                
                case Gear.drive:
                    setTarget(GEARBOX_CHANNEL, GEAR_D);
                    break;

                default:
                    Logger.Log(this, String.Format("trying to set not-existing gear", gear), 2);
                    break;
            }
        }

    }
}
