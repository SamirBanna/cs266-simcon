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
            sensorNames.Add("ProxmitySensor");
        }

        //Setup all the intial values for the experiment
        //Create the robots
        //Create their sensors
        //Set their initial coordinates
        //Place in their algorithms
        public override void SetupExperiment()
        {
            base.robots = new List<Robot>();
            for (int x = 0; x < numRobots; x++)
            {
                Robot r = new Robot(numRobots, numRobots.ToString(), new Coordinates(0, 0), 0, 0, 0);
                r.CurrentAlgorithm = new RandomWalk(r);
                Dictionary<String, SensorInput> sensors = new Dictionary<String, SensorInput>();
                //create the sensors
                foreach (String name in sensorNames)
                {
                    //the Sensors are being created by their names from the sensorName List<String>
                    //this is used since we would like eventually allow the use of config files to specify which sensors to use
                    sensors.Add(name,SensorList.makeSensor(name));
                }
            }
        }
    }
}
