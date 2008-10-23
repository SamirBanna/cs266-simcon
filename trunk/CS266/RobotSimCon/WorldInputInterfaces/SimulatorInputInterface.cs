using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public class SimulatorInputInterface : WorldInputInterface
    {
        public override List<Robot> GetRobots()
        {
            throw new NotImplementedException();
        }

        public override List<PhysObject> GetPhysObjects()
        {
            throw new NotImplementedException();
        }
    }
}
