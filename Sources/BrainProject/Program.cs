using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV.Structure;
using VisionFilters.Filters.Image_Operations;
using VisionFilters.Output;


namespace BrainProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ViewForm());
        }
    }
}
