using System;
using System.Collections.Generic;


using System.Text;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public abstract class WorldInputInterface
    {

        protected List<Robot> RobotList;
        protected List<PhysObject> PhysObjList;
        protected List<Food> FoodList;
        public ControllerWorldState ws;


        public WorldInputInterface()
        {
            RobotList = new List<Robot>();
            PhysObjList = new List<PhysObject>();
            FoodList = new List<Food>();
            int x = 0;
            int y = 0;
            ws = new ControllerWorldState(RobotList, PhysObjList, FoodList,x,y);
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

        public double worldWidth;
        public double worldHeight;
    }
}
