using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller.WorldOutputInterfaces;

namespace CS266.SimCon.Controller
{
    public abstract class Algorithm
    {

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
