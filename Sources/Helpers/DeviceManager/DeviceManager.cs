using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Helpers
{
    public class DeviceManager
    {
        private static DeviceManager __GLOBAL_DEVICE_MANAGER__ = null;
        public static DeviceManager GlobalDeviceManager
        {
            get
            {
                if (__GLOBAL_DEVICE_MANAGER__ == null)
                {
                    __GLOBAL_DEVICE_MANAGER__ = new DeviceManager();
                }
                return __GLOBAL_DEVICE_MANAGER__;
            }
            private set { __GLOBAL_DEVICE_MANAGER__ = value; }
        }

        private List<Device> devicesList = new List<Device>();

        public event DeviceStateHasChangedEventHandler evDeviceStateHasChanged;

        /// <summary>
        /// changing this state will also invoke event 'evDeviceStateHasChanged'
        /// 
        /// default value is DeviceState.OK
        /// </summary>
        public DeviceState overallState
        {
            get { return __OVERALLSTATE__; }
            private set
            {
                if (__OVERALLSTATE__ != value)
                {
                    __OVERALLSTATE__ = value;
                    DeviceStateHasChangedEventHandler temp = evDeviceStateHasChanged;
                    if (temp != null)
                    {
                        temp(this, new DeviceStateHasChangedEventArgs(__OVERALLSTATE__));
                    }
                }
            }
        }
        private DeviceState __OVERALLSTATE__ = DeviceState.OK;

        public DeviceManager()
        {

        }

        public void RegisterDevice(Device dev)
        {
            devicesList.Add(dev);
            dev.evDeviceStateHasChanged += dev_evDeviceStateHasChanged;
            Logger.Log(this, String.Format("Device has been registred: {0}", dev.ToString()), 1);
        }

        private void dev_evDeviceStateHasChanged(object sender, DeviceStateHasChangedEventArgs args)
        {
            Logger.Log(this, 
                String.Format(
                    "device {0} state has changed to: {1}", 
                    sender.ToString(), 
                    args.ToString()), 
                1);

            switch (args.GetDeviceState())
            {
                case DeviceState.Error:
                    overallState = DeviceState.Error;
                    EmergencyStop();
                    break;

                case DeviceState.Warrning:
                    if (overallState == DeviceState.OK)
                    {
                        PauseEffectors();
                        overallState = DeviceState.Warrning;
                    }
                    break;

                case DeviceState.OK:
                    bool isEverythingOk = true;
                    foreach (Device dev in devicesList)
                    {
                        if (dev.state != DeviceState.OK)
                        {
                            isEverythingOk = false;
                        }
                    }
                    if (isEverythingOk)
                    {
                        overallState = DeviceState.OK;
                    }
                    break;

                default:
                    throw new ApplicationException("Unknown device state");
            }

        }

        public void Initialize()
        {
            Parallel.ForEach(devicesList, dev =>
            {
                Logger.Log(this, String.Format("Device initialization: {0}", dev.ToString()), 1);
                dev.InitializeWithPreAndPostWork();
            });
        }

        public void StartSensors()
        {
            Parallel.ForEach(devicesList, dev =>
            {
                Logger.Log(this, String.Format("Device sensors starting: {0}", dev.ToString()), 1);
                dev.StartSensorsWithPreAndPostWork();
            });
        }

        public void StartEffectors()
        {
            Parallel.ForEach(devicesList, dev =>
            {
                Logger.Log(this, String.Format("Device effectors starting: {0}", dev.ToString()), 1);
                dev.StartEffectorsWithPreAndPostWork();
            });
        }

        public void PauseEffectors()
        {
            Parallel.ForEach(devicesList, dev =>
            {
                Logger.Log(this, String.Format("Device effectors pausing: {0}", dev.ToString()), 2);
                dev.PauseEffectorsWithPreAndPostWork();
            });
        }

        public void EmergencyStop()
        {
            Parallel.ForEach(devicesList, dev =>
            {
                Logger.Log(this, String.Format("Device emergency stop: {0}", dev.ToString()), 2);
                dev.EmergencyStopWithPreAndPostWork();
            });
        }
    }
}
