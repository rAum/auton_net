using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarController.Model.Communicators;
using CarController;
using Rhino.Mocks;
using System.Collections.Generic;

namespace HardwareCommunicatorsTests
{
    [TestClass]
    public class SafeRS232ControllerTests
    {
        private static MockRepository mocksRepo;
        private static RealCarCommunicator carCommunicatorMock;
        private static SafeRS232Communicator RS232Mock;
        private static SafeRS232Controller controller;

        //standard messages
        private char[] giveMeSteeringWheelAngleMsg = new char[] { '1', 'P', (char)13 }; //TODO: try changing it to byte[] //not necessery, but char[] probably wont work for values > 127...
        private char[] giveMeBrakeAngleMsg = new char[] { '2', 'P', (char)13 };
        private char[] giveMeSteeringWheelDiagnosisMsg = new char[] { '1', 'D', (char)13 };
        private char[] giveMeBrakeDiagnosisMsg = new char[] { '2', 'D', (char)13 };

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            mocksRepo = new Rhino.Mocks.MockRepository();

            carCommunicatorMock = mocksRepo.StrictMock<RealCarCommunicator>(new Object()); //IT IS ERROR //TODO: finish this test //SafeRS232Communicator should not rely on RealCarCommunicator!!! (byt on interface)
            RS232Mock = mocksRepo.StrictMock<SafeRS232Communicator>("JUST A FAKE PORT STRING");

            controller = new SafeRS232Controller(carCommunicatorMock, RS232Mock);
            controller.InitializeWithPreAndPostWork();
        }

        [TestMethod]
        public void BrakesReadTest()
        {
            Expect.Call(RS232Mock.Query(giveMeSteeringWheelAngleMsg)).Return(new List<int>(new int[] { 'A', '3', '5' }));
            Expect.Call(delegate { carCommunicatorMock.WheelAngleAcquired(2.0); }).Repeat.Once(); ;

            controller.StartSensorsWithPreAndPostWork();
        }
    }
}
