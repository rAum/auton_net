using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace Helpers
{
    public class GamePad
    {
        Timer timer;
        DirectInput directInput;
        Guid joystickGuid;
        Joystick joystick;

        const int TIMER_INTERVAL_IN_MS = 10;

        public delegate void newGamePadXYInfoAcquiredEventHangler(object sender, double x, double y);
        public event newGamePadXYInfoAcquiredEventHangler evNewGamePadXYInfoAcquired;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="buttonNo"></param>
        /// <param name="pressed">
        /// pressed = 1
        /// unpressed = 0;
        /// </param>
        public delegate void newGamePadButtonInfoAcquiredEventHandler(object sender, int buttonNo, bool pressed);
        public event newGamePadButtonInfoAcquiredEventHandler evNewGamePadButtonInfoAcquired;

        volatile bool GamePadBlocker = false;

        public GamePad()
        {
            // Initialize DirectInput
            directInput = new DirectInput();

            // Find a Joystick Guid
            joystickGuid = Guid.Empty;

            // Find a Gamepad
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad,
                        DeviceEnumerationFlags.AllDevices))
                joystickGuid = deviceInstance.InstanceGuid;

            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick,
                        DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Logger.Log(this, "No joystick/Gamepad found.", 2);            
            }
            else
            {
                // Instantiate the joystick
                joystick = new Joystick(directInput, joystickGuid);

                Logger.Log(this, String.Format("Found Joystick/Gamepad with GUID: {0}", joystickGuid), 1);

                // Set BufferSize in order to use buffered data.
                joystick.Properties.BufferSize = 128;

                // Acquire the joystick
                joystick.Acquire();

                // Poll events from joystick


                //while (true)
                //{
                //    joystick.Poll();
                //    var datas = joystick.GetBufferedData();
                //    foreach (var state in datas)
                //        Console.WriteLine(state);
                //}

                timer = new Timer();
                timer.Interval = TIMER_INTERVAL_IN_MS;
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
        }

        int lastX = 0;
        int lastY = 0;
        bool XorYAcuired = false;
        void timer_Tick(object sender, EventArgs e)
        {
            if (!GamePadBlocker)
            {
                try
                {
                    GamePadBlocker = true;
                    joystick.Poll();
                    var datas = joystick.GetBufferedData();

                    //double x = ReScaller.ReScale(ref datas[0].Value, 
                    foreach (var obj in datas)
                    {
                        switch (obj.Offset)
                        {
                            case JoystickOffset.Y:
                                lastY = obj.Value;
                                XorYAcuired = true;
                                break;

                            case JoystickOffset.RotationZ:
                                lastX = obj.Value;
                                XorYAcuired = true;
                                break;

                            case JoystickOffset.Buttons1:
                                newGamePadButtonInfoAcquiredEventHandler temp = evNewGamePadButtonInfoAcquired;
                                if (temp != null)
                                {
                                    if (obj.Value == 0)
                                    {
                                        temp(this, 1, false);
                                    }
                                    else
                                    {
                                        temp(this, 1, true);
                                    }
                                }
                                break;
                        }
                    }

                    if(XorYAcuired)
                    {
                        double tempX = lastX;
                        double tempY = lastY;

                        if(Math.Abs(tempX - 31487) < 100)
                        {
                            tempX = 65535 / 2 + 1;
                        }

                        if (Math.Abs(tempY - 31487) < 100)
                        {
                            tempY = 65535 / 2 + 1;
                        }

                        ReScaller.ReScale(ref tempX, 0, 65536, -100, 100);
                        ReScaller.ReScale(ref tempY, 65536, 0, -100, 100);

                        newGamePadXYInfoAcquiredEventHangler temp = evNewGamePadXYInfoAcquired;
                        if (temp != null)
                        {
                            temp(this, tempX, tempY);
                        }
                    }



                    GamePadBlocker = false;
                }
                finally
                {
                    GamePadBlocker = false;
                }
            }


        }
    }
}
