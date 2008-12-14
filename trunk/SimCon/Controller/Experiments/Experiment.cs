using System;
using System.Collections.Generic;
using System.Threading;
using CS266.SimCon.Controller.Algorithms;
using CS266.SimCon.Controller.Exceptions;
using CS266.SimCon.Controller.WorldInputInterfaces;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using CS266.SimCon.Controller.InputSensors;

namespace CS266.SimCon.Controller.Experiments
{
    
    /// <summary>
    /// Running of each algorithm will be represented as an Experiment
    /// The experiment object is reponsible for setting up the WorldInputInterfaces, and WorldOutputInterfaces needed
    /// ie, use VisionInput with SimluatorOutput etc.
    /// This class will also initialize the robot objects and pass in the appropriate Algorithm object and InputSensor objects as needed
    /// </summary>
    public abstract class Experiment
    {
        /// <summary>
        /// Algorithm to be used for this experiment
        /// </summary>
        /// 
        public Algorithm algName;
        public GlobalAlgorithm globalAlg = null;

        /// <summary>
        /// Sensors required for this experiment 
        /// </summary>
        public Dictionary<String, InputSensor> sensor;


        
        /// <summary>
        /// List of the sensors that will be created for this experiment 
        /// </summary>
        public List<String> sensorNames;
        
        /// <summary>
        /// Input interface to be used for this experiment
        /// </summary>
        public WorldInputInterface Wii;

        /// <summary>
        /// Output interface to be used for this experiment
        /// </summary>
        public WorldOutputInterface Woi;

        /// <summary>
        /// Robots to be used in this experiment;
        /// </summary>
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

        /// <summary>
        /// Setup Experiment should create the Robot class instantiations.
        /// This could be creating robots based on a specific input, ie the camera
        /// The desired implementation is left to the child class
        /// </summary>
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
