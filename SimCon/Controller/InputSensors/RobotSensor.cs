using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller.InputSensors
{
    // senses other robots
    public class RobotSensor : ProximitySensor
    {
        // returns true if robot senses another robot in +/- angle within distance
        public bool senseRobot(double maxangle, double maxdistance, Robot robot,
                              PhysObject obj)
        {
            if (obj.GetType() == typeof(Robot))
            {
                Robot other = (Robot)obj;
                if (Math.Abs(other.Location.X - robot.Location.X) < 5 &&
                    Math.Abs(other.Location.Y - robot.Location.Y) < 5)
                {
                    return false;
                }
            }

            return senseObject(maxangle, maxdistance, robot, obj) && (obj.GetType() == typeof(Robot));
        }

        // Checks all robots and sees if any of them is close to the current
        public override void UpdateSensor()
        {
             detectObject = false;        
             bool detect = false;

                foreach (Robot rob in worldState.robots)
                {
                    if (rob.Id != this.robot.Id)
                    {
                        detect = senseRobot(objectSensingAngle, objectSensingDist, this.robot, rob);
                        if (detect)
                        {
                            Console.WriteLine("I'm detecting another Robot: " + rob.Id);
                            Console.WriteLine("My on ID: " + robot.Id);
                            detectObject = true;
                            return;
                        }
                    }
                }

            }
        
    }
}
