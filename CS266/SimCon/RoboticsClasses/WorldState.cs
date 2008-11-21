using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.RoboticsClasses
{
    public class WorldState
    {
        public List<ObjectState> objects;
        public WorldState(List<ObjectState> objects)
        {
            this.objects = objects;
        }
    }
}
