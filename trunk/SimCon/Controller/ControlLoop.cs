using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using CS266.SimCon.Controller.Exceptions;

using CS266.SimCon.Controller.Algorithms;
using CS266.SimCon.Controller.InputSensors;

namespace CS266.SimCon.Controller
{
    class ControlLoop
    {
        Dictionary<int, Robot> Robots = new Dictionary<int,Robot>();
        Dictionary<int, PhysObject> WorldObjects = new Dictionary<int,PhysObject>();
        WorldInputInterface Wii;
        WorldOutputInterface Woi;
        ControllerWorldState worldState;
        GlobalAlgorithm globalAlgorithm;

        /// <summary>
        /// Robot grid. This should be instantiated in Experiments with a reference to worldState, and dimensions
        /// </summary>
        public static Grid robotGrid;

        public static Queue<PhysicalRobotAction> ActionQueue = new Queue<PhysicalRobotAction>();

        /// <summary>
        /// Constructor. Initialize the robots. Initialize the world state.
        /// </summary>
        /// <param name="Wii"></param>
        /// <param name="Woi"></param>
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
            // Assume default for now. Experiment class can change after
            globalAlgorithm = new DefaultGlobalAlgorithm();
        }

        public void setGlobalAlgorithm(GlobalAlgorithm alg)
        {
            this.globalAlgorithm = alg;
        }

        /// <summary>
        /// Runs the actual control loop
        /// </summary>
        public void RunLoop()
        {
           // Console.WriteLine("Number of robots in the ControlLoop: " + Robots.Count);
            this.GetInput();
            //Console.WriteLine("got input");

            this.RunAlgorithms();
            
            this.RunActionQueue(); 
        }


        private void RunActionQueue()
        {
            foreach (PhysicalRobotAction action in ActionQueue)
            {
                Woi.DoActions(action);
            }
            ActionQueue.Clear();
        }
    

        /// <summary>
        /// Updates the states of robots and objects with new information
        /// 1. Get's the world state from the world input interface
        /// 2. update each robot's local view by updating that robots' sensors
        /// </summary>
        private void GetInput()
        {
           worldState = Wii.getWorldState();
           //  Console.WriteLine("Control Loop WS Angle before sensor: "+ worldState.robots[0].Orientation);
           // Console.WriteLine("controloop con");

            Console.WriteLine("*****Number of objects in the world: " + (worldState.physobjects.Count) + "*******");
            Console.WriteLine("**********Number of food objects in the world: " + (worldState.foodlist.Count) + "*******");
            //update robot orientation and location
            foreach (Robot z in worldState.robots)
            {
                
                if (Robots.ContainsKey(z.Id))
                {
                    Robots[z.Id].Orientation = z.Orientation;
                    Robots[z.Id].Location = z.Location;
                }
            }

            foreach (Robot r in Robots.Values)
            {
                foreach (InputSensor sensor in r.Sensors.Values)
                {
                    
                    sensor.UpdateWorldState(worldState);
                  //  Console.WriteLine("SENSOR AFTER WORLD STATE UPDATE: " + sensor.worldState.robots[0].Orientation);
                    sensor.UpdateSensor();
                }
            }
        }

        /// <summary>
        /// Iterate through list of robots, and all their algorithm's execute function
        /// </summary>
        private void RunAlgorithms()
        {
            foreach (Robot r in Robots.Values)
            {
                try
                {
                    r.CurrentAlgorithm.Execute();
                }
                catch (NewRobotException e)
                {
                    Robots.Add(e.robot.Id, e.robot);
                    break;
                }
            }
            globalAlgorithm.Execute();

        }
    }
}

