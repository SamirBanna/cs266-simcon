// this is the research team's rand walk with obstacle avoidance 
// FileName RandomWalk.cs
using System;
using CS266.SimCon.Controller.InputSensors;

namespace CS266.SimCon.Controller.Algorithms
{
    // Assume the robot has the following sensors:
    // - Food sensor
    // - Obstacle sensor
    // - Boundary Sensor
    // Checks if robot has speed sensor
    class RandomWalk : Algorithm
    {
        public RandomWalk(Robot r)
            : base(r)
        {

        }

        public RandomWalk(Robot r, int degInterval, double probTurn, double moveDistance)
            : base(r)
        {
            this.degInterval = degInterval; // interval at which to randomize angles. 
                                            // E.g. if 30, rand would only give 0, 30, 60, 90, ..., 360
            this.probTurn = probTurn;   // probability of turning when agent isn't moving
            this.moveDistance = moveDistance; // distance to move forward if moving
        }

        int degInterval = 15; // assume divisible by 360
        bool isFinished = false;
        double probTurn = 0.0;
        double moveDistance = 100;    // 100 mm = 10 cm

         
        public override void Execute()
        {
            // Get all sensor information
            bool senseFood = ((FoodSensor)this.robot.Sensors["FoodSensor"]).detectObject;
            bool detectObstacle = ((ObstacleSensor)this.robot.Sensors["ObstacleSensor"]).detectObject;
            bool detectRobot = ((RobotSensor)this.robot.Sensors["RobotSensor"]).detectObject;
            bool detectBoundary = ((BoundarySensor)this.robot.Sensors["BoundarySensor"]).detectObject;
            
            //Console.WriteLine("Detecting another robot" + 

            bool faceObstacle = detectObstacle || detectRobot || detectBoundary;
            bool isMoving = false;
            double speed;


            if(this.robot.Sensors.ContainsKey("SpeedSensor")){
                speed = ((SpeedSensor)this.robot.Sensors["SpeedSensor"]).speed;             
                if(speed > 0) isMoving = true;
            }else{
                speed = 0; // assumed that robot isn't moving when algorithm gets to execute
            }
              
            if (isFinished)
            {
                robot.Turn(180);
                return;
            }
            
            if (senseFood == true)
            {
                Console.WriteLine("robot " + this.robot.Id + " found food");
                isFinished = true; // throw global termination (TODO: check if isFinished does this)
                Finished();

            }
            else if (isMoving == true && faceObstacle == false) // won't be here if speed = 0
            {
                return;
            }
            else if (isMoving == true && faceObstacle == true)  // won't be here if speed = 0
            {
                robot.Stop();
                return;
            }
            else if (faceObstacle == false) // Not facing an obstacle
            { // and agent is not moving    
                Console.WriteLine("robot " + this.robot.Id + " not facing obstacle and not moving");
                double prob = new Random().Next(0, 100) / (double)100; // discretizing probabilities to within 100 b/c we are lazy
                if (prob > probTurn)
                { // shouldn't turn 
                    robot.MoveForward(moveDistance);
                    return;
                }
                else
                { // turn in random direction at interval
                    int howManyIntervals = (int)(360 / degInterval);
                    double turnDegrees = (double)(new Random().Next(0, howManyIntervals)) * degInterval;

                    if (turnDegrees > 180)
                    {
                        turnDegrees = turnDegrees - 360;
                    }

                    while (Math.Abs(turnDegrees) < 60)
                    {
                        turnDegrees = (double)(new Random().Next(0, howManyIntervals)) * degInterval;

                        if (turnDegrees > 180)
                        {
                            turnDegrees = turnDegrees - 360;
                        }

                    }

                    robot.Turn(turnDegrees);
                    return;
                }
            }
            else { // face obstacle and isn't moving
                Console.WriteLine("robot " + this.robot.Id + " facing obstacle and not moving");
                int howManyIntervals = (int)(360 / degInterval);
                double turnDegrees = (double)(new Random().Next(0, howManyIntervals)) * degInterval;

                if (turnDegrees > 180)
                {
                    turnDegrees = turnDegrees - 360;
                }

                //while (Math.Abs(turnDegrees) < 60)
                //{
                //    turnDegrees = (double)(new Random().Next(0, howManyIntervals)) * degInterval;

                //    if (turnDegrees > 180)
                //    {
                //        turnDegrees = turnDegrees - 360;
                //    }

                //}

                //if (turnDegrees > 180)
                //{
                //    turnDegrees = turnDegrees - 360;
                //}

                while (Math.Abs(turnDegrees) > 90)
                {
                    turnDegrees = (double)(new Random().Next(0, howManyIntervals)) * degInterval;

                    if (turnDegrees > 180)
                    {
                        turnDegrees = turnDegrees - 360;
                    }

                }


                robot.Turn(turnDegrees);
                return;
            }
        }
    }
}



