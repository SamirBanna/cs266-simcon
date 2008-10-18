using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SimCon.WorldInputInterfaces;

namespace SimCon
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
            wip = new VisionInputInterface();
            List<Robot> robots = wip.GetRobots();
            List<PhysObject> worldObjects = wip.GetPhysObjects();

            controlLoop = new ControlLoop(robots, worldObjects);
            wip.SetRunLoopDelegate(controlLoop.RunLoop);



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
