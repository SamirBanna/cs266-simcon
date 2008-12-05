using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class ControllerWorldState
    {
        public List<Robot> robots;
        public List<PhysObject> physobjects;

        public ControllerWorldState(List<Robot> robots, List<PhysObject> physobjects)
        {
            this.robots = robots;
            this.physobjects = physobjects;
        }

    }
}
