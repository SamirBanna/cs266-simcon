using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller.PolygonIntersection;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using CS266.SimCon.Controller.Exceptions;

namespace CS266.SimCon.Controller
{

    public delegate void RunLoopDelegate();

    class ControlLoop
    {
        Dictionary<int, Robot> Robots = new Dictionary<int,Robot>();
        Dictionary<int, PhysObject> WorldObjects = new Dictionary<int,PhysObject>();
        WorldInputInterface Wii;
        WorldOutputInterface Woi;
        ControllerWorldState worldState;

        // Robot grid. This should be instantiated in Experiments with a reference to worldState, and dimensions
        public static Grid robotGrid;

        public static Queue<PhysicalRobotAction> ActionQueue = new Queue<PhysicalRobotAction>();

        //Constructor
        //Initialize the robots
        //Initialize the world state
        public ControlLoop(WorldInputInterface Wii, WorldOutputInterface Woi)
        {
            this.Wii = Wii;
            this.Woi = Woi;

            worldState = Wii.ws;

            List<Robot> robots = worldState.robots;
            List<PhysObject> worldObjects = worldState.physobjects;
            

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
            Console.WriteLine("Number of robots in the ControlLoop: " + Robots.Count);
            this.GetInput();
            //Console.WriteLine("got input");

            this.RunAlgorithms();
            
            this.RunActionQueue(); ;
        }


        private void RunActionQueue()
        {
            foreach (PhysicalRobotAction action in ActionQueue)
            {
                Woi.DoActions(action);
            }
            ActionQueue.Clear();
        }
    
        // Updates the states of robots and objects with new information
        //1. Get's the world state from the world input interface
        //2. update each robot's local view by updating that robots' sensors
        private void GetInput()
        {
            worldState = ((VisionInputInterface)Wii).getNewWorldState();
          //  Console.WriteLine("Control Loop WS Angle before sensor: "+ worldState.robots[0].Orientation);
           // Console.WriteLine("controloop con");

            Console.WriteLine("*****Number of objects in the world: " +worldState.physobjects.Count + "*******");
            //update robot orientation and location
            foreach (Robot r in Robots.Values)
            {
                foreach (Robot z in worldState.robots)
                {
                    if (r.Id == z.Id)
                    {
                        r.Orientation = z.Orientation;
                        r.Location = z.Location;
                    }
                }
            }

            foreach (Robot r in Robots.Values)
            {
                foreach (SensorInput sensor in r.Sensors.Values)
                {
                    
                    sensor.UpdateWorldState(worldState);
                  //  Console.WriteLine("SENSOR AFTER WORLD STATE UPDATE: " + sensor.worldState.robots[0].Orientation);
                    sensor.UpdateSensor();
                }
            }
        }

        //Iterate through list of robots, and all their algorithm's execute function
        private void RunAlgorithms()
        {

            foreach (Robot r in Robots.Values)
            {
                try
                {
                    r.CurrentAlgorithm.Execute();
                }catch (NewRobotException e)
                {
                 
                    break;
                }
            }
        }

    }
}
