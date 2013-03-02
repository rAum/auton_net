using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Helpers;

namespace CarController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DefaultCarController Controller { get; private set; }

        private System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();
        private const int TIMER_INTERVAL_IN_MS = 10;

        private const int TARGET_BRAKE_SETTING_WHEN_MANUAL_BRAKING_ON = 100;
        private const int BRAKE_ACTIVATION_TIME_ON_SPACE_PRESSING_IN_MS = 500; //its much too much, but smaller values blinks at start
        private const int MAX_FORWARD_SPEED_WHEN_DRIVING_ON_GAMEPAD_IN_MPS = 7; 
        private const int MAX_BACKWARD_SPEED_WHEN_DRIVING_ON_GAMEPAD_IN_MPS = -5; //it should be < 0 !
        private const int MAX_WHEEL_ANGLE_CHANGE_PER_SEC_WHEN_DRIVING_ON_GAMEPAD = 10;
        private const int WHEEL_ANGLE_CHANGING_WITH_GAMEPAD_TIMER_INTERVAL_IN_MS = 50;
        private const double MIN_GAMEPAD_Y_TO_START_TURNING_WHEEL_IN_PERCENTS = 5.0;
        private const double SPEED_CHANGE_PER_UP_DOWN_ARR_CLICK = 2.5;
        private const double WHEEL_ANGLE_CHANGE_PER_LEFT_RIGHT_ARR_CLICK = 2;

        private GamePad gamePad;

        System.Timers.Timer brakingTimer;
        private bool GamePadBrakingButtonPressed = false;

        System.Windows.Forms.Timer wheelAngleChangingWithGamePadTimer;

        private double gamePadCurrentTurningPerTick = 0.0;

        
        /// <summary>
        /// constructor which is initializing CarController on itself
        /// </summary>
        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.AboveNormal;
            Controller = new DefaultCarController();
            
            InitializeComponent();

            ExternalEventsHandlingInit();
            
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            //initialize timer
            mTimer.Interval = TIMER_INTERVAL_IN_MS;
            mTimer.Tick += new EventHandler(mTimer_Tick);
            mTimer.Start();
        }

        public MainWindow(DefaultCarController _controller)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.AboveNormal;
            Controller = _controller;

            InitializeComponent();

            ExternalEventsHandlingInit();
            
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            //initialize timer
            mTimer.Interval = TIMER_INTERVAL_IN_MS;
            mTimer.Tick += new EventHandler(mTimer_Tick);
            mTimer.Start();
        }

        private void ExternalEventsHandlingInit()
        {
            brakingTimer = new System.Timers.Timer();
            brakingTimer.Elapsed += new System.Timers.ElapsedEventHandler(brakingTimer_Elapsed);

            Controller.Model.evTargetSpeedChanged += new TargetSpeedChangedEventHandler(Model_evTargetSpeedChanged);
            Controller.Model.CarComunicator.evSpeedInfoReceived += new SpeedInfoReceivedEventHander(CarComunicator_evSpeedInfoReceived);

            Controller.Model.evTargetSteeringWheelAngleChanged += new TargetSteeringWheelAngleChangedEventHandler(Model_evTargetSteeringWheelAngleChanged);
            Controller.Model.CarComunicator.evSteeringWheelAngleInfoReceived += new SteeringWheelAngleInfoReceivedEventHandler(CarComunicator_evSteeringWheelAngleInfoReceived);
            Controller.Model.CarComunicator.evBrakePositionReceived += new BrakePositionReceivedEventHandler(CarComunicator_evBrakePositionReceived);

            Controller.Model.SpeedRegulator.evNewSpeedSettingCalculated += new NewSpeedSettingCalculatedEventHandler(SpeedRegulator_evNewSpeedSettingCalculated); //this is also target for brake regulator
            Controller.Model.SteeringWheelAngleRegulator.evNewSteeringWheelSettingCalculated += new NewSteeringWheelSettingCalculatedEventHandler(SteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated);
            Controller.Model.BrakeRegulator.evNewBrakeSettingCalculated += new NewBrakeSettingCalculatedEventHandler(BrakeRegulator_evNewBrakeSettingCalculated);

            Controller.Model.evAlertBrake += new EventHandler(Model_evAlertBrake);

            wheelAngleChangingWithGamePadTimer = new System.Windows.Forms.Timer();
            wheelAngleChangingWithGamePadTimer.Interval = WHEEL_ANGLE_CHANGING_WITH_GAMEPAD_TIMER_INTERVAL_IN_MS;
            wheelAngleChangingWithGamePadTimer.Tick += new EventHandler(wheelAngleChangingWithGamePadTimer_Tick);
            //please dont start me (wheelAngleChangingWithGamePadTimer) in here //it really should not be started in here

            gamePad = new GamePad();
            gamePad.evNewGamePadXYInfoAcquired += new GamePad.newGamePadXYInfoAcquiredEventHangler(gamePad_evNevGamePadInfoAcquired);
            gamePad.evNewGamePadButtonInfoAcquired += new GamePad.newGamePadButtonInfoAcquiredEventHandler(gamePad_evNewGamePadButtonInfoAcquired);

        }

        void brakingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!GamePadBrakingButtonPressed)
            {
                Controller.EndTargetBrakeSteeringOverriding();
            }
        }

        void wheelAngleChangingWithGamePadTimer_Tick(object sender, EventArgs e)
        {
            Controller.ChangeTargetWheelAngle(gamePadCurrentTurningPerTick);
        }

        void gamePad_evNewGamePadButtonInfoAcquired(object sender, int buttonNo, bool pressed)
        {
            switch(buttonNo)
            {
                case 5:
                case 6:
                    if (pressed)
                    {
                        Controller.OverrideTargetBrakeSetting(TARGET_BRAKE_SETTING_WHEN_MANUAL_BRAKING_ON);
                        GamePadBrakingButtonPressed = true;
                    }
                    else
                    {
                        Controller.EndTargetBrakeSteeringOverriding();
                        GamePadBrakingButtonPressed = false;
                    }
                    break;
            }
        }

        void gamePad_evNevGamePadInfoAcquired(object sender, double x, double y)
        {
            Controller.SetTargetSpeed(ReScaller.ReScale(ref y, -100, 100, MAX_BACKWARD_SPEED_WHEN_DRIVING_ON_GAMEPAD_IN_MPS, MAX_FORWARD_SPEED_WHEN_DRIVING_ON_GAMEPAD_IN_MPS));

            //Controller.SetTargetWheelAngle(ReScaller.ReScale(ref x, -100, 100, -1 * MAX_WHEEL_ANGLE_CHANGE_PER_SEC_WHEN_DRIVING_ON_GAMEPAD, MAX_WHEEL_ANGLE_CHANGE_PER_SEC_WHEN_DRIVING_ON_GAMEPAD));
            if (Math.Abs(y) > MIN_GAMEPAD_Y_TO_START_TURNING_WHEEL_IN_PERCENTS)
            {
                gamePadCurrentTurningPerTick = y * MAX_WHEEL_ANGLE_CHANGE_PER_SEC_WHEN_DRIVING_ON_GAMEPAD * WHEEL_ANGLE_CHANGING_WITH_GAMEPAD_TIMER_INTERVAL_IN_MS / 1000;
                wheelAngleChangingWithGamePadTimer.Start();
            }
            else
            {
                wheelAngleChangingWithGamePadTimer.Stop();
            }            
        }

        void Model_evAlertBrake(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock>((textBlock)
                    => textBlock.Text = "ALERT BRAKE ACTIVATED!!!!"),
                    textBlock_alertBrakeActivated
            );
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(e);
        }

        private void HandleKeyDown(KeyEventArgs key)
        {
            switch (key.Key)
            {
                case Key.Up:
                case Key.W:
                    Controller.ChangeTargetSpeed(SPEED_CHANGE_PER_UP_DOWN_ARR_CLICK);
                    break;

                case Key.Down:
                case Key.S:
                    Controller.ChangeTargetSpeed(-SPEED_CHANGE_PER_UP_DOWN_ARR_CLICK);
                    break;

                case Key.Left:
                case Key.A:
                    Controller.ChangeTargetWheelAngle(-WHEEL_ANGLE_CHANGE_PER_LEFT_RIGHT_ARR_CLICK);
                    break;

                case Key.Right:
                case Key.D:
                    Controller.ChangeTargetWheelAngle(WHEEL_ANGLE_CHANGE_PER_LEFT_RIGHT_ARR_CLICK);
                    break;

                case Key.Escape:
                    Controller.AlertBrake();
                    break;

                case Key.Space:
                    Controller.OverrideTargetBrakeSetting(TARGET_BRAKE_SETTING_WHEN_MANUAL_BRAKING_ON);
                    brakingTimer.Interval = BRAKE_ACTIVATION_TIME_ON_SPACE_PRESSING_IN_MS;
                    brakingTimer.Start();
                    break;

            }
        }

        void mTimer_Tick(object sender, EventArgs e)
        {
            textBlock_time.Text = String.Format(@"{0:mm\:ss\:ff}", Time.GetTimeFromProgramBeginnig());
        }



        private class LabelData
        {
            public double newValue;
            public double setValue;
            public bool labelSetInvokeAwaiting;

            public LabelData()
            {
                newValue = Double.NegativeInfinity;
                setValue = Double.NegativeInfinity;
                labelSetInvokeAwaiting = false;
            }
        }

        /// <summary>
        /// this method updates label with new value,
        ///     you can use this method as much as you want (e.g. 10000 times per second) 
        ///     it wont lead to stackoverflow (standard invoke would lead to stack overflow (and did it!))
        ///     
        /// the reason this method has sense is events which are updating textblock values sometimes are 
        /// invoked very frequently (like few hundred times a second)
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="valueToSet"></param>
        /// <param name="labelData"></param>
        private void UpdateTextBlock(TextBlock textBlock, double valueToSet, LabelData labelData)
        {
            labelData.newValue = valueToSet;
            if (labelData.newValue != labelData.setValue)
            {
                if (!labelData.labelSetInvokeAwaiting)
                {
                    labelData.labelSetInvokeAwaiting = true;
                    this.Dispatcher.Invoke(
                        new Action<TextBlock, double>((textBox, val) =>
                        {
                            textBox.Text = String.Format("{0:0.###}", val);
                            labelData.setValue = val;
                            labelData.labelSetInvokeAwaiting = false;
                        }),
                            textBlock,
                            labelData.newValue
                    );
                }
            }
        }

        /* 
         * below actions are done in order to fix stack overflow error which was happening in here
         *      (when there was just invoke on event like commented bellow)
         *      
         * when there were a lot of invoked events on every event window thread saved an information (on stack)
         * that something has to be done (usually label changed), when machine had a lot of computing to do it
         * ended in stack overflow in here (in window...)
         */
        LabelData steeringWheelSettingLabelData = new LabelData();
        void SteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated(object sender, NewSteeringWheelSettingCalculateddEventArgs args)
        {
            UpdateTextBlock(textBlock_steeringAngle, args.getSteeringWheelAngleSetting(), steeringWheelSettingLabelData);
            
            //this.Dispatcher.Invoke(
            //    new Action<TextBlock, string>((textBox, val)
            //        => textBox.Text = val),
            //            textBlock_steeringAngle,
            //            String.Format("{0:0.###}", args.getSteeringWheelAngleSetting())
            //);
        }

        LabelData steeringSpeedLabelData = new LabelData();
        LabelData targetBrakeLabelData = new LabelData();
        void SpeedRegulator_evNewSpeedSettingCalculated(object sender, NewSpeedSettingCalculatedEventArgs args)
        {
            UpdateTextBlock(textBlock_steeringSpeed, args.getSpeedSetting(), steeringSpeedLabelData);

            //target for brake regulator
            double targetBrake = args.getSpeedSetting();
            if(targetBrake < 0)
            {
                targetBrake *= -1;
            }
            else
            {
                targetBrake = 0;
            }
            UpdateTextBlock(textBlock_targetBrake, targetBrake, targetBrakeLabelData);
        }

        LabelData currentAngleLabelData = new LabelData();
        void CarComunicator_evSteeringWheelAngleInfoReceived(object sender, SteeringWheelAngleInfoReceivedEventArgs args)
        {
            UpdateTextBlock(textBlock_currentAngle, args.GetAngle(), currentAngleLabelData);
        }

        LabelData targetAngleLabelData = new LabelData();
        void Model_evTargetSteeringWheelAngleChanged(object sender, TargetSteeringWheelAngleChangedEventArgs args)
        {
            UpdateTextBlock(textBlock_targetAngle, args.GetTargetWheelAngle(), targetBrakeLabelData);
        }

        LabelData currentSpeedLabelData = new LabelData();
        void CarComunicator_evSpeedInfoReceived(object sender, SpeedInfoReceivedEventArgs args)
        {
            UpdateTextBlock(textBlock_currentSpeed, args.GetSpeedInfo(), currentSpeedLabelData);
        }

        LabelData targetSpeedLabelData = new LabelData();
        void Model_evTargetSpeedChanged(object sender, TargetSpeedChangedEventArgs args)
        {
            UpdateTextBlock(textBlock_targetSpeed, args.GetTargetSpeed(), targetSpeedLabelData);
        }

        LabelData currentBrakeLabelData = new LabelData();
        void CarComunicator_evBrakePositionReceived(object sender, BrakePositionReceivedEventArgs args)
        {
            UpdateTextBlock(textBlock_currentBrake, args.GetPosition(), currentBrakeLabelData);
        }

        LabelData steeringBrakeLabelData = new LabelData();
        void BrakeRegulator_evNewBrakeSettingCalculated(object sender, NewBrakeSettingCalculatedEventArgs args)
        {
            UpdateTextBlock(textBlock_steeringBrake, args.GetBrakeSetting(), steeringBrakeLabelData);
        }

        private void button_saveStats_Click(object sender, RoutedEventArgs e)
        {
            Controller.SaveStatsToFile("stats.txt");
            Logger.Log(this, "STATS HAS BEEN WRITTEN TO FILE!", 1);
        }
    }
}
