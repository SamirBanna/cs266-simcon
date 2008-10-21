using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS266.SimCon.Controller.Driver
{
    class Driver
    {
        public static void Run()
        {
            wip = new VisionInputInterface();
            List<Robot> robots = wip.GetRobots();
            List<PhysObject> worldObjects = wip.GetPhysObjects();

            controlLoop = new ControlLoop(robots, worldObjects);
            wip.SetRunLoopDelegate(controlLoop.RunLoop);
        }
    }
}
