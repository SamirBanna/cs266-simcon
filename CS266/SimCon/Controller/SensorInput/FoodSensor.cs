using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class FoodSensor : SensorInput
    {
        // Bisection angle at which food can be sensed:
        double foodSensingAngle = 10;
        // Sensing range:
        double foodSensingDist = 2;
        public ControllerWorldState worldState;
        public bool faceFood;
        
        public FoodSensor(ControllerWorldState worldState)
        {
            this.worldState = worldState;
            
        }

        // returns true if robot senses food in +/- angle within distance
        public bool senseFood(double maxangle, double maxdistance, Robot robot,
                              PhysObject obj)
        {
            if(obj.Name != 'Food'){
                return false;
            }

            Coordinates foodLocation = obj.Location;
            Coordinates robotLocation = robot.Location;
            
            
            double distance = Math.Sqrt((foodLocation.X - robotLocation.X)*
                                        (foodLocation.X - robotLocation.X) + 
                                        (foodLocation.Y - robotLocation.Y)*
                                        (foodLocation.Y - robotLocation.Y));  
            
            if(distance > maxdistance) return false;
            
            // get angle between
            double radians = Math.Atan2(foodLocation.Y - robotLocation.Y, 
                                        foodLocation.X - robotLocation.X); 
            double angle = radians * (180/Math.PI); // from -180 to 180
            
            // assume orientation is defined from -180 to 180
            // TODO: Is the above true?
            double angleDifference;
            double orientation = robot.Orientation;
            if((angle >= 0 && orientation >= 0) || 
               (angle <= 0 && orientation <= 0)){
                angleDifference = Math.Abs(angle - orientation);
            }else{    //find angle difference by adding degree distance from 0,
                      //then finding shorter arc if it exists
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
