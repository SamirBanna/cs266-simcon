using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller.WorldOutputInterfaces;

namespace CS266.SimCon.Controller.Algorithms
{
    /// <summary>
    /// Abstract class from which all other Algorithms should inherit
    /// </summary>
    public abstract class Algorithm
    {

        /// <summary>
        /// Reference to the robot executing the algorithm
        /// </summary>
        public Robot robot;

        public Algorithm(Robot robot)
        {
            this.robot = robot;
            // What else?
        }

        public abstract void Execute();

        
        public void Finished()
        {
            Console.WriteLine("WE ARE DONE HERE");
            //throw new AlgorithmFinishedException(robot.Id);
        }
    }
}
