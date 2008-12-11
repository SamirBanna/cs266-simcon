using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class NewRobotException : System.Exception
    {
        public Robot robot;

        public NewRobotException(Robot newRobot){
            this.robot = newRobot;
        }
    }
}
