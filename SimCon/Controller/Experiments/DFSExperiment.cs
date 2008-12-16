using System;
using System.Collections.Generic;
using CS266.SimCon.Controller.Algorithms;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using CS266.SimCon.Controller.InputSensors;

namespace CS266.SimCon.Controller.Experiments
{
    /*****
     * 
     * Class to represent the BasicDFS Experiment.
     * Initializes robots
     * 
     * 
     * */


    public class DFSExperiment : Experiment
    {

        public static float doorX = 9.5f;
        public static float doorY = 4.5f;
        private bool moveRandom = false; // whether or not the leader should move randomly

        public DFSExperiment( 
            WorldInputInterface Wii, 
            WorldOutputInterface Woi)
            : base(Wii, Woi)
        {
            //Note the sensorNames list is being generated in the code here
            //However it can easily be extended to read from a Configuration File from the disk
            sensorNames = new List<string>();
            
            //sensorNames.Add("RobotSensor");
            sensorNames.Add("DFSSensor");
            sensorNames.Add("GridSensor");
            sensorNames.Add("BoundarySensor");
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
            //NOTE: look at SimulatorInputInterface for info on where/how many robots are created
            
            // Create grid           
            double width = ((SimulatorInputInterface)Wii).worldWidth; // world width in cm
            double height = ((SimulatorInputInterface)Wii).worldHeight; // world height in cm
            int numXCells = 20; // TODO: what should these values be?
            int numYCells = 9;
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
                r.CurrentAlgorithm = new BasicDFS(r, true, moveRandom); // starting robot is leader
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
