using System;
using System.Collections.Generic;


using System.Text;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public abstract class WorldInputInterface
    {

        protected List<Robot> RobotList;
        protected List<PhysObject> PhysObjList;
        protected List<PhysObject> FoodList;
        public ControllerWorldState ws;
        public double worldWidth;
        public double worldHeight;

        public WorldInputInterface()
        {
            RobotList = new List<Robot>();
            PhysObjList = new List<PhysObject>();
            FoodList = new List<Food>();
            ws = new ControllerWorldState(RobotList, PhysObjList, worldWidth, worldHeight);
        }

        public void SetRunLoopDelegate(RunLoopDelegate del)
        {
            runLoopDelegate = del;
        }
        public abstract List<Robot> GetRobots();
        public abstract List<PhysObject> GetPhysObjects();
        public abstract List<Food> GetFood();
        public RunLoopDelegate runLoopDelegate;
        public abstract ControllerWorldState getWorldState();
        public abstract void setupInitialState();
        public abstract ControllerWorldState getNewWorldState();
    }
}
