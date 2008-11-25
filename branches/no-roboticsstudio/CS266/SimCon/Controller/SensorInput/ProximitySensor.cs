using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class ProximitySensor : SensorInput
    {
        public double objectSensingAngle = 45; // angle at which it can sense obstacle objects
                                    // assumed symmetric for + or - direction
        public double objectSensingDist = 80; // distance at which obstacles can be sensed
        
        public bool detectObject;
        

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

                //Console.WriteLine("==================Proximity Sensor===========================");
                //Console.WriteLine("x obstacle = " + objectLocation.X);
                //Console.WriteLine("y obstacle = " + objectLocation.Y);
                //Console.WriteLine("x robot    = " + robotLocation.X);
                //Console.WriteLine("y robot    = " + robotLocation.Y);
                //Console.WriteLine("orientation robot = " + robot.Orientation);
                

                //Console.WriteLine("DISTANCE = " + distance);
                //Console.WriteLine("maxdistance =");
                //System.Console.WriteLine(maxdistance);

                if (distance > maxdistance)
                {
                    //Console.WriteLine("Detectable distance:" + maxdistance);
                    //Console.WriteLine("Actual Distance: " + distance);
                    //Console.WriteLine("Too far away, cannot sense it. ");
                     
                    return false;
                }
                //Console.WriteLine("went through");

                // get angle between
                double radians = Math.Atan2(objectLocation.Y - robotLocation.Y,
                               objectLocation.X - robotLocation.X);// -0.5 * Math.PI;    // -pi/2 factor shifts frame
                                                                                     // according to definition in
                                                                                     // vision
                double angle = radians * (180/Math.PI); // from -180 to 180

                Console.WriteLine("ROBOT-OBSTACLE ANGLE = " + angle);

                // assume orientation is defined from -180 to 180
                // TODO: Is the above true?
                double angleDifference;
                double orientation = robot.Orientation;

                //Console.WriteLine("ROBOT DIRECTION = " + orientation);

                if((angle >= 0 && orientation >= 0) || (angle <= 0 && orientation <= 0)){

                    //Console.WriteLine("ANGLES HAVE SAME SIGN");

                    angleDifference = Math.Abs(angle - orientation);
                }else{    //find angle difference by adding their degree distance from 0, then 
                          // finding shorter arc if it exists

                    //Console.WriteLine("ANGLES HAVE DIFFERENT SIGNS");

                    angleDifference = Math.Abs(angle) + Math.Abs(orientation);
                    if(angleDifference > (360 - angleDifference)){
                        angleDifference = 360 - angleDifference;
                    }
                }

                //Console.WriteLine("ANGLE DIFFERENCE = " + angleDifference);
                //Console.WriteLine("=============================================");
                if(angleDifference < maxangle){
                    //Console.WriteLine("max angle:" + maxangle);
                    //Console.WriteLine("Actual angle: " + angle);
                    //Console.WriteLine("Angle difference is less than max angle");
                    return true;    
                }
                //Console.WriteLine("before last false");
                return false;
            }
            
            // Update the proximity sensor data fields
            // 1. For all objects in the world  (robot or wall or food or whatever)
            // 2. Check if robot senses any object
         
            public override void UpdateSensor()
            {
                detectObject = false;    
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

