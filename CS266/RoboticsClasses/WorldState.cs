using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS266.RoboticsClasses
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
