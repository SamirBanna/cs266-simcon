using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.RoboticsClasses;
using CS266.SimCon.Simulator;

namespace CS266.SimCon.Controller.WorldOutputInterfaces
{
    public abstract class WorldOutputInterface
    {
        public void DoActions(Queue<PhysicalRobotAction> actions);
    }
}
