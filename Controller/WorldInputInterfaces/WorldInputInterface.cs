using System;
using System.Collections.Generic;


using System.Text;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public abstract class WorldInputInterface
    {

        protected List<Robot> RobotList;
        protected List<PhysObject> PhysObjList;
        public ControllerWorldState ws;
        public double worldWidth = 20;
        public double worldHeight = 9;

        public WorldInputInterface()
        {
            RobotList = new List<Robot>();
            PhysObjList = new List<PhysObject>();
            ws = new ControllerWorldState(RobotList, PhysObjList,worldWidth, worldHeight );
        }

        public void SetRunLoopDelegate(RunLoopDelegate del)
        {
            runLoopDelegate = del;
        }
        public abstract List<Robot> GetRobots();
        public abstract List<PhysObject> GetPhysObjects();
        public RunLoopDelegate runLoopDelegate;
        public abstract ControllerWorldState getWorldState();
        public abstract void setupInitialState();
        public abstract ControllerWorldState getNewWorldState();
    }
}
