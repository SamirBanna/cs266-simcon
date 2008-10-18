using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    abstract class WorldInputInterface
    {
        public abstract List<Robot> GetRobots();
        public abstract List<PhysObject> GetPhysObjects();
    }
}
