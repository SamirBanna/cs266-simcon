using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using System.Diagnostics;
using CS266.SimCon.Simulator;


namespace CS266.SimCon.Controller.Driver
{
    class Driver
    {

        public CS266.SimCon.Simulator.OurSimulator os;

        public void setOurSim(CS266.SimCon.Simulator.OurSimulator os)
        {
            this.os = os;
        }
        public void runSimulator()
        {
            Console.WriteLine("Setting up Wii and Woi");
            WorldInputInterface wii = new SimulatorInputInterface(os);
            WorldOutputInterface woi = new SimulatorOutputInterface(os);

            // Here we need to check for os.Algo type!!!!
            if (os.Algo.name == "DFS")
            {
                DFSExperiment exp = new DFSExperiment(wii, woi);
                exp.SetupExperiment();
                exp.runExperiment();
            }
            else
            {
                RandomWalkExperiment exp = new RandomWalkExperiment(wii, woi);
                exp.SetupExperiment();
                exp.runExperiment();
            }

            //exp.SetupExperiment();
            //exp.runExperiment();
        }

        public static void Run(String experimentType)
        {
            //Process roboRealmProcess = Process.Start("C:\\Documents and Settings\\cs266\\Desktop\\FINAL FIXED RoboRealm\\RoboRealm.exe");
           // System.Threading.Thread.Sleep(2000);

            //Check the type of experiment and create that experiment object
            if (experimentType == "WalkStraight")
            {
                Console.WriteLine("Setting up Wii and Woi");
                WorldInputInterface wii = new VisionInputInterface();
                WorldOutputInterface woi = new RadioOutputInterface();

                WalkStraightExperiment exp = new WalkStraightExperiment(wii, woi);
                exp.SetupExperiment();
                exp.runExperiment();
            }
            else if (experimentType == "RandomWalk")
            {
                Console.WriteLine("Setting up Wii and Woi");
                WorldInputInterface wii = new VisionInputInterface();
                WorldOutputInterface woi = new RadioOutputInterface();

                RandomWalkExperiment exp = new RandomWalkExperiment(wii, woi);
                exp.SetupExperiment();
                exp.runExperiment();
            }
            else if (experimentType == "SimulationRandomWalk")
            {
                
            }


            //roboRealmProcess.Close();
        }
    }
}
