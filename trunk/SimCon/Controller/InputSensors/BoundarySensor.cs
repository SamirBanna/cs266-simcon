using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller.InputSensors
{
    // This sensor is used to check whether an object is close to a boundary 
    public class BoundarySensor : InputSensor
    {
        public double objectSensingAngle = 15; // angle at which it can sense obstacle objects
        // assumed symmetric for + or - direction
        public double objectSensingDist = 10; // distance at which obstacles can be sensed [cm]

        public bool detectObject;


        public BoundarySensor() { }

        public BoundarySensor(double angle, double distance)
        {
            this.objectSensingAngle = angle;
            this.objectSensingDist = distance;
        }

        public BoundarySensor(ControllerWorldState worldState)
        {
            this.worldState = worldState;
        }

        public void SetSensingAngle(double angle)
        {
            objectSensingAngle = angle;
        }

        public void SetSensingDistance(double distance)
        {
            objectSensingDist = distance;
        }

        // checks if point is in the boundaries of the space
        public bool isInside(double px, double py, double minx, double miny, double maxx, double maxy)
        {
            return (px < maxx && px > minx && py < maxy && py > miny);
        }

        // Given the boundaries of the world, determine whether robot senses a boundary
        public bool senseObject(Robot robot, double minx, double miny, double maxx, double maxy)
        {

            Coordinates robotLocation = robot.Location;
            
            double angle = robot.Orientation;

            //Console.WriteLine("********************************************************************************");
            //Console.WriteLine("ROBOT ANGLE = " + angle);
            //Console.WriteLine("********************************************************************************");

            if (angle < 0)
            {
                // convert to [0, 360]
                angle += 360;
            }
            // Convert to radians
            angle = angle * Math.PI / 180;

            Coordinates endLocation = new Coordinates((robotLocation.X + objectSensingDist * Math.Cos(angle)),
                                                       (robotLocation.Y + objectSensingDist * Math.Sin(angle)));

            //Console.WriteLine("Robot location (x,y): " + robotLocation.X + " " + robotLocation.Y);
            //Console.WriteLine("End location (x,y): " + endLocation.X + " " + endLocation.Y);
            //Console.WriteLine("Boundaries (x,y): " +  maxx + " " + maxy);

            if (isInside(endLocation.X, endLocation.Y, minx, miny, maxx, maxy))
            {
                // doesn't sense boundary; no collision
                //Console.WriteLine("In bounds!");
                return false;
            }
            else
            {
                Console.WriteLine("Boundary collision!");
                return true;
            }
        }

        // Update the proximity sensor data fields
        // 1. For all objects in the world  (robot or wall or food or whatever)
        // 2. Check if robot senses any object

        public override void UpdateSensor()
        {
            /// TODO: get information from the world state
            /// 
            detectObject = senseObject(this.robot, 0, 0, worldState.maxX, worldState.maxY);


        }
    }
}

