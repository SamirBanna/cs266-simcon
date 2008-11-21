using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.RoboticsClasses
{
    public class RobotActions
    {
        public RobotAction[] actions;
        public int size;
        public RobotActions(RobotAction[] acts)
        {
            this.actions = acts;
            this.size = acts.Length;
        }
    }
}
