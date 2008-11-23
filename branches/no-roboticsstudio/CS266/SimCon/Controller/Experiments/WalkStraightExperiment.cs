using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;


namespace CS266.SimCon.Controller
{
    /**
     * 
     * 
     * Experiment that will just make the robots walk forward
     * 
     * */
    public class WalkStraightExperiment:Experiment
    {

        public WalkStraightExperiment( 
            WorldInputInterface Wii, 
            WorldOutputInterface Woi
            )
            : base(Wii, Woi)
        {
            sensorNames = new List<string>();
      
            //sensorNames.Add("ProximitySensor");
            
        }

        public override void SetupExperiment()
        {
            Robot r = new Robot(numRobots, numRobots.ToString(), new Coordinates(0, 0), 0, 0, 0);
            r.CurrentAlgorithm = new RandomWalk(r);
            Dictionary<String, SensorInput> sensors = new Dictionary<String, SensorInput>();
            //create the sensors
            foreach (String name in sensorNames)
            {
                sensors.Add(name, SensorList.makeSensor(name));
            }
        }
    }
}
