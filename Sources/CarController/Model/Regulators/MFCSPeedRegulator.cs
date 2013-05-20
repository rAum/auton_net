using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarController
{
    public class MFCSPeedRegulator : ISpeedRegulator
    {
        public event NewSpeedSettingCalculatedEventHandler evNewSpeedSettingCalculated;

        public ICar Car { get; private set; }

        public double SpeedSteering { get; private set; }

        public CarSimulator.CarModel CarModel { get; private set; }

        public IDictionary<string, double> GetRegulatorParameters()
        {
            throw new NotImplementedException();
        }
    }
}
