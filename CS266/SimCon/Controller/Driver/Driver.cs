using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;


namespace CS266.SimCon.Controller.Driver
{
    class Driver
    {
        public static void Run(String experimentType)
        {
           
            //Check the type of experiment and creat that experiment object
            if (experimentType == "WalkStraight")
            {
                WorldInputInterface wii = new VisionInputInterface();
                WorldOutputInterface woi = new RadioOutputInterface();

                WalkStraightExperiment exp = new WalkStraightExperiment(wii, woi);
                exp.runExperiment();
            }
            
        }
    }
}
