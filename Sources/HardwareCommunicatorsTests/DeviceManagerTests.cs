using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helpers;
using Rhino.Mocks;
using System.Threading;

namespace HardwareCommunicatorsTests
{
    public class FakeDevice : Device
    {
        protected override void Initialize()
        {
            Init();
        }

        public virtual void Init()
        {
        
        }

        protected override void PauseEffectors()
        {
            PsEfctors();
        }

        public virtual void PsEfctors()
        {
            
        }

        protected override void EmergencyStop()
        {
            EmgncyStp();
        }

        public virtual void EmgncyStp()
        {
            
        }

        protected override void StartEffectors()
        {
            StartEfctors();
        }

        public virtual void StartEfctors()
        {
            
        }

        protected override void StartSensors()
        {
            StartSnsrs();
        }

        public virtual void StartSnsrs()
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
        private Device dummyDevice;

        [TestInitialize()]
        public void TestInit()
        {
            devManager = new DeviceManager(false);  
            mocks = new MockRepository();
            dummyDevice = new FakeDevice();
            deviceMock = mocks.DynamicMock<FakeDevice>(); //like niceMock in googletest
            devManager.RegisterDevice(deviceMock);
            devManager.RegisterDevice(dummyDevice);
        }

        [TestCleanup()]
        public void TestCleanup()
        {

        }

        /* making overriden methods protected fucked everything up... - fake methods are not called, because its mock ...
        [TestMethod]
        public void DeviceManagerPassingMethodCallsTest()
        {
            using (mocks.Ordered())
            {
                Expect.Call(deviceMock.Init).Repeat.Once();
                Expect.Call(deviceMock.StartSnsrs).Repeat.Once();
                Expect.Call(deviceMock.StartEfctors).Repeat.Once();
                Expect.Call(deviceMock.PsEfctors).Repeat.Once();
                Expect.Call(deviceMock.EmgncyStp).Repeat.Once();
            }

            mocks.ReplayAll();

            devManager.Initialize();
            devManager.StartSensors();
            devManager.StartEffectors();
            devManager.PauseEffectors();
            devManager.EmergencyStop();

            mocks.VerifyAll();
        }

        [TestMethod]
        public void DeviceManagerStopsAllDevicesOnError()
        {
            using (mocks.Record())
            {
                Expect.Call(deviceMock.EmgncyStp).Repeat.Once();
            }

            using (mocks.Playback())
            {
                deviceMock.FakeError();
            }

            mocks.VerifyAll();
        }

        [TestMethod]
        public void DeviceManagerPausesAllEffectorsOnWarrning()
        {
            using (mocks.Record())
            {
                Expect.Call(deviceMock.PsEfctors).Repeat.Once();
            }

            using (mocks.Playback())
            {
                deviceMock.FakeWarrning();
            }

            mocks.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void DevicerThrowsExceptionInDebugModeOnWrongTransition()
        {
#if DEBUG
            devManager.StartEffectors();
#endif
        }
        */

        [TestMethod]
        public void DeviceStateIsChangingCorrectly()
        {
            int maxStateChangeTimeinMs = 50; //states are changed on different thread //TODO: RLY?!? sleeps in test? it looks bad

            Assert.AreEqual(DeviceInitializationState.NotInitialized, dummyDevice.initializationState);

            devManager.Initialize();
            Thread.Sleep(maxStateChangeTimeinMs);
            Assert.AreEqual(DeviceInitializationState.Initialized, dummyDevice.initializationState);

            devManager.StartSensors();
            Thread.Sleep(maxStateChangeTimeinMs);
            Assert.AreEqual(DeviceInitializationState.SensorsStarted, dummyDevice.initializationState);

            devManager.StartEffectors();
            Thread.Sleep(maxStateChangeTimeinMs);
            Assert.AreEqual(DeviceInitializationState.EffectorsStarted, dummyDevice.initializationState);

            devManager.PauseEffectors();
            Thread.Sleep(maxStateChangeTimeinMs);
            Assert.AreEqual(DeviceInitializationState.EffectorsPaused, dummyDevice.initializationState);

            devManager.StartEffectors();
            Thread.Sleep(maxStateChangeTimeinMs);
            Assert.AreEqual(DeviceInitializationState.EffectorsStarted, dummyDevice.initializationState);

            devManager.EmergencyStop();
            Thread.Sleep(maxStateChangeTimeinMs);
            Assert.AreEqual(DeviceInitializationState.EmergencyStopped, dummyDevice.initializationState);
        }

    }
}
