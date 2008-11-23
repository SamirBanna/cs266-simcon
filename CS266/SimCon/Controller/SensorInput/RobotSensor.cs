using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    // senses other robots
    public class RobotSensor : ProximitySensor
    {
        // returns true if robot senses another robot in +/- angle within distance
        public bool senseRobot(double maxangle, double maxdistance, Robot robot,
                              PhysObject obj)
        {
            return (senseObject(maxangle, maxdistance, robot, obj) && (obj.GetType() == typeof(Robot)));
        }

        // Checks all robots and sees if any of them is close to the current
        public override void UpdateSensor()
        {
                 
             bool detect = false;

                foreach (Robot rob in worldState.robots)
                {
                    detect = senseRobot(objectSensingAngle, objectSensingDist, this.robot, rob);
                    if (detect)
                    {
                        detectObject = true;
                        return;
                    }
                }

            }
        
    }
}
