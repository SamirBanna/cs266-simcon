using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class ProximitySensor : SensorInput
    {
        public double objectSensingAngle = 10; // angle at which it can sense obstacle objects
                                    // assumed symmetric for + or - direction
        public double objectSensingDist = 2; // distance at which obstacles can be sensed
        
        public bool detectObject;
        public ControllerWorldState worldState;

        public ProximitySensor() { }

        public ProximitySensor(double angle, double distance) {
            this.objectSensingAngle = angle;
            this.objectSensingDist = distance;
        }

        public ProximitySensor(ControllerWorldState worldState)
        {
            this.worldState = worldState;          
        }
  
        public void SetSensingAngle(double angle){
            objectSensingAngle = angle;
        }

        public void SetSensingDistance(double distance)
        {
            objectSensingDist = distance;
        }

        // Given an object, determines whether that object is sensed by the robot
        public bool senseObject(double maxangle, double maxdistance, Robot robot, PhysObject obj){
                Coordinates objectLocation = obj.Location;
                Coordinates robotLocation = robot.Location;


                double distance = Math.Sqrt(
	            (objectLocation.X - robotLocation.X)*(objectLocation.X - robotLocation.X) + 
	            (objectLocation.Y - robotLocation.Y)*(objectLocation.Y - robotLocation.Y));  

                if(distance > maxdistance) return false;

                // get angle between
                double radians = Math.Atan2(objectLocation.Y - robotLocation.Y, 
		                       objectLocation.X - robotLocation.X); 
                double angle = radians * (180/Math.PI); // from -180 to 180

                // assume orientation is defined from -180 to 180
                // TODO: Is the above true?
                double angleDifference;
                double orientation = robot.Orientation;
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
            
            // Update the proximity sensor data fields
            // 1. For all objects in the world  (robot or wall or food or whatever)
            // 2. Check if robot senses any object
         
            public override void UpdateSensor()
            {
                bool detect = false;

                foreach (PhysObject obj in worldState.physobjects)
                {
                    detect = senseObject(objectSensingAngle, objectSensingDist, this.robot, obj);
                    if (detect)
                    {
                        detectObject = true;
                        return;
                    }
                }

               foreach (Robot rob in worldState.robots)
                {
                        detect = senseObject(objectSensingAngle, objectSensingDist, this.robot, rob);
                        if (detect)
                        {
                            detectObject = true;
                            return;
                        }
                }
            }
        }
    }

