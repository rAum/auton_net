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
            this.Dispatcher.Invoke(new Action<double>(x => this.slider_RPM.Value = x)); //update RMP slider
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            sim.model.Start();
        }

        private void slider_transmission_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void slider_externalForces_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sim.model.externalAntiForces = e.NewValue;
        }

        private void slider_acceleration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
