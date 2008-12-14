using System;
using System.Collections.Generic;
using CS266.SimCon.Controller.Algorithms;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using CS266.SimCon.Controller.InputSensors;

namespace CS266.SimCon.Controller.Experiments
{

    /// <summary>
    /// Class to represent the Random Walk Experiment.
    /// Initializes robots.
    /// </summary>
    public class RandomWalkExperiment:Experiment
    {

        public RandomWalkExperiment( 
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
            Wii.setupInitialState();
            robots = Wii.GetRobots();
            
            foreach (Robot r in robots)
            {
                r.CurrentAlgorithm = new RandomWalk(r);
                r.Sensors = new Dictionary<string, InputSensor>();

                
                foreach (String s in sensorNames)
                {
                    InputSensor sens = SensorList.makeSensor(s);
                    //Console.WriteLine(s);
                    ControllerWorldState ws = Wii.ws;
                    //Console.WriteLine("getting ws");
                    sens.UpdateWorldState(ws);
                    sens.robot = r;
                    r.Sensors.Add(s, sens);
                }
            }

            //Console.WriteLine("finish setup experiment");
        }
    }
}
