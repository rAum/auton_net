using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using car_communicator;
using Helpers;

namespace autonomiczny_samochod.Model.Communicators
{
    class Speedometer : Device
    {
        public override string ToString()
        {
            return "Speedometer";
        }

        public event SpeedInfoReceivedEventHander evSpeedInfoReceived;

        private USB4702 extentionCardCommunicator;

        private const int SPEED_MEASURING_TIMER_INTERVAL_IN_MS = 10; //in ms
        private const int SPEED_TABLE_SIZE = 30;
        private const double WHEEL_CIRCUIT_IN_M = 0.548 * Math.PI; //informacja of Filipa Godlewskiego z grupy mechaniki - oby prawdziwa ;)
        private const int NO_OF_HAAL_METERS = 5;
        private const int TICKS_TO_RESTART = 10000;

        private System.Windows.Forms.Timer SpeedMeasuringTimer = new System.Windows.Forms.Timer();
        private int[] lastTicksMeasurements = new int[SPEED_TABLE_SIZE];
        private int tickTableIterator = 0;
        private int lastTicks = 0;

        public Speedometer(USB4702 extentionCard)
        {
            extentionCardCommunicator = extentionCard;

            for (int i = 0; i < lastTicksMeasurements.Length; i++) 
            {
                lastTicksMeasurements[i] = 0;
            }

            SpeedMeasuringTimer.Interval = SPEED_MEASURING_TIMER_INTERVAL_IN_MS;
            SpeedMeasuringTimer.Tick += new EventHandler(SpeedMeasuringTimer_Tick);
        }

        void SpeedMeasuringTimer_Tick(object sender, EventArgs e)
        {
            //read
            int ticks = extentionCardCommunicator.getSpeedCounterStatus();

            //calculations
            lastTicksMeasurements[tickTableIterator] = ticks - lastTicks;

            tickTableIterator = (tickTableIterator + 1) % SPEED_TABLE_SIZE;

            double speed = Convert.ToDouble(WHEEL_CIRCUIT_IN_M) / Convert.ToDouble(NO_OF_HAAL_METERS) * Convert.ToDouble(lastTicksMeasurements.Sum()) / (Convert.ToDouble(SPEED_TABLE_SIZE) * Convert.ToDouble(SPEED_MEASURING_TIMER_INTERVAL_IN_MS) / 1000.0);

            if (ticks > TICKS_TO_RESTART)
            {
                extentionCardCommunicator.RestartSpeedCounter();
                lastTicks = 0;
            }

            //sending event
            SpeedInfoReceivedEventHander SpeedEvent = evSpeedInfoReceived;
            if (SpeedEvent != null)
            {
                SpeedEvent(this, new SpeedInfoReceivedEventArgs(speed));
            }
            lastTicks = ticks;
        }

        protected override void Initialize()
        {
            //no init - its dummy device - wrapper for speed measuring
        }

        protected override void StartSensors()
        {
            SpeedMeasuringTimer.Start();
        }

        protected override void StartEffectors()
        {
            //no effectors in here
        }

        protected override void PauseEffectors()
        {
            //no effectors in here
        }

        protected override void EmergencyStop()
        {
            //no effectors in here
        }
    }
}
