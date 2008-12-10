using System;
using System.Collections.Generic;

using System.Text;

using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;

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

        /// <summary>
        /// Robot grid. This should be instantiated in Experiments with a reference to worldState, and dimensions
        /// </summary>
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
                Console.WriteLine("******************Robot ID "+robot.Id);
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
            //Console.WriteLine("Number of robots in the ControlLoop: " + Robots.Count);
            this.GetInput();
            //Console.WriteLine("got input");
            Console.WriteLine("Number of robots in the ControlLoop: " + Robots.Count);
            System.Threading.Thread.Sleep(1500);

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
            worldState = (Wii).getNewWorldState();
          //  Console.WriteLine("Control Loop WS Angle before sensor: "+ worldState.robots[0].Orientation);
           // Console.WriteLine("controloop con");

            Console.WriteLine("*****Number of objects in the world: " +worldState.physobjects.Count + "*******");
            
            
            //update robot orientation and location
            // NOTE: Heather, Jella, and Diana wonder why this is necessary, since robot dictionary contains
            // actual robot objects??
            //we propose: delete if-else and just say: 
            // if (!Robots.ContainsKey(z.Id)) Robots.Add(robot.Id,robot);

            foreach (Robot z in worldState.robots)
            {
                if (!Robots.ContainsKey(z.Id)) Robots.Add(z.Id, z);
            }
            //foreach (Robot z in worldState.robots)
            //{
            //    //if we already know about this robot, then update parametes; otherwise create and add
            //    if (Robots.ContainsKey(z.Id))
            //    {
            //        Robots[z.Id].Orientation = z.Orientation;
            //        Robots[z.Id].Location = z.Location;
            //    }
            //    else
            //    {
            //        // create a new robot. TODO: should 7,7 be z.width and z.height?
            //        Robot newRobot = new Robot(z.Id, z.Name, z.Location, z.Orientation, 7, 7);
                    
            //        // copy algorithm and action
            //        newRobot.CurrentAction = z.CurrentAction;
            //        newRobot.CurrentAlgorithm = z.CurrentAlgorithm;
                    
                    //code from DFSExperiment to deepcopy the sensors over
                    //foreach (String s in z.Sensors.Keys)
                    //{
                    //    SensorInput sens = SensorList.makeSensor(s);
                    //    ControllerWorldState ws = Wii.ws;
                    //    sens.UpdateWorldState(ws);
                    //    sens.robot = newRobot;
                    //    newRobot.Sensors.Add(s, sens);
                    //}
                    
            //        //code from DFSExperiment to deepcopy the sensors over
            //        foreach (String s in z.Sensors.Keys)
            //        {
            //            SensorInput sens = SensorList.makeSensor(s);
            //            ControllerWorldState ws = Wii.ws;
            //            sens.UpdateWorldState(ws);
            //            sens.robot = newRobot;
            //            newRobot.Sensors.Add(s, sens);
            //        }
            //        Robots.Add(newRobot.Id, newRobot);
            //    }
            //}

            //A hack until Angela adds sensors and an algorithm to the newly added robot
            //int j = 0;

            foreach (Robot r in Robots.Values)
            {
                //j++;
                //if (j == 3) break;
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
            //Another hack until angela add the sensor/algo stuff

            //int j = 0;
            foreach (Robot r in Robots.Values)
            {
                //j++;
                //if (j == 3) break;
                r.CurrentAlgorithm.Execute();
            }
        }

    }
}
