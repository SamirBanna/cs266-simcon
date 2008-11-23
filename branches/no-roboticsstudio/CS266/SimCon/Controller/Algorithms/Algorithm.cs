using System;
using System.Collections.Generic;

using System.Text;

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
            throw new AlgorithmFinishedException(robot.Id);
        }
    }
}
