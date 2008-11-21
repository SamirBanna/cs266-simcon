using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.RoboticsClasses
{
    public class RobotActions
    {
        //PHASE 1
        //public RobotAction[] actions;
        public List<RobotAction> actions;
        public int size;
        public RobotActions(List<RobotAction> acts)
        {
            this.actions = acts;
            this.size = acts.Count;
        }
        //Newly Added
        public RobotActions()
        {
            this.actions = new List<RobotAction>();
            this.size = actions.Count;
        }
        //Newly Added
        public void Add(RobotAction act)
        {
            this.actions.Add(act);
            this.size = this.actions.Count;
        }
    }
    // DID HAVE A "}" HERE
}
