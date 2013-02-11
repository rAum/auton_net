using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helpers;
using Rhino.Mocks;

namespace HardwareCommunicatorsTests
{
    public class FakeDevice : Device
    {
        protected override void Initialize()
        {

        }

        protected override void PauseEffectors()
        {

        }

        protected override void EmergencyStop()
        {

        }

        protected override void StartEffectors()
        {

        }

        protected override void StartSensors()
        {

        }

        public void FakeError()
        {
            this.overallState = DeviceOverallState.Error;
        }

        public void FakeWarrning()
        {
            this.overallState = DeviceOverallState.Warrning;
        }
    }

    [TestClass]
    public class DeviceManagerTests
    {
        private DeviceManager devManager;
        private MockRepository mocks;
        private FakeDevice deviceMock;

        [TestInitialize()]
        public void TestInit()
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
            //using (mocks.Ordered())
            //{
            //    //Expect.Call(deviceMock.Initialize).Repeat.Once();
            //    //Expect.Call(deviceMock.StartSensors).Repeat.Once();
            //    //Expect.Call(deviceMock.StartEffectors).Repeat.Once();
            //    //Expect.Call(deviceMock.PauseEffectors).Repeat.Once();
            //    //Expect.Call(deviceMock.EmergencyStop).Repeat.Once();
            //}

            //mocks.ReplayAll();

            //devManager.Initialize();
            //devManager.StartSensors();
            //devManager.StartEffectors();
            //devManager.PauseEffectors();
            //devManager.EmergencyStop();

            //mocks.VerifyAll();
            
        }

        [TestMethod]
        public void DeviceManagerStopsAllDevicesOnError()
        {
            //using (mocks.Record())
            //{
            //    Expect.Call(deviceMock.EmergencyStop).Repeat.Once();
            //}

            //using (mocks.Playback())
            //{
            //    deviceMock.FakeError();
            //}

            //mocks.VerifyAll();
        }

        [TestMethod]
        public void DeviceManagerPausesAllEffectorsOnWarrning()
        {
            //using (mocks.Record())
            //{
            //    Expect.Call(deviceMock.PauseEffectors).Repeat.Once();
            //}

            //using (mocks.Playback())
            //{
            //    deviceMock.FakeWarrning();
            //}

            //mocks.VerifyAll();
        }
    }
}
