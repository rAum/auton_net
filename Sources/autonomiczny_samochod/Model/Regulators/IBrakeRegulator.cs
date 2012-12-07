using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace autonomiczny_samochod
{
    public delegate void NewBrakeSettingCalculatedEventHandler(object sender, NewBrakeSettingCalculatedEventArgs args);
    public class NewBrakeSettingCalculatedEventArgs
    {
        private double brakeSetting;
        
        public NewBrakeSettingCalculatedEventArgs(double setting)
        {
            brakeSetting = setting;
        }

        public double GetBrakeSetting()
        {
            return brakeSetting;
        }
    }

    public interface IBrakeRegulator
    {
        event NewBrakeSettingCalculatedEventHandler evNewBrakeSettingCalculated;
    
        double BrakeSteering
        {
            get;
        }

        ICar ICar
        {
            get;
        }

        IDictionary<string, double> GetRegulatorParameters();
    }
}
