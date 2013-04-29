using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EngineSimulator
{
    //http://www.asawicki.info/Mirror/Car%20Physics%20for%20Games/Car%20Physics%20for%20Games.html <- can be usefull

    //http://www.carfolio.com/specifications/models/car/?car=140591 <- more spec but for different model

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


    /*
     * GEARS:
     * http://en.wikipedia.org/wiki/Toyota_C_transmission
     * 
     * z wiki:
     * 1st	    2nd	     3rd	4th	     5th    Reverse	 Final
     * 3.545	1.913	1.310	1.027	0.850	3.214	 3.550
     * 
     * odwrotnosc (1/x) tego z wiki:
     * 1 -> 0,28208744710860366713681241184767
     * 2 -> 0,52273915316257187663355985363304
     * 3 -> 0,76335877862595419847328244274809
     * 4 -> 0,97370983446932814021421616358325
     * 5 -> 1,1764705882352941176470588235294
     * 
     * Final =  Differential Ratio  = 1 / 3.550 = 0,28169014084507042253521126760563
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
        public abstract double staticEngineResistanceForces { get;  } //N*m
        public abstract double dynamicEngineResistancePerRPM { get; } //N*m/RPM
        public abstract double externalResistanceForces { get; set; }
        public abstract double engineMomentum { get; } //kg * m^2
        public abstract double RPM { get; set; }
        public abstract double Torque { get; }
        public abstract double Power { get; }
        public abstract double WheelRadius { get; }
        public abstract double DifferentialRatio { get; }
        public abstract double ThrottleOppeningLevel { get; set; }
        public abstract double MaxEngineRPM { get; }
        public abstract double Mass{get;}

        public double Speed { get { return RPM / 60 * TransmissionRate * Wheel_circuit; } }
        public double ForwardForceOnWheelsFromEngine { get { return Torque / TransmissionRate / WheelRadius * ThrottleOppeningLevel; } }
        public double Wheel_circuit { get { return WheelRadius * 2 * Math.PI; } }
        public double TransmissionRate { get { return GearRatio(CurrGear) * DifferentialRatio; } }

        public double dynamicEngineResistanceForces { get { return dynamicEngineResistancePerRPM * RPM; } }
        public double engineResistanceForces { get { return dynamicEngineResistanceForces + staticEngineResistanceForces; } }
        public double engineResistanceForcesOnWheels { get { return engineResistanceForces / TransmissionRate / WheelRadius; } }

        public abstract int CurrGear { get; set; }
        public abstract int MaxGear { get; }
        public abstract double GearRatio(int gear);

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
            double torque;
            var engineStat = engineStats.Find(x => x.RPM == RPM);
            if (engineStat != null) //RPM is a point on our map
            {
                torque = engineStat.torque;
            }
            else
            {
                //RPM is not a point on our map (and it has top be approximated)
                if (RPM > engineStats.First().RPM && RPM < engineStats.Last().RPM) //it is in scale
                {
                    var p1 = engineStats.Find(x => x.RPM > RPM);
                    var p2 = engineStats.FindLast(x => x.RPM < RPM); //can be optimized
                    torque = LinearApprox(p1.RPM, p1.torque, 0, 0, RPM);
                }
                else if (RPM < engineStats.First().RPM) //if its under a scale
                {
                    var p1 = engineStats[0];
                    torque = LinearApprox(p1.RPM, p1.torque, 0, 0, RPM);
                }
                else //if its over a scale
                {
                    var p1 = engineStats.Last();
                    torque = LinearApprox(p1.RPM, p1.torque, MaxEngineRPM, 0, RPM);
                }
            }

            return torque;
        }

        public abstract double WheelMomentum { get; }
        public abstract double WheelMass { get; }
        public abstract double WheelsNo { get; }
    }

    class ToyotaYaris : CarModel
    {
        private List<EnginePointStats> __ENGINE_STATS__ = new List<EnginePointStats>()     
        {
            new EnginePointStats(1493, 13500, 86.1),
            new EnginePointStats(2010, 20200, 96.1),
            new EnginePointStats(2508, 27000, 102.8),
            new EnginePointStats(3010, 33200, 105.2),
            new EnginePointStats(3508, 41400, 112.6),
            new EnginePointStats(4017, 47500, 112.8),
            new EnginePointStats(4209, 50200, 113.8),
            new EnginePointStats(4416, 52800, 114.3),
            new EnginePointStats(4616, 54800, 113.6),
            new EnginePointStats(5011, 55800, 106.3),
            new EnginePointStats(5512, 57500, 99.6),
            new EnginePointStats(5811, 57300, 94.2),
            new EnginePointStats(6006, 56200, 89.4),
            new EnginePointStats(6203, 56100, 86.4)
        };
        public override List<EnginePointStats> engineStats { get { return __ENGINE_STATS__; } }
        private double[] gearTransmissionRatios = new double[]{
                0.2820874, // 1
                0.5227392, // 2
                0.7633588, // 3
                0.9737099, // 4
                1.1764705  // 5
        };
        public override double DifferentialRatio { get { return 1.0; } }// { get { return 1.0/3.550; } }
        
        public ToyotaYaris()
        {
            engineStats.OrderBy(x => x.RPM); //ENGINE STATS HAVE TO BE ORDERED BY RPM

            RPM = 0;
            externalResistanceForces = 0;
            ThrottleOppeningLevel = 0;
            CurrGear = 1;
        }

        public override int CurrGear { get; set; }
        public override int MaxGear { get { return 5; } }
        public override double GearRatio(int gear)
        {
            if (gear > MaxGear || gear < 1)
                throw new ArgumentException("gear is invalid");

            return gearTransmissionRatios[gear - 1];
        }

        public override double staticEngineResistanceForces{get { return 10.0; }}
        public override double dynamicEngineResistancePerRPM { get { return 0.009; } }
        public override double engineMomentum { get { return 1.0; } }

        public override double externalResistanceForces { get; set; }
        public override double RPM { get; set; }

        public override double Torque { get { return this.GetTorque(RPM); } }
        public override double Power { get { throw new NotImplementedException(); } }

        public override double ThrottleOppeningLevel { get; set; } //in [0,1] range

        public override void Start()
        {
            RPM = 1000;
        }

        // wheel: 175/65-R14
        public override double WheelRadius { get { return 14.0 * 2.54 / 2 / 100 + 0.65 * 0.175; } } // = 0,29155m //in meters

        public override double MaxEngineRPM { get { return 7000; } }

        public override double Mass { get { return 984.0; } }

        public override double WheelMomentum { get { return 1.0 / 2.0 * WheelMass * WheelRadius * WheelRadius; } } // 1/2 M * R^2 for cylinder //TODO: calculate it better

        public override double WheelsNo { get { return 4; } }

        public override double WheelMass{get{6.5 + 
        {
            get { throw new NotImplementedException(); }
        }
    }

    class EngineSimulator
    {
        public CarModel model;

        const double SIMULATION_TIMER_INTERVAL_IN_MS = 10;
        Timer SimulationTimer = new Timer(SIMULATION_TIMER_INTERVAL_IN_MS);
        
        public EngineSimulator(CarModel _model)
        {
            model = _model;

            SimulationTimer.Elapsed += SimulationTimer_Elapsed;
            SimulationTimer.Start();
        }

        /* MY CALCULATIONS ABOUT CAR DYNAMICS (TORQUE BASED):
         * torque_current = E_engine * M_engine + //E = epsilon, M = momentum //engine ineria
         *          + E_wheels * M_wheels +  //wheels inertia
         *          + a_car * m_car / transmission_rate / r_wheel + //a = acceleration, m = mass, r = radius //car dynamics //ERROR - it probably should be multiplied by transmission //TODO: look for mistake
         *          + internal_forces //???
         *          + external_forces / transmission_rate / r_wheel //external forces on wheel
         *
         * transmission_rate = omega_wheel / omega_engine 
         * so:
         * E_wheels = E_engine * transmission_rate  
         * 
         * 
         *  torque_current = torque[curr_rpm] * gas_in_peccents_current
         */

        void SimulationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //double E_engine = 0;

            //Console.WriteLine("E_engine change on: engine inertion: {0}", model.Torque / model.engineMomentum * model.ThrottleOppeningLevel);
            //Console.WriteLine("E_engine change on: engine resistance: {0}", model.engineResistanceForces * -1);
            //Console.WriteLine("E_engine change on: engine dynamic resistance:{0}", model.dynamicEngineResistanceForces * model.RPM * -1);
            //Console.WriteLine("E_engine change on: external resistance: {0}", model.externalResistanceForces * model.TransmissionRate / model.Wheel_circuit * -1);

            //E_engine += model.Torque / model.engineMomentum * model.ThrottleOppeningLevel; //E = epsilon //bezwladnosc silnika //NOTE: tutaj dodawać następne bezwładności
            //if(model.RPM > 0) E_engine -= model.engineResistanceForces; //opory tarcia silnika
            //if (model.RPM > 0) E_engine -= model.externalResistanceForces * model.TransmissionRate / model.Wheel_circuit; //sily zewnetrzne

            //model.RPM += E_engine * (SIMULATION_TIMER_INTERVAL_IN_MS / 1000) * 60;

            //if (model.RPM < 0)
            //{
            //    model.RPM = 0;
            //}

            //new way of calculations (basing on force on wheels)
            double ForceBallance = 0;

            ForceBallance += model.ForwardForceOnWheelsFromEngine;
            ForceBallance -= model.engineResistanceForcesOnWheels;
            ForceBallance -= model.externalResistanceForces;

            //double Epsilon_engine = 
            //    ForceBallance /
            //        (model.engineMomentum / model.TransmissionRate / model.WheelRadius +
            //        model.WheelsNo * model.WheelMomentum / model.WheelRadius +
            //        model.Mass / model.WheelRadius);

            //model.RPM += Epsilon_engine * (SIMULATION_TIMER_INTERVAL_IN_MS / 1000) * 60;

            //if (model.RPM < 0)
            //{
            //    model.RPM = 0;
            //}

            double Acceleration = //a = F/m (but we got some additional radial inetrions, so we have to remember about E = M / I)
                ForceBallance /
                    (model.Mass +
                    model.engineMomentum / model.TransmissionRate / model.WheelRadius + // engine inertion
                    model.WheelsNo * model.WheelMomentum / model.WheelRadius); // wheels inertion 
        }
    }
}
