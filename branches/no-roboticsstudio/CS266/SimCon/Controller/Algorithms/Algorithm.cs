using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public abstract class Algorithm
    {

        public Robot Robot;

        public Algorithm(Robot robot)
        {
            Robot = robot;
            // What else?
        }
        public abstract void Execute();
    }
}
