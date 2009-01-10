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
        public double worldWidth;
        public double worldHeight;

        public WorldInputInterface()
        {
            RobotList = new List<Robot>();
            PhysObjList = new List<PhysObject>();
            FoodList = new List<Food>();
            int x = 0;
            int y = 0;
            ws = new ControllerWorldState(RobotList, PhysObjList, FoodList, y, x);
        }


        public List<Robot> GetRobots()
        {
            return RobotList;
        }

        public List<PhysObject> GetPhysObjects()
        {
            return PhysObjList;
        }

        public List<Food> GetFood()
        {
            return FoodList;
        }

        public abstract ControllerWorldState getWorldState();
        public abstract void SetupInitialState();
    }
}
