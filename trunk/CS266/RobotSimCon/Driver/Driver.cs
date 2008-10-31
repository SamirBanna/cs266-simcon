using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;

namespace CS266.SimCon.Controller.Driver
{
    class Driver
    {
        public static void Run()
        {
            VisionInputInterface wip = new VisionInputInterface();
            List<Robot> robots = wip.GetRobots();
            List<PhysObject> worldObjects = wip.GetPhysObjects();

            ControlLoop cl = new ControlLoop(robots, worldObjects);
            wip.SetRunLoopDelegate(cl.RunLoop);
        }
    }
}
