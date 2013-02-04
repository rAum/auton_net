using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace autonomiczny_samochod.Model.Communicators
{
    public delegate void DeviceStateHasChangedEventHandler(object sender, DeviceStateHasChangedEventArgs args);
    public class DeviceStateHasChangedEventArgs : EventArgs
    {
        private DeviceState deviceState;

        public DeviceStateHasChangedEventArgs(DeviceState state)
        {
            deviceState = state;
        }

        public DeviceState GetDeviceState()
        {
            return deviceState;
        }
    }

    public enum DeviceState
    {
        OK, Warrning, Error
    }

    public abstract class Device
    {
        public event DeviceStateHasChangedEventHandler evDeviceStateHasChanged;

        /// <summary>
        /// changing this state will also invoke event 'evDeviceStateHasChanged'
        /// (and do some stuff in DeviceManager which will be subscribed for this event)
        /// 
        /// default value is DeviceState.OK
        /// </summary>
        public DeviceState state
        {
            get { return __STATE__; }
            protected set
            {
                if (__STATE__ != value)
                {
                    __STATE__ = value;
                    DeviceStateHasChangedEventHandler temp = evDeviceStateHasChanged;
                    if (temp != null)
                    {
                        temp(this, new DeviceStateHasChangedEventArgs(__STATE__));
                    }
                }
            }
        }
        private DeviceState __STATE__ = DeviceState.OK;

        public abstract void Initialize();

        public abstract void StartSensors();

        public abstract void StartEffectors();

        public abstract void PauseEffectors();

        public abstract void EmergencyStop();
    }
}
