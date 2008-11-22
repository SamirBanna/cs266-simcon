using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public abstract class WorldInputInterface
    {
        private ControllerWorldState worldState;

        public WorldInputInterface()
        {
        }

        public void SetRunLoopDelegate(RunLoopDelegate del)
        {
            runLoopDelegate = del;
        }
        public abstract List<Robot> GetRobots();
        public abstract List<PhysObject> GetPhysObjects();
        public RunLoopDelegate runLoopDelegate;
        public ControllerWorldState getWorldState(){
            return worldState;
        }
    }
}
