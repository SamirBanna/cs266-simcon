using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class ProximitySensor : SensorInput
    {
        public bool faceObject;
        
        public ProximitySensor(ControllerWorldState worldState)
        {
            List<Robot> robots = worldState.robots;
            List<PhysObject> objects = worldState.physobjects;
            bool senseObject(double maxangle, double maxdistance, Robot robot, PhysObject object);
            double objectSensingAngle = 10; // angle at which it can sense obstacle objects
                                    // assumed symmetric for + or - direction
            double objectSensingDist = 2; // distance at which obstacles can be sensed

            foreach (Robot robot in robots){
                this.faceObject = true;    // reset to false each time
                foreach (PhysObject object in objects){
	            // check if it is near the robot
	            if(senseObject(objectSensingAngle, objectSensingDist, robot, object) == true){
	                this.faceObject = true;
                        break;
                    }
                }
            }

            // returns true if robot senses food in +/- angle within distance
            bool senseObject(double maxangle, double maxdistance, Robot robot, PhysObject object){
                Coordinates objectLocation = object.location;
                Coordinates robotLocation = robot.location;

                double distance = Math.Sqrt(
	            (objectLocation.x - robotLocation.x)*(objectLocation.x - robotLocation.x) + 
	            (objectLocation.y - robotLocation.y)*(objectLocation.y - robotLocation.y));  

                if(distance > maxdistance) return false;

                // get angle between
                double radians = ATan2(objectLocation.y - robotLocation.y, 
		                       objectLocation.x - robotLocation.x); 
                double angle = radians * (180/Math.PI); // from -180 to 180

                // assume orientation is defined from -180 to 180
                // TODO: Is the above true?
                double angleDifference;
                double orientation = robot.orientation;
                if((angle >= 0 && orientation >= 0) || (angle <= 0 && orientation <= 0)){
                    angleDifference = Math.Abs(angle - orientation);
                }else{    //find angle difference by adding their degree distance from 0, then 
                          // finding shorter arc if it exists
                    angleDifference = Math.Abs(angle) + Math.Abs(orientation);
                    if(angleDifference > 360 - angleDifference){
                        angleDifference = 360 - angleDifference;
                    }
                }
                if(angleDifference < maxangle){
                    return true;    
                }
                return false;
            }   
        }
    }
}
