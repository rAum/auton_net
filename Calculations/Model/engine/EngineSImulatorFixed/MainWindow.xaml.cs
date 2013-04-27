using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace EngineSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double FORM_UPDATE_INTERVAL_IN_MS = 25.0d;

        EngineSimulator sim = new EngineSimulator(new ToyotaYaris());
        Timer formUpdater = new Timer(FORM_UPDATE_INTERVAL_IN_MS);

        public MainWindow()
        {
            InitializeComponent();

            formUpdater.Elapsed += formUpdater_Elapsed;
            formUpdater.Start();
        }

        void formUpdater_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action<double>(x => this.slider_RPM.Value = x), sim.model.RPM); //update RMP slider
            this.Dispatcher.Invoke(new Action<double>(x => this.TextBlock_speed.Text = x.ToString() + " km/h"), sim.model.Speed);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            sim.model.Start();
        }

        private void slider_transmission_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newGear = Convert.ToInt32(e.NewValue);

            sim.model.CurrGear = newGear;
            if (this.TextBlock_currGear != null) // fix for initialization order issues
            {
                this.TextBlock_currGear.Text = newGear.ToString();
            }
        }

        private void slider_externalForces_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sim.model.externalAntiForces = e.NewValue;

            if (this.TextBlock_externalForces != null)
            {
                this.TextBlock_externalForces.Text = String.Format("{0}", e.NewValue.ToString("0.0"));
            }
        }

        private void slider_acceleration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Console.WriteLine("acceleration changed to: {0}", e.NewValue / 100);
            sim.model.ThrottleOppeningLevel = e.NewValue / 100;

            if (this.TextBlock_acceleration != null)
            {
                this.TextBlock_acceleration.Text = String.Format("{0}", e.NewValue.ToString("0.0"));
            }
        }
    }
}
