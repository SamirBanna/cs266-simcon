using System.Collections.Generic;
using CS266.SimCon.Controller.Algorithms;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using CS266.SimCon.Controller.InputSensors;

namespace CS266.SimCon.Controller.Experiments
{
    /// <summary>
    /// Experiment that will just make the robots walk forward
    /// </summary>
    public class WalkStraightExperiment:Experiment
    {

        public WalkStraightExperiment( 
            WorldInputInterface Wii, 
            WorldOutputInterface Woi
            )
            : base(Wii, Woi)
        {
            sensorNames = new List<string>();

            
            
        }

        public override void SetupExperiment()
        {
            Wii.SetupInitialState();
            robots = Wii.getWorldState().robots;
            foreach (Robot r in robots)
            {
                r.CurrentAlgorithm = new WalkStraight(r);
                r.Sensors = new Dictionary<string, InputSensor>();
            }
        }
    }
}
