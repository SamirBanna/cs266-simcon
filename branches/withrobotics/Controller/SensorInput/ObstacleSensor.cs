using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class ObstacleSensor : ProximitySensor
    {
        // returns true if robot senses obstacle in +/- angle within distance
        public bool senseObstacle(double maxangle, double maxdistance, Robot robot,
                              PhysObject obj)
        {
            return (base.senseObject(maxangle, maxdistance, robot, obj) && (obj.GetType() == typeof(Obstacle)));
        }

        // Checks all physObjects and sees if it is food
        public override void UpdateSensor()
        {
            detectObject = false;    
             bool detect = false;
             
                foreach (PhysObject obj in worldState.physobjects)
                {
                    detect = senseObstacle(objectSensingAngle, objectSensingDist, this.robot, obj);
                    if (detect)
                    {
                        detectObject = true;
                        return;
                    }
                }

            }
        
    }
}
