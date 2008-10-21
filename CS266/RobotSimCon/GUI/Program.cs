using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CS266.SimCon.Controller.WorldInputInterfaces;

namespace CS266.SimCon.Controller
{
    static class Program
    {

        static ControlLoop controlLoop;
        static WorldInputInterface wip;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //wip = new VisionInputInterface();
            //List<Robot> robots = wip.GetRobots();
            //List<PhysObject> worldObjects = wip.GetPhysObjects();

            //controlLoop = new ControlLoop(robots, worldObjects);
            //wip.SetRunLoopDelegate(controlLoop.RunLoop);



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
