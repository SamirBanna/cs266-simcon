using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;

namespace CS266.SimCon.Controller
{
    class NodeCountingExperiment:Experiment
    {
        public NodeCountingExperiment( 
            WorldInputInterface Wii, 
            WorldOutputInterface Woi)
            : base(Wii, Woi)
        {
            //Note the sensorNames list is being generated in the code here
            //However it can easily be extended to read from a Configuration File from the disk
            sensorNames = new List<string>();
            
            sensorNames.Add("RobotSensor");
            sensorNames.Add("ObstacleSensor");
            sensorNames.Add("FoodSensor");
            sensorNames.Add("BoundarySensor");
            sensorNames.Add("LocalGridSensor");
        }

     
            //Setup all the intial values for the experiment
        //Create the robots
        //Create their sensors
        //Set their initial coordinates
        //Place in their algorithms
        public override void SetupExperiment()
        {
            Wii.setupInitialState();
            robots = Wii.GetRobots();
            ControllerWorldState worldstate = Wii.getWorldState();
            //NOTE: look at SimulatorInputInterface for info on where/how many robots are created
            
            // Create grid           
            double width = worldstate.maxX; // world width in cm
            double height = worldstate.maxY; // world height in cm
            int numXCells = 2; // TODO: what should these values be?
            int numYCells = 2;
            ControlLoop.robotGrid = new Grid(Wii.ws, width, height, numXCells, numYCells);

            // Add food, obstacles, robots to grid
            List<PhysObject> objectList = Wii.GetPhysObjects();
            List<Robot> robotList = Wii.GetRobots();
            foreach (PhysObject obj in objectList)
            {
                ControlLoop.robotGrid.getGridLoc(obj.Location).objectsInSquare.Add(obj);
            }
            foreach (Robot r in robotList)
            {
                ControlLoop.robotGrid.getGridLoc(r.Location).objectsInSquare.Add(r);
            }

            // Experimenter class creates one robot (the leader)
            foreach (Robot r in robots)
            {
                

                r.CurrentAlgorithm = new NodeCounting(r, 30, 0.5, 100); // starting robot is leader
                r.Sensors = new Dictionary<string, SensorInput>();
               
                foreach (String s in sensorNames)
                {
                    SensorInput sens = SensorList.makeSensor(s);
                    ControllerWorldState ws = Wii.ws;
                    sens.UpdateWorldState(ws);
                    sens.robot = r;
                    r.Sensors.Add(s, sens);
                }
            }
        }
    }

}
