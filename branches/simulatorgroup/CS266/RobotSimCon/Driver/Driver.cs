using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.RoboticsClasses;

namespace CS266.SimCon.Controller.Driver
{
    class Driver
    {
        public static void Run(ObjectState ws, bool vision)
        {
            if (vision)
            {
                VisionInputInterface wip = new VisionInputInterface();
            }
            else
            {
                SimulatorInputInterface wip = new SimulatorInputInterface(ws);
            }
            List<Robot> robots = wip.GetRobots();
            List<PhysObject> worldObjects = wip.GetPhysObjects();

            //ControlLoop cl = new ControlLoop(robots, worldObjects);
            //wip.SetRunLoopDelegate(cl.RunLoop);
        }
    }
}
