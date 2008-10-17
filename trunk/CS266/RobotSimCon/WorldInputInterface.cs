using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    abstract class WorldInputInterface
    {
        public List<Robot> GetRobots();
        public List<PhysObject> GetPhysObjects();
    }
}
