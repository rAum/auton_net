using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace autonomiczny_samochod
{
    public class FakeBrakeRegulator : IBrakeRegulator
    {
        public event NewBrakeSettingCalculatedEventHandler evNewBrakeSettingCalculated;

        public double BrakeSteering { get; private set; }

        public ICar ICar { get; private set; }

        public IDictionary<string, double> GetRegulatorParameters()
        {
            return new Dictionary<string, double>();
        }
    }
}
