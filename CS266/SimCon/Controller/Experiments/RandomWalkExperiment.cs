using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller
{
    /*****
     * 
     * Class to represent the Random Walk Experiment.
     * Initializes robots
     * 
     * 
     * */
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
        }

        //Setup all the intial values for the experiment
        //Create the robots
        //Create their sensors
        //Set their initial coordinates
        //Place in their algorithms
        public override void SetupExperiment()
        {
            Wii.setupInitialState();
            robots = Wii.getWorldState().robots;
            
            foreach (Robot r in robots)
            {
                r.CurrentAlgorithm = new randWalk(r);
                r.Sensors = new Dictionary<string, SensorInput>();
                foreach (String s in sensorNames)
                {
                    r.Sensors.Add(s, SensorList.makeSensor(s));
                }
            }


        }
    }
}
