using System;
using System.Collections.Generic;
using CS266.SimCon.Controller.Algorithms;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using CS266.SimCon.Controller.InputSensors;

namespace CS266.SimCon.Controller.Experiments
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

     
        /// <summary>
        /// Setup all the intial values for the experiment.
        /// Create the robots.
        /// Create their sensors.
        /// Set their initial coordinates.
        /// Place in their algorithms.
        /// </summary>
        public override void SetupExperiment()
        {
            Wii.SetupInitialState();
            robots = Wii.GetRobots();
            ControllerWorldState worldstate = Wii.GetWorldState();
            //NOTE: look at SimulatorInputInterface for info on where/how many robots are created
            
            // Create grid           
            double width = worldstate.maxX; // world width in cm
            double height = worldstate.maxY; // world height in cm
            int numXCells = 24; // TODO: what should these values be?
            int numYCells = 12;
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
                r.Sensors = new Dictionary<string, InputSensor>();
               
                foreach (String s in sensorNames)
                {
                    InputSensor sens = SensorList.makeSensor(s);
                    ControllerWorldState ws = Wii.ws;
                    sens.UpdateWorldState(ws);
                    sens.robot = r;
                    r.Sensors.Add(s, sens);
                }
            }
        }
    }

}
