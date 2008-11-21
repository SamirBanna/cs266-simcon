using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.RoboticsClasses
{
    public class RobotAction
    {
        public string name;
        public float newdegreesfromx;
        public float distance;
        public RobotAction(string name, float newdegreesfromx, float distance)
        {
            this.name = name;
            this.newdegreesfromx = newdegreesfromx;
            this.distance = distance;
        }
    }
}
