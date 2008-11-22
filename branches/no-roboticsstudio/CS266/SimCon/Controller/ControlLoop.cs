using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller.PolygonIntersection;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;


namespace CS266.SimCon.Controller
{

    public delegate void RunLoopDelegate();

    class ControlLoop
    {
        Dictionary<int, Robot> Robots;
        Dictionary<int, PhysObject> WorldObjects;
        WorldInputInterface Wii;
        WorldOutputInterface Woi;
        ControllerWorldState worldState;


        public static Queue<PhysicalRobotAction> ActionQueue = new Queue<PhysicalRobotAction>();


        public ControlLoop(List<Robot> robots,  List<PhysObject> worldObjects)
        {
            foreach (Robot robot in robots)
            {
                Robots.Add(robot.Id,robot);
            }

            foreach (PhysObject obj in worldObjects)
            {
                WorldObjects.Add(obj.Id, obj);
            }
        }

        /// <summary>
        /// Runs the actual control loop
        /// </summary>
        public void RunLoop()
        {
            this.GetInput();
        }

        
        // Updates the states of robots and objects with new information
        //1. Get's the world state from the world input interface
        //2. update each robot's local view by updating that robots' sensors
        private void GetInput()
        {
            //List<PhysObject> worldObjects = Wii.GetPhysObjects();
            //List<Robot> robots = Wii.GetRobots();


            //// New positions of objects get updated immediately
            //foreach (PhysObject obj in worldObjects)
            //{
            //    this.WorldObjects[obj.Id].Location = obj.Location;
            //    this.WorldObjects[obj.Id].Orientation = obj.Orientation;
            //} 

            // New positions of objects get updated immediately
            // 



            //List<List<PhysObject>> allCollisions = CollisionDetector.Detect(robots, worldObjects);

            worldState = Wii.getWorldState();
            foreach (Robot r in Robots.Values)
            {
                foreach (SensorInput sensor in r.Sensors.Values)
                {
                    sensor.UpdateWorldState(worldState);
                }
            }
            
        }

    }
}
