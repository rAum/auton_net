﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Helpers
{
    public partial class DeviceManagerForm : Form
    {
        private const int UPDATER_TIMER_INTERVAL_IN_MS = 100;

        private Jurek jurek;

        private DeviceManager deviceManager;
        private System.Windows.Forms.Timer updaterTimer;

        public DeviceManagerForm(DeviceManager devManager)
        {
            deviceManager = devManager;

            InitializeComponent();
            jurek = new Jurek();

            dataGridView1.Columns.Add("device name", "device name");
            dataGridView1.Columns.Add("overall state", "overall state");
            dataGridView1.Columns.Add("initialization state", "initialization state");

            updaterTimer = new System.Windows.Forms.Timer();
            updaterTimer.Interval = UPDATER_TIMER_INTERVAL_IN_MS;
            updaterTimer.Tick += updaterTimer_Tick;
            updaterTimer.Start();

            jurek.Say("Dzień dobry, mówi Twój najlepszy kierowca Jurek!");
            jurek.StartListening(hearedCommand);
        }

        void hearedCommand(string cmd, float confidence)
        {
            if (confidence >= 0.6)
            {
                if (cmd == "start")
                {
                    button_StartEffectors_Click(this, new EventArgs());
                }
                else if (cmd == "prepare")
                {
                    button_StartSensors_Click(this, new EventArgs());
                }
                else if (cmd == "stop")
                {
                    button_EmergencyStop_Click(this, new EventArgs());
                }
                else if (cmd == "yoorek")
                {
                    Random rnd = new Random();
                    int r = rnd.Next(0, 9);
                    switch (r)
                    {
                        case 0: jurek.Say("Słucham?"); break;
                        case 1: jurek.Say("no?"); break;
                        case 2: jurek.Say("Słucham Cię panie?"); break;
                        case 3: jurek.Say("Tak?"); break;
                        case 4: jurek.Say("Przed wyruszeniem w drogę należy zebrać drużynę!"); break;
                        case 5: jurek.Say("Ha ha ha"); break;
                        case 6: jurek.Say("Co robi grabaż?"); Thread.Sleep(2000); jurek.Say("Częstochowa. Ha ha ha he he"); break;
                        case 7: jurek.Say("Jakiś problem?"); break;
                        case 8: jurek.Say("Co robi blondynka pod drzewem?"); Thread.Sleep(2000); jurek.Say("Czeka na autograf Kory. Bu hehehe"); break;
                        default:  jurek.Say("Kurwa"); break;
                    }
                }
            }
        }

        void updaterTimer_Tick(object sender, EventArgs e)
        {
            UpdateForm();
        }

        delegate void UpdateFormDelegate();
        private void UpdateForm() //TODO: in some future make it event based (not timer based)
        {
            if (label1.InvokeRequired)
            {
                // this is worker thread
                UpdateFormDelegate del = new UpdateFormDelegate(UpdateForm);
                label1.Invoke(del, new object[] {});
            }
            else
            {
                // this is UI thread
                label_DeviceManagerAction.Text = deviceManager.currentActionName;

                try
                {
                    for (int i = 0; i < deviceManager.devicesList.Count; i++)
                    {
                        if (dataGridView1.Rows.Count < i + 1)
                        {
                            dataGridView1.Rows.Add();
                        }

                        //update device name
                        if ((string)dataGridView1.Rows[i].Cells[0].Value != deviceManager.devicesList[i].ToString())
                        {
                            dataGridView1.Rows[i].Cells[0].Value = deviceManager.devicesList[i].ToString();
                        }

                        //update overall device state
                        if ((string)dataGridView1.Rows[i].Cells[1].Value != deviceManager.devicesList[i].overallState.ToString())
                        {
                            dataGridView1.Rows[i].Cells[1].Value = deviceManager.devicesList[i].overallState.ToString();
                        }

                        //update initialization device state
                        if ((string)dataGridView1.Rows[i].Cells[2].Value != deviceManager.devicesList[i].initializationState.ToString())
                        {
                            dataGridView1.Rows[i].Cells[2].Value = deviceManager.devicesList[i].initializationState.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // no!   
                }
            } 
        }

        private void button_initialize_Click(object sender, EventArgs e)
        {
            jurek.Say("Rozpoczynam inicjalizację.");
            deviceManager.Initialize();
            jurek.Say("Inicjalizacja zakończona.");
        }

        private void button_StartSensors_Click(object sender, EventArgs e)
        {
            jurek.Say("Uruchamiam sensory.");
            deviceManager.StartSensors();
            jurek.Say("Sensory uruchomione.");
        }

        private void button_StartEffectors_Click(object sender, EventArgs e)
        {
            jurek.Say("Uruchamiam efektory.");
            deviceManager.StartEffectors();
            jurek.Say("Efektory uruchomione.");
        }

        private void button_PauseEffectors_Click(object sender, EventArgs e)
        {
            jurek.Say("Pauzuje efektory.");
            deviceManager.PauseEffectors();
        }

        private void button_EmergencyStop_Click(object sender, EventArgs e)
        {
            deviceManager.EmergencyStop();
            jurek.Say("Oż kurwa, zatrzymać! Przycisk bezpieczeństwa użyty.");
        }

    }
}
