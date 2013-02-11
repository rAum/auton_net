using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
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


        public void InitializeWithPreAndPostWork()
        {
            InitializePreWork();
            Initialize();
            InitializePostWork();
        }
        protected abstract void Initialize();
        private void InitializePreWork()
        {
            Logger.Log(this, "Initialization started", 1);
        }
        private void InitializePostWork()
        {
            Logger.Log(this, "Initialization completed", 1);
        }

        public void StartSensorsWithPreAndPostWork()
        {
            StartSensorsPreWork();
            StartSensors();
            StartSensorsPostWork();
        }
        protected abstract void StartSensors();
        private void StartSensorsPreWork()
        {
            Logger.Log(this, "Starting sensors started", 1);
        }
        private void StartSensorsPostWork()
        {
            Logger.Log(this, "Starting sensors completed", 1);
        }

        public void StartEffectorsWithPreAndPostWork()
        {
            StartEffectorsPreWork();
            StartEffectors();
            StartEffectorsPostWork();
        }
        protected abstract void StartEffectors();
        private void StartEffectorsPreWork()
        {
            Logger.Log(this, "Starting effectors started", 1);
        }
        private void StartEffectorsPostWork()
        {
            Logger.Log(this, "Starting effectors completed", 1);
        }

        public void PauseEffectorsWithPreAndPostWork()
        {
            PauseEffectorsPreWork();
            PauseEffectors();
            PauseEffectorsPostWork();
        }
        protected abstract void PauseEffectors();
        private void PauseEffectorsPreWork()
        {
            Logger.Log(this, "Pausing effectors started", 1);
        }
        private void PauseEffectorsPostWork()
        {
            Logger.Log(this, "Pausing effectors completed", 1);
        }

        public void EmergencyStopWithPreAndPostWork()
        {
            EmergencyStopPreWork();
            EmergencyStop();
            EmergencyStopPostWork();
        }
        protected abstract void EmergencyStop();
        private void EmergencyStopPreWork()
        {
            Logger.Log(this, "Emergency stopping started", 1);
        }
        private void EmergencyStopPostWork()
        {
            Logger.Log(this, "Emergency stopping completed", 1);
        }
    }
}
