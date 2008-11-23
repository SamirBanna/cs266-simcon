using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class WalkStraight:Algorithm
    {
        public WalkStraight(Robot r) : base(r) 
        { 
        
        }

        public override void Execute()
        {
            robot.MoveForward(100);                
        }
    }
}
