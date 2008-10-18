using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon.Algorithms
{
    class RandomWalk : Algorithm
    {
        public const float minProximity = 5;

        public RandomWalk(Robot r) : base(r)
        {

        }

        bool isFinished = false;

        public void Execute()
        {
            if (isFinished)
            {
                return;
            }

            
            SingleProximityInput spi = (SingleProximityInput)Robot.CurrentInput;

            if (spi.Distance < minProximity && spi.ObjectType != ObjectType.Food)
            {
                float turnDegrees = new Random().Next(-12, 12) * 15;
                Robot.Turn(turnDegrees);
            }
            else if (spi.Distance < minProximity && spi.ObjectType == ObjectType.Food)
            {
                Robot.MoveForward(spi.Distance / 2);
                isFinished = true;
            }
            else
            {
                Robot.MoveForward(minProximity);
            }
        }
    }
}
