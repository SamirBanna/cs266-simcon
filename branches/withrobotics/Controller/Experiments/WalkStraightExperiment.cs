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

            
            
        }

        public override void SetupExperiment()
        {
            Wii.setupInitialState();
            robots = Wii.getWorldState().robots;
            foreach (Robot r in robots)
            {
                r.CurrentAlgorithm = new WalkStraight(r);
                r.Sensors = new Dictionary<string, SensorInput>();
            }
        }
    }
}
