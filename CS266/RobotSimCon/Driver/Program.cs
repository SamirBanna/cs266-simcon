using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CS266.SimCon.Controller.WorldInputInterfaces;
//using Robotics.SimulationTutorial1;

namespace CS266.SimCon.Controller
{
    public static class Program
    {

        static ControlLoop controlLoop;
        static WorldInputInterface wip;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {

            Driver.Driver.Run();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
