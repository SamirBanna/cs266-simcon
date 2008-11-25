using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class FoodSensor : ProximitySensor
    {
        // returns true if robot senses food in +/- angle within distance
        public bool senseFood(double maxangle, double maxdistance, Robot robot,
                              PhysObject obj)
        {
            return (senseObject(maxangle, maxdistance, robot, obj) && (obj.GetType() == typeof(Food)));
        }

        // Checks all physObjects and sees if it is food
        public override void UpdateSensor()
        {
                 
             bool detect = false;

                foreach (PhysObject obj in worldState.physobjects)
                {
                    detect = senseFood(objectSensingAngle, objectSensingDist, this.robot, obj);
                    if (detect)
                    {
                        detectObject = true;
                        return;
                    }
                }

            }
        
    }
}
