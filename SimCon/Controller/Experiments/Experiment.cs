using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using CS266.SimCon.Controller.Exceptions;

namespace CS266.SimCon.Controller
{
    //Running of each algorithm will be represented as an Experiment
    //The experiment object is reponsible for setting up the WorldInputInterfaces, and WorldOutputInterfaces needed
    //ie, use VisionInput with SimluatorOutput etc.
    //This class will also initialize the robot objects and pass in the appropriate Algorithm object and SensorInput objects as needed
    public abstract class Experiment
    {
        //algorithm to be used for this experiment
        public Algorithm algName;
        public GlobalAlgorithm globalAlg = null;
        //sensors required for this experiment
        public Dictionary<String, SensorInput> sensor;
        //List of the sensors that will be created for this experiment
        public List<String> sensorNames;
        //input and ouput interfaces to be used for this experiment
        public WorldInputInterface Wii;
        public WorldOutputInterface Woi;
        //robots to be used in this experiment;
        public List<Robot> robots;
        public int numRobots;
        //Boolean used to return whether or not the experiment has finished running
        //Used to tell the control loop to stop looping
        //public Boolean isRunning = true;
        
        public Experiment(WorldInputInterface Wii, WorldOutputInterface Woi)
        {
            this.Wii = Wii;
            this.Woi = Woi;
            //this.numRobots = Wii.GetRobots().Count;
        }

        //Setup Experiment should create the Robot class instantiations
        //This could be creating robots based on a specific input, ie the camera
        //The desired implementation is left to the child class
        public abstract void SetupExperiment();

        public void runExperiment()
        {
            
            ControlLoop cl = new ControlLoop(Wii, Woi);
            if(globalAlg != null)
                cl.setGlobalAlgorithm(globalAlg);
            while (true)
            {
                try
                {
                    Console.WriteLine("Running Control Loop");
                    cl.RunLoop();
                    //Thread.Sleep(1000);
                }
                catch (AlgorithmFinishedException e)
                {
                    Console.WriteLine("Experiment Finished by Robot: " + e.robotid);
                    // Check for batch mode
                    if (Wii.GetType() == typeof(SimulatorInputInterface))
                    {
                        CS266.SimCon.Simulator.OurSimulator osNew = ((SimulatorInputInterface)Wii).getOurSimulator().Finished();
                        if (osNew == null)
                        {
                            // We're done
                            break;
                        }
                        else
                        {
                            Wii = new SimulatorInputInterface(osNew);
                            Woi = new SimulatorOutputInterface(osNew);
                            SetupExperiment();
                            runExperiment();
                            break;
                        }
                    }
                }
                catch (GlobalAlgorithmFinishedException e)
                {
                    Console.WriteLine("Experiment Finished globally");
                }
                Thread.Sleep(3000);
            }
        }

       
    }
}
