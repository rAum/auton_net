using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EngineSimulator
{
    // http://carsmind.com/specification.php?make=TOYOTA&model=Yaris - technical spec
    /*
        FOR NOW LET'S TAKE THIS ONE:
        
        Technical specification TOYOTA Yaris 1.3 2008Make:	TOYOTA
        Model:	Yaris 1.3
        Year:	2008
        Car category:	Small / Economy Cars
        Car engine position:	Front
        Car engine:	1298 ccm (78,80 cubic inches)
        Car engine type:	in-line, 4-cyl
        Car valves per cylinder:	4
        Car max power:	87.00 PS (63,68 kW or 85,57 HP) at 6000 Rev. per min.
        Car max torque:	122.00 Nm (12,34 kgf-m or 89,55 ft.lbs) at 4200 Rev. per min.
        Car bore stroke:	72.0 x 79.7 mm (2,79 x 3.1 inches)
        Car compression:	10.0:1
        Car top speed:	175.0 km/h (108,16 mph)
        Car fuel:	Gasoline, unleaded 95
        Car transmission:	Manual, 5-speed
        Car power per weight:	0.0888 PS/kg
        0 100 km h 0 62 mph:	12.1 seconds
        Car drive:	Front
        Car seats:	5
        Car passenger space:	2662 litres (699,49 gallons)
        Car doors:	3
        Car front tire:	175/65-R14
        Car rear tire:	175/65-R14
        Car chassis:	Hatchback
        Car co2 emissions:	165.0 g/km
        Car turn circle:	10 m (383,87 inches)
        Car weight:	984 kg (2158,45 pounds)
        Car towing weight:	906 kg (1987,41 pounds)
        Car total length:	3650 mm (142,98 inches)
        Car total width:	1670 mm (65,37 inches)
        Car total height:	1510 mm (59,10 inches)
        Car max weight with load:	1368 kg (3000,82 pounds)
        Car wheelbase:	2380 mm (93,23 inches)
        Car cooling:	Liquid
        Car front brakes type:	Ventilated disks
        Car rear brakes type:	Disks
        Car cargo space:	205 litres (53,73 gallons)
        Car lubrication:	Wet sump
        Car aerodynamic dragcoefisient:	1
        Car fuel with mixed drive:	5.8 litres/100 km (40,36 miles per gallon)
        Car fuel tank capacity:	45.0 litres (11,83 gallons)
    */

    /* 
        TORQUE AND POWER DIAGRAM:
        
        http://rototest-research.eu/popup/performancegraphs.php?ChartsID=582
      
        also avitable in .pdf file in projects dir 
     */
    
    /* MY CALCULATIONS ABOUT CAR DYNAMICS (TORQUE BASED):
     * torque_current = E_engine * M_engine + //E = epsilon, M = momentum //engine ineria
     *          + E_wheels * M_wheels +  //wheels inertia
     *          + a_car * m_car / transmission_rate / r_wheel + //a = acceleration, m = mass, r = radius //car dynamics
     *          + internal_forces //???
     *          + external_forces / transmission_rate / r_wheel //external forces on wheel
     *          
     *  torque_current = torque[curr_rpm] * gas_in_peccents_current
     */
        

    public class EnginePointStats
    {
        public double RPM; // 1/s
        public double power; // W //engine output power (on wheel - when there are no transmission)
        public double torque; // N*m //ending output torque (on wheel - when there are no transmission)

        public EnginePointStats(double _RPM, double _power, double _torque)
        {
            RPM = _RPM;
            power = _power;
            torque = _torque;
        }
    }

    abstract class CarModel
    {
        public abstract List<EnginePointStats> engineStats { get; }
        public abstract double staticEngineAntiForces { get; } //N*m
        public abstract double dynamicEngineAntiForces { get; } //N*m/RPM
        public abstract double engineMomentum { get; } //kg * m^2
        public abstract double RPM { get; protected set; }
        public abstract double Torque { get; }
        public abstract double Power { get; }

        public abstract void Start();

        private static double LinearApprox(double x1, double y1, double x2, double y2, double wantedX)
        {
            double yDiff = y2 - y1;
            double xDiff = x2 - x1;
            double changePerPoint = yDiff / xDiff;
            double wantedXdiffFromX1 = wantedX - x1;

            return y1 + changePerPoint * wantedXdiffFromX1;
        }

        protected double GetTorque(double RPM)
        {
            int upperBound = engineStats.FindIndex(x => x.RPM > RPM);
            int lowerBound = engineStats.FindLastIndex(x => x.RPM < RPM);

            if (lowerBound != 1 && upperBound != -1) //it is in scale
            {
                var p1 = engineStats[lowerBound];
                var p2 = engineStats[upperBound];
                return LinearApprox(p1.RPM, p1.torque, p2.RPM, p2.torque, RPM);
            }
            else if (lowerBound == -1) //if its under a scale
            {
                var p1 = engineStats[upperBound];
                var p2 = engineStats[upperBound + 1];
                return LinearApprox(p1.RPM, p1.torque, p2.RPM, p2.torque, RPM);
            }
            else //if its over a scale
            {
                var p1 = engineStats[lowerBound];
                var p2 = engineStats[lowerBound - 1];
                return LinearApprox(p1.RPM, p1.torque, p2.RPM, p2.torque, RPM);
            }
        }
    }

    class ToyotaYaris : CarModel
    {
        private List<EnginePointStats> __ENGINE_STATS__ = new List<EnginePointStats>();
        public override List<EnginePointStats> engineStats
        {
            get { return __ENGINE_STATS__; }
        }

        public ToyotaYaris()
        {
            engineStats.Add(new EnginePointStats(1493 / 60, 13500, 86.1));
            engineStats.Add(new EnginePointStats(2010 / 60, 20200, 96.1));
            engineStats.Add(new EnginePointStats(2508 / 60, 27000, 102.8));
            engineStats.Add(new EnginePointStats(3010 / 60, 33200, 105.2));
            engineStats.Add(new EnginePointStats(3508 / 60, 41400, 112.6));
            engineStats.Add(new EnginePointStats(4017 / 60, 47500, 112.8));
            engineStats.Add(new EnginePointStats(4209 / 60, 50200, 113.8));
            engineStats.Add(new EnginePointStats(4416 / 60, 52800, 114.3));
            engineStats.Add(new EnginePointStats(4616 / 60, 54800, 113.6));
            engineStats.Add(new EnginePointStats(5011 / 60, 55800, 106.3));
            engineStats.Add(new EnginePointStats(5512 / 60, 57500, 99.6));
            engineStats.Add(new EnginePointStats(5811 / 60, 57300, 94.2));
            engineStats.Add(new EnginePointStats(6006 / 60, 56200, 89.4));
            engineStats.Add(new EnginePointStats(6203 / 60, 56100, 86.4));

            engineStats.OrderBy(x => x.RPM); //ENGINE STATS HAVE TO BE ORDERED BY RPM
        }

        public override double staticEngineAntiForces{get { return -50; }}
        public override double dynamicEngineAntiForces { get { return -1 * 0.2 * RPM; } }

        private double __RPM__;
        public override double RPM { get { return __RPM__; } protected set { __RPM__ = value; } }
        public override double Torque{get{ return this.GetTorque(RPM); }}
        public override double Power
        {
            get { throw new NotImplementedException(); }
        }

        public override void Start()
        {
            RPM = 1000;
        }
    }


    class EngineSimulator
    {
        CarModel model;

        const double SIMULATION_TIMER_INTERVAL_IN_MS = 10;
        Timer SimulationTimer = new Timer(SIMULATION_TIMER_INTERVAL_IN_MS);
        
        public EngineSimulator(CarModel _model)
        {
            model = _model;

            SimulationTimer.Elapsed += SimulationTimer_Elapsed;
            SimulationTimer.Start();
        }

        void SimulationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
