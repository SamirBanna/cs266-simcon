using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller.WorldOutputInterfaces
{
    public abstract class WorldOutputInterface
    {
        public abstract void DoActions(PhysicalRobotAction action);

    }
}
