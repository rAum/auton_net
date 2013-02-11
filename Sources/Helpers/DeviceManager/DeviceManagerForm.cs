using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Helpers
{
    public partial class DeviceManagerForm : Form
    {
        private DeviceManager deviceManager;

        public DeviceManagerForm(DeviceManager devManager)
        {
            deviceManager = devManager;

            InitializeComponent();


            dataGridView1.Columns.Add("device name", "device name");
            dataGridView1.Columns.Add("overall state", "overall state");
            dataGridView1.Columns.Add("initialization state", "initialization state");

            foreach(var item in deviceManager.devicesList)
            {
                string[] row = new string[] { item.ToString(), item.overallState.ToString(), item.initializationState.ToString() };

                
                //row.Cells["device name"].Value = item.ToString();
                //row.Cells["overall state"].Value = item.overallState.ToString();
                //row.Cells["initialization state"].Value = item.initializationState.ToString();

                dataGridView1.Rows.Add(row);
            }

        }

        private void button_initialize_Click(object sender, EventArgs e)
        {
            deviceManager.Initialize();
        }

        private void button_StartSensors_Click(object sender, EventArgs e)
        {
            deviceManager.StartEffectors();
        }

        private void button_StartEffectors_Click(object sender, EventArgs e)
        {
            deviceManager.StartEffectors();
        }

        private void button_PauseEffectors_Click(object sender, EventArgs e)
        {
            deviceManager.PauseEffectors();
        }

        private void button_EmergencyStop_Click(object sender, EventArgs e)
        {
            deviceManager.EmergencyStop();
        }


    }
}
