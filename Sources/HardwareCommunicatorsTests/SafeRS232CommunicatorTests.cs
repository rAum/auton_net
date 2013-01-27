using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using autonomiczny_samochod.Model.Communicators;
using System.IO.Ports;
using System.Threading;

namespace HardwareCommunicatorsTests
{
    [TestClass()]
    public class SafeRS232CommunicatorTests
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /*
         * IMPORTANT NOTE:
         *  Bellow test methods are using com0com COM port emulator
         *  It creates 2 COM ports, when you write something to one
         *  of theese - second one can read it
         */

        /* COM connected constants */
        private const int END_OF_STREAM = -1;
        private const char END_OF_MESSAGE = (char)13;
        /* end of COM connected constants */

        static string com0comPort1 = "COM5";
        static string com0comPort2 = "COM6";

        static SafeRS232Communicator testedRS232;
        static SerialPort fakeRS232;

        [TestInitialize()]
        public void TestInit()
        {
            testedRS232 = new SafeRS232Communicator(com0comPort1);

            fakeRS232 = new SerialPort(com0comPort2, 9600, Parity.None, 8, StopBits.One);
            fakeRS232.Open();
        }

        [ClassCleanup()]
        public static void TestCleanUp()
        {
            testedRS232.Close();
            if (fakeRS232.IsOpen)
            {
                fakeRS232.Close();
            }
        }


        private void fakeRS232SendBackData(object sender, SerialDataReceivedEventArgs e)
        {
            int msg;
            while ((msg = fakeRS232.ReadByte()) != END_OF_STREAM)
            {
                fakeRS232.Write(new byte[] { (byte)msg }, 0, 1);
            }
        }

        [TestMethod]
        public void BasicCommunicationTest()
        {
            fakeRS232.DataReceived += new SerialDataReceivedEventHandler(fakeRS232SendBackData);
            var data = testedRS232.Query(new char[] { 'd', 'u', 'p', 'a', END_OF_MESSAGE });

            Assert.AreEqual('d', Convert.ToChar(data[0]));
            Assert.AreEqual('u', Convert.ToChar(data[1]));
            Assert.AreEqual('p', Convert.ToChar(data[2]));
            Assert.AreEqual('a', Convert.ToChar(data[3]));
        }

        
        private void QueryAndExpectRandomMsg()
        {
            Random rand = new Random();
            char randChar = (char)(rand.Next() % 10); //no more than 12 - 13 would cause error

            var data = testedRS232.Query(new char[] { randChar, END_OF_MESSAGE });

            Assert.AreEqual(randChar, Convert.ToChar(data[0]));
        }

        [TestMethod]
        public void MultiThreadRS232Communication()
        {
            const int NO_OF_THREADS = 10;

            fakeRS232.DataReceived += new SerialDataReceivedEventHandler(fakeRS232SendBackData);

            System.Threading.Thread[] threads = new System.Threading.Thread[NO_OF_THREADS];
            for (int i = 0; i < NO_OF_THREADS; i++)
            {
                threads[i] = new Thread(new ThreadStart(QueryAndExpectRandomMsg));
                threads[i].Start();
            }

            for (int i = 0; i < NO_OF_THREADS; i++)
            {
                threads[i].Join();
            }
        }

        private void fakeRS232ReadDataWith2sDelayAndResendIt(object sender, SerialDataReceivedEventArgs e)
        {
            int msg;
            while ((msg = fakeRS232.ReadByte()) != END_OF_STREAM)
            {
                Thread.Sleep(2000);
                fakeRS232.Write(new byte[] { (byte)msg }, 0, 1);
            }
        }

        [TestMethod]
        public void TimeoutOnRS232Write()
        {
            fakeRS232.DataReceived += new SerialDataReceivedEventHandler(fakeRS232ReadDataWith2sDelayAndResendIt);

            bool timeoutOccured = false;
            try
            {
                testedRS232.Query(new char[] { 'a', END_OF_MESSAGE });
            }
            catch (TimeoutException)
            {
                timeoutOccured = true;
            }

            Assert.IsTrue(timeoutOccured);
        }


        private void fakeRS232ResendDataWith2sDelay(object sender, SerialDataReceivedEventArgs e)
        {
            int msg;
            while ((msg = fakeRS232.ReadByte()) != END_OF_STREAM)
            {
                Thread.Sleep(2000);
                fakeRS232.Write(new byte[] { (byte)msg }, 0, 1);
            }
        }

        [TestMethod]
        public void TimeoutOnRS232Communication()
        {
            fakeRS232.DataReceived += new SerialDataReceivedEventHandler(fakeRS232ResendDataWith2sDelay);

            bool timeoutOccured = false;
            try
            {
                testedRS232.Query(new char[] { 'a', END_OF_MESSAGE });
            }
            catch(TimeoutException)
            {
                timeoutOccured = true;
            }

            Assert.IsTrue(timeoutOccured);
        }
    }
}
