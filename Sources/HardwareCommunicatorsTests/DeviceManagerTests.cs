using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using autonomiczny_samochod.Model.Communicators;
using Rhino.Mocks;

namespace HardwareCommunicatorsTests
{
    public class FakeDevice : Device
    {
        public override void Initialize()
        {

        }

        public override void PauseEffectors()
        {

        }

        public override void EmergencyStop()
        {

        }

        public override void StartEffectors()
        {

        }

        public override void StartSensors()
        {

        }

        public void FakeError()
        {
            this.state = DeviceState.Error;
        }

        public void FakeWarrning()
        {
            this.state = DeviceState.Warrning;
        }
    }

    [TestClass]
    public class DeviceManagerTests
    {
        private static DeviceManager devManager;
        private static MockRepository mocks;
        private static FakeDevice deviceMock;

        [ClassInitialize()]
        public static void ClassInit(TestContext param)
        {
            devManager = new DeviceManager();  
            mocks = new MockRepository();
            deviceMock = mocks.StrictMock<FakeDevice>();
            devManager.RegisterDevice(deviceMock);
        }

        [TestCleanup()]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void DeviceManagerPassingMethodCallsTest()
        {
            using (mocks.Ordered())
            {
                Expect.Call(deviceMock.Initialize);
                Expect.Call(deviceMock.StartSensors);
                Expect.Call(deviceMock.StartEffectors);
                Expect.Call(deviceMock.PauseEffectors);
                Expect.Call(deviceMock.EmergencyStop);

                devManager.Initialize();
                devManager.StartSensors();
                devManager.StartEffectors();
                devManager.PauseEffectors();
                devManager.EmergencyStop();
            }
        }

        [TestMethod]
        public void DeviceManagerStopsAllDevicesOnError()
        {
            using (mocks.Record())
            {
                Expect.Call(deviceMock.EmergencyStop);
            }

            using (mocks.Playback())
            {
                deviceMock.FakeError();
            }
        }

        [TestMethod]
        public void DeviceManagerPausesAllEffectorsOnWarrning()
        {
            using (mocks.Record())
            {
                Expect.Call(deviceMock.PauseEffectors);
            }

            using (mocks.Playback())
            {
                deviceMock.FakeWarrning();
            }
        }
    }
}
