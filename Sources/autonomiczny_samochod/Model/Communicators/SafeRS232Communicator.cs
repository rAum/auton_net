using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using Helpers;

namespace autonomiczny_samochod.Model.Communicators
{
    /// <summary>
    /// THIS CLASS IS THREAD-SAFE
    /// 
    /// this class is created to communication with devices which needs query to send you any answer
    /// communication looks like this:
    /// THIS CLASS          RS232 DEVICE
    ///     |                   |
    ///     |                   |
    ///     |------query------->|
    ///     |                   |
    ///     |<----answer--------|
    ///     |                   |
    ///     |------query------->|
    ///     |                   |
    ///     |<----answer--------|
    ///     >
    /// no other messages are allowed and they will be threated as errors
    /// </summary>
    public class SafeRS232Communicator
    {
        public override string ToString()
        {
            return "RS232 communicator";
        }

        private SerialPort port;

        private Object readLock = new Object();
        private volatile bool readingActive = false;
        private Object queryLock = new Object();

        private AutoResetEvent msgReceivedARE;
        private LinkedList<int> inputList;
        private List<int> lastReceivedMessage;


        /* COM connected constants */
        private const int END_OF_STREAM = -1;
        private const char END_OF_MESSAGE = (char)13;
        /* end of COM connected constants */

        private const int SLEEP_ON_FAILED_PORT_OPPENING_BEFORE_NEXT_TRY_AT_APP_INIT_IN_MS = 10;
        private const int SLEEP_ON_FAILED_PORT_OPPENING_BEFORE_NEXT_TRY_AT_APP_WORKING_IN_MS = 0; //needed ASAP
#if DEBUG
        private const int uC_QUERY_TIMEOUT_IN_MS = 1000;
        private const int READ_TIMEOUT_IN_MS = 1000;
        private const int WRITE_TIMEOUT_IN_MS = 1000;
#else
        private const int uC_QUERY_TIMEOUT_IN_MS = 500;
        private const int READ_TIMEOUT_IN_MS = 500;
        private const int WRITE_TIMEOUT_IN_MS = 500;
#endif
        private const int DEFAULT_MAX_OPPENING_TRIES_NO = 60;



        public SafeRS232Communicator(
            string portName,
            int baudRate = 9600,
            Parity parity = Parity.None,
            int dataBits = 8,
            StopBits stopBits = StopBits.One
        )
        {
            port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            port.Encoding = Encoding.UTF8;
            port.ReadTimeout = READ_TIMEOUT_IN_MS;
            port.WriteTimeout = WRITE_TIMEOUT_IN_MS;
            TryOppeningPortUntilItSucceds(SLEEP_ON_FAILED_PORT_OPPENING_BEFORE_NEXT_TRY_AT_APP_INIT_IN_MS);

            inputList = new LinkedList<int>();
            msgReceivedARE = new AutoResetEvent(false);
            
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        ~SafeRS232Communicator()
        {
            if (port.IsOpen)
            {
                port.Close();
            }
        }

        public void Close()
        {
            lock (readLock)
            {
                lock (queryLock)
                {
                    if (port.IsOpen)
                    {
                        port.Close();
                    }
                }
            }
        }

        public List<int> Query(char[] queryMsg)
        {
            lock(queryLock)
            {
                try
                {
                    if (!port.IsOpen)
                    {
                        TryOppeningPortUntilItSucceds(SLEEP_ON_FAILED_PORT_OPPENING_BEFORE_NEXT_TRY_AT_APP_WORKING_IN_MS);
                    }

                    lastReceivedMessage = null;

                    Write(queryMsg);
                    msgReceivedARE.WaitOne(uC_QUERY_TIMEOUT_IN_MS);

                    if (lastReceivedMessage == null)
                        throw new TimeoutException("RS232 communicator timeout - no message has been received in time");

                    return lastReceivedMessage;
                }
                finally
                {
                    msgReceivedARE.Reset();
                }
            }
        }

        /// <summary></summary>
        /// <param name="queryMsg">END_OF_MESSAGE at the end is not neccesery</param>
        private void Write(char[] queryMsg)
        {
            port.Write(queryMsg, 0, queryMsg.Length);

            if (queryMsg[queryMsg.Length - 1] != END_OF_MESSAGE)
            {
                port.Write(new char[] { END_OF_MESSAGE }, 0, 1);
            } 
        }

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (readingActive) //so there won't be situaltion when there are 10000 threads waiting for lock
                return;

            lock (readLock) //only 1 thread can be executing read in the same time
            {
                try
                {
                    readingActive = true;

                    int msg;
                    while ((msg = port.ReadByte()) != END_OF_STREAM)
                    {
                        if (msg == END_OF_MESSAGE)
                        {
                            lastReceivedMessage = inputList.ToList();
                            inputList.Clear();
                            msgReceivedARE.Set();
                        }
                        else
                        {
                            inputList.AddLast(msg);
                        }
                    }
                }
                catch (TimeoutException)
                {
                    port.DiscardInBuffer();
                    inputList.Clear();
                    Logger.Log(this, "RS232 read timeout occured - in buffer discarded", 1);
                }
                catch (System.IO.IOException)
                {
                    port.DiscardInBuffer(); //not sure about it
                    inputList.Clear(); //not sure about it

                    Logger.Log(this, "RS232 IO exception occured", 2);
                }
                finally
                {
                    readingActive = false;
                }
            }
        }

        [Serializable]
        public class MaxTriesToConnectRS232ExceededException : Exception
        {
            public MaxTriesToConnectRS232ExceededException() { }
            public MaxTriesToConnectRS232ExceededException(string message) : base(message) { }
            public MaxTriesToConnectRS232ExceededException(string message, Exception inner) : base(message, inner) { }
            protected MaxTriesToConnectRS232ExceededException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

        private void TryOppeningPortUntilItSucceds(int waitBeforeNextTryInMs, int triesLeft = DEFAULT_MAX_OPPENING_TRIES_NO)
        {
            bool initializationFinished = false;
            while (!initializationFinished)
            {
                if (--triesLeft < 0)
                {
                    Logger.Log(this, "Max tries number to connect RS232 port has been exceeded!");
                    throw new MaxTriesToConnectRS232ExceededException();
                }

                try
                {
                    port.Open();
                    initializationFinished = true; //if program goes here and no exception has been thrown everything is ok
                }
                catch (Exception)
                {
                    Logger.Log(this, String.Format("RS232 port oppening failed, waiting {0}ms before next try, {1} tries left", waitBeforeNextTryInMs, triesLeft), 2);
                    Thread.Sleep(waitBeforeNextTryInMs);
                }
            }
        }

    }
}
