using System;
using System.Collections.Generic;


using System.Text;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public abstract class WorldInputInterface
    {
        /// <summary>
        /// Holds a list of Robot objects to be updated. This overlaps with the physical object list.
        /// </summary>
        protected List<Robot> RobotList;
        /// <summary>
        /// Holds a list of all physical objects in the world except food. (This design decision might be reconsidered.)
        /// </summary>
        protected List<PhysObject> PhysObjList;
        /// <summary>
        /// Holds a list of all food objects.
        /// </summary>
        protected List<Food> FoodList;
        /// <summary>
        /// 
        /// </summary>
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

        public abstract ControllerWorldState GetWorldState();
        public abstract void SetupInitialState();
    }
}
