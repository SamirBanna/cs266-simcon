using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller.InputSensors
{
    public class FoodSensor : ProximitySensor
    {
        // returns true if robot senses food in +/- angle within distance
        public bool senseFood(double maxangle, double maxdistance, Robot robot,
                              PhysObject obj)
        {
            Console.WriteLine("object location: " + obj.Location.X + " " + obj.Location.Y);
            if (obj.GetType() == typeof(Food))
            {
                Console.WriteLine("THIS IS A FOOD OBJECT");
                
            }
            return (base.senseObject(maxangle, maxdistance, robot, obj) && (obj.GetType() == typeof(Food)));
        }

        // Checks all physObjects and sees if it is food
        public override void UpdateSensor()
        {
            detectObject = false;
            bool detect = false;

                foreach (PhysObject obj in worldState.foodlist)
                {
                    detect = senseFood(objectSensingAngle, objectSensingDist, this.robot, obj);
                    if (detect)
                    {
                        detectObject = true;
                        Console.WriteLine("DETECTED FOOD!");
                        return;
                    }
                }

            }
        
    }
}
