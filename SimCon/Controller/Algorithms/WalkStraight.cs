using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    /// <summary>
    /// Simple algorithm that just makes a robot walk straight. No collision detection or anything.
    /// </summary>
    public class WalkStraight:Algorithm
    {
        public int x = 0;

        public WalkStraight(Robot r) : base(r) 
        { 
        
        }

        public override void Execute()
        {
            x++;
            //robot.Turn(200);
            robot.MoveForward(100);
            
        }
    }
}
