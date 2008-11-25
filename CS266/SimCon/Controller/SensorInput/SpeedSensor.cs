using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class SpeedSensor : SensorInput
    {
        public double speed = 0;
        

        public SpeedSensor() { }

        public SpeedSensor(ControllerWorldState worldState)
        {
            this.worldState = worldState;
        }

        // Given an object, determines whether that object is sensed by the robot
        public double senseSpeed(Robot robot)
        {
            // should return the speed of the robot!! where is this information??
            return 0;
        }

        public override void UpdateSensor()
        {
            speed = senseSpeed(this.robot);
        }
    }

}