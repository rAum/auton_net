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

namespace autonomiczny_samochod
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CarController Controller { get; private set; }

        private System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();
        private const int TIMER_INTERVAL_IN_MS = 10;

        public MainWindow()
        {
            Controller = new CarController(this);
            
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
            Controller.Model.evTargetSpeedChanged += new TargetSpeedChangedEventHandler(Model_evTargetSpeedChanged);
            Controller.Model.CarComunicator.evSpeedInfoReceived += new SpeedInfoReceivedEventHander(CarComunicator_evSpeedInfoReceived);

            Controller.Model.evTargetSteeringWheelAngleChanged += new TargetSteeringWheelAngleChangedEventHandler(Model_evTargetSteeringWheelAngleChanged);
            Controller.Model.CarComunicator.evSteeringWheelAngleInfoReceived += new SteeringWheelAngleInfoReceivedEventHandler(CarComunicator_evSteeringWheelAngleInfoReceived);
            Controller.Model.CarComunicator.evBrakePositionReceived += new BrakePositionReceivedEventHandler(CarComunicator_evBrakePositionReceived);

            Controller.Model.SpeedRegulator.evNewSpeedSettingCalculated += new NewSpeedSettingCalculatedEventHandler(SpeedRegulator_evNewSpeedSettingCalculated); //this is also target for brake regulator
            Controller.Model.SteeringWheelAngleRegulator.evNewSteeringWheelSettingCalculated += new NewSteeringWheelSettingCalculatedEventHandler(SteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated);
            Controller.Model.BrakeRegulator.evNewBrakeSettingCalculated += new NewBrakeSettingCalculatedEventHandler(BrakeRegulator_evNewBrakeSettingCalculated);

            Controller.Model.evAlertBrake += new EventHandler(Model_evAlertBrake);
        }

        void Model_evAlertBrake(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock>((textBlock)
                    => textBlock.Text = "ALERT BRAKE ACTIVATED!!!!"),
                    textBlock_alertBrakeActivated
            );
        }

        void CarComunicator_evBrakePositionReceived(object sender, BrakePositionReceivedEventArgs args)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock, string>((textBox, val)
                    => textBox.Text = val),
                        textBlock_currentBrake,
                        String.Format("{0:0.###}", args.GetPosition())
            );
        }

        void BrakeRegulator_evNewBrakeSettingCalculated(object sender, NewBrakeSettingCalculatedEventArgs args)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock, string>((textBox, val)
                    => textBox.Text = val),
                        textBlock_steeringBrake,
                        String.Format("{0:0.###}", args.GetBrakeSetting())
            );
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    Controller.ChangeTargetSpeed(1);
                    break;

                case Key.Down:
                case Key.S:
                    Controller.ChangeTargetSpeed(-1);
                    break;

                case Key.Left:
                case Key.A:
                    Controller.ChangeTargetWheelAngle(-5);
                    break;

                case Key.Right:
                case Key.D:
                    Controller.ChangeTargetWheelAngle(5);
                    break;

                case Key.Space:
                    Controller.AlertBrake();
                    break;
            }
        }

        void mTimer_Tick(object sender, EventArgs e)
        {
            textBlock_time.Text = String.Format(@"{0:mm\:ss\:ff}", Time.GetTimeFromProgramBeginnig());
        }

        void SteeringWheelAngleRegulator_evNewSteeringWheelSettingCalculated(object sender, NewSteeringWheelSettingCalculateddEventArgs args)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock, string>((textBox, val)
                    => textBox.Text = val),
                        textBlock_steeringAngle,
                        String.Format("{0:0.###}", args.getSteeringWheelAngleSetting())
            );
        }

        void SpeedRegulator_evNewSpeedSettingCalculated(object sender, NewSpeedSettingCalculatedEventArgs args)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock, string>((textBox, val)
                    => textBox.Text = val),
                        textBlock_steeringSpeed,
                        String.Format("{0:0.###}", args.getSpeedSetting())
            );

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
            this.Dispatcher.Invoke(
                new Action<TextBlock, string>((textBlock, val)
                    => textBlock.Text = val),
                        textBlock_targetBrake,
                        String.Format("{0:0.###}", targetBrake)
            );
        }

        void CarComunicator_evSteeringWheelAngleInfoReceived(object sender, SteeringWheelAngleInfoReceivedEventArgs args)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock, string>((textBox, val)
                    => textBox.Text = val),
                        textBlock_currentAngle,
                        String.Format("{0:0.###}", args.GetAngle())
            );
        }

        void Model_evTargetSteeringWheelAngleChanged(object sender, TargetSteeringWheelAngleChangedEventArgs args)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock, string>((textBox, val)
                    => textBox.Text = val),
                        textBlock_targetAngle,
                        String.Format("{0:0.###}", args.GetTargetWheelAngle())
            );
        }

        void CarComunicator_evSpeedInfoReceived(object sender, SpeedInfoReceivedEventArgs args)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock, string>((textBox, val) 
                    => textBox.Text = val), 
                        textBlock_currentSpeed, 
                        String.Format("{0:0.###}", args.GetSpeedInfo())
            );
        }

        void Model_evTargetSpeedChanged(object sender, TargetSpeedChangedEventArgs args)
        {
            this.Dispatcher.Invoke(
                new Action<TextBlock, string>((textBox, val)
                    => textBox.Text = val),
                        textBlock_targetSpeed,
                        String.Format("{0:0.###}", args.GetTargetSpeed())
            );
        }

        private void button_saveStats_Click(object sender, RoutedEventArgs e)
        {
            Controller.SaveStatsToFile("stats.txt");
            Logger.Log(this, "STATS HAS BEEN WRITTEN TO FILE!", 1);
        }
    }
}
