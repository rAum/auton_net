using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

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
        private SerialPort port;

        private Object readLock = new Object();
        private Object queryLock = new Object();
        private AutoResetEvent msgReceivedARE;
        private LinkedList<int> inputList;
        private List<int> lastReceivedMessage;


        /* COM connected constants */
        private const int END_OF_STREAM = -1;
        private const char END_OF_MESSAGE = (char)13;
        /* end of COM connected constants */
        
#if DEBUG
        private const int uC_QUERY_TIMEOUT_IN_MS = 1000;
#else
        private const int uC_QUERY_TIMEOUT_IN_MS = 50;
#endif

        public SafeRS232Communicator(
            string portName,
            int baudRate = 9600,
            Parity parity = Parity.None,
            int dataBits = 8,
            StopBits stopBits = StopBits.One
        )
        {
            port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            port.Open();

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
            if (port.IsOpen)
            {
                port.Close();
            }
        }

        public List<int> Query(char[] queryMsg)
        {
            lock(queryLock)
            {
                try
                {
                    lastReceivedMessage = null;

                    Write(queryMsg);
                    msgReceivedARE.WaitOne(uC_QUERY_TIMEOUT_IN_MS);

                    if (lastReceivedMessage == null)
                        throw new TimeoutException("RS232 communicator timeout - no message has been received in time");

                    //TODO: handle timeout exception
                    
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
            lock (readLock) //only 1 thread can be executing read in the same time
            {
                int msg;
                while((msg = port.ReadByte()) != END_OF_STREAM)
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
        }

    }
}
