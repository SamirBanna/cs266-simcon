// this is the research team's node counting algorithm
// FileName nodeCounting.cs
using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller;

using CS266.SimCon.Controller.InputSensors;

namespace CS266.SimCon.Controller.Algorithms
{
    // Assume the robot has the following sensors:
    // - Food sensor
    // - Obstacle sensor
    // - Boundary Sensor
    // - Grid Sensor
    // Checks if robot has speed sensor

    /// <summary>
    /// Assume the robot has the following sensors:
    /// - Food sensor
    /// - Obstacle sensor
    /// - Boundary Sensor
    /// - Grid Sensor
    /// Checks if robot has speed sensor
    /// </summary>
    class NodeCounting : Algorithm
    {
        /// <summary>
        /// algorithm is trying to avoid and is in the obstacle avoidance routine
        /// </summary>
        public bool finishedTurning = false;
        int lastcol = 0;
        int lastrow = 0;


        public NodeCounting(Robot r)
            : base(r)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="degInterval">Interval at which to randomize angles. E.g. if 30, rand would only give 0, 30, 60, 90, ..., 360</param>
        /// <param name="probTurn">Probability of turning when agent isn't moving</param>
        /// <param name="moveDistance">Distance to move forward if moving</param>
        public NodeCounting(Robot r, int degInterval, double probTurn, double moveDistance)
            : base(r)
        {
            this.degInterval = degInterval;
            this.probTurn = probTurn;
            this.moveDistance = moveDistance;
            
        }

        int degInterval = 15; // assumes divisible by 360
        bool isFinished = false;
        double probTurn = 0.0;
        double moveDistance = 300.0;


        public override void Execute()
        {
            ControlLoop.robotGrid.GridUpdate(this.robot);
            if (ControlLoop.robotGrid.gridVisited())
            {
                Console.WriteLine("COVERED BABY - boots with the fur");
                isFinished = true;
                Finished();
                return;
            }
            Console.WriteLine("Updated grid already");
            // Get all sensor information
            bool senseFood = ((FoodSensor)this.robot.Sensors["FoodSensor"]).detectObject;
            bool detectObstacle = ((ObstacleSensor)this.robot.Sensors["ObstacleSensor"]).detectObject;
            bool detectRobot = ((RobotSensor)this.robot.Sensors["RobotSensor"]).detectObject;
            bool detectBoundary = ((BoundarySensor)this.robot.Sensors["BoundarySensor"]).detectObject;
            double[,] localgrid = ((LocalGridSensor)this.robot.Sensors["LocalGridSensor"]).localgrid;
            bool faceObstacle = detectObstacle || detectRobot || detectBoundary;
            bool isMoving = false;
            double speed;
            double turnDegrees;

            ControlLoop.robotGrid.PrintGrid();
            
            //ControlLoop.robotGrid.Mark(robot, true);
            if (this.robot.Sensors.ContainsKey("SpeedSensor"))
            {
                speed = ((SpeedSensor)this.robot.Sensors["SpeedSensor"]).speed;
                if (speed > 0) isMoving = true;
            }
            else
            {
                speed = 0; // assumed that robot isn't moving when algorithm gets to execute
            }

            if (isFinished)
            {
                robot.Turn(180);
                return;
            }

            // Initiate algorithm
            if (senseFood == true)
            {
                isFinished = true; // throw global termination (TODO: check if isFinished does this)
                Finished();
                return;
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

            if (finishedTurning && faceObstacle)
            {
                // Initiate avoidance
                // In this state until we have moved in some random direction
                Console.WriteLine("Facing obstacle");

                int howManyIntervals = (int)(360 / degInterval);
                turnDegrees = (double)(new Random().Next(0, howManyIntervals)) * degInterval;

                if (turnDegrees > 180)
                {
                    turnDegrees = turnDegrees - 360;
                }
                robot.Turn(turnDegrees);
                return;
            }

            if (finishedTurning && !faceObstacle){
               // if direction is chosen and legal, just move.
             //  robot.MoveForward((double)moveDistance);
                robot.MoveForward(150);

               // robot only moves here. Before moving, mark your spot
               

               finishedTurning = false; // so we choose a direction next time 
                    // TODO: maybe have propensity to just move forward sometimes?
               return;
            }


            // Else, Run Node counting to figure out direction to move in
            //                Figure out direction to moveDistance in.
            // Check all 8 directions
                double[,] directionalvalues = new double[3, 3];
                // for now, just fill in based on the 8 squares closest to the robot, placed at grid center
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // TODO: assume local grid is also 3x3 [otherwise, get robot's loc in grid and look there]
                        directionalvalues[i, j] = localgrid[i, j];
                    }
                }

                // Find direction with smallest value
                int rowIndexOfSmallest = 0;
                int colIndexOfSmallest = 0;

                if (directionalvalues[lastcol, lastrow] >= 0)
                {//if the last direction is legal
                    colIndexOfSmallest = lastcol;
                    rowIndexOfSmallest = lastrow;
                }
                else
                {
                    bool found = false;
                    for (colIndexOfSmallest = 0; colIndexOfSmallest < 3; colIndexOfSmallest++)
                    {
                        for (rowIndexOfSmallest = 0; rowIndexOfSmallest < 3; rowIndexOfSmallest++)
                        {
                            if (directionalvalues[colIndexOfSmallest, rowIndexOfSmallest] >= 0)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found == true)
                        {
                            break;
                        }
                    }
                }

                
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1)
                        {// robot's current square, ignore
                            continue;
                        }
                        if (directionalvalues[i,j] < 0)
                        {
                            continue;
                        }
                        // Comparator is < here to give tie-breaking preference to the 
                        // directionvalue at [lastcol, lastrow]
                        if (directionalvalues[i, j] < directionalvalues[colIndexOfSmallest, rowIndexOfSmallest])
                        {
                            colIndexOfSmallest = i;
                            rowIndexOfSmallest = j;
                        }
                    }
                }
                
                // Update the last chosen directional index
                Console.WriteLine("Last col and row: " + lastcol + " " + lastrow);
                
                lastcol = colIndexOfSmallest;
                lastrow = rowIndexOfSmallest;

                Console.WriteLine("Current col and row: " + lastcol + " " + lastrow);

            //    Console.WriteLine("Node counting says Moving to " + colIndexOfSmallest + ", " + rowIndexOfSmallest);
                // get base direction (angles from -180 to 180 for the agent to go in)
                double angle = getBaseDirection(colIndexOfSmallest, rowIndexOfSmallest);

                Console.WriteLine("Angle to go: " + angle);
                

                // get robot orientation
                double orientation = this.robot.Orientation;

                Console.WriteLine("Robot's current orientation: " + orientation);

                // Figure out how many angles to get the robot to turn, 
                turnDegrees = (double)(angle - orientation);

                // turnDegrees is between 0 and 360, try to turn on short edge s.t. range is [-180,180]
                if (Math.Abs(turnDegrees) > 180)
                {
                    if (turnDegrees < 0) //turnDegrees < -180
                    {
                        turnDegrees += 360;
                    }
                    else
                    { // turnDegrees > 180 and 
                        turnDegrees -= 360;
                    }
                }

                 Console.WriteLine("Turn this much: " + turnDegrees);
               
                // Check turnDegrees. If it is very small (hard-coded to 5), 
                // we prefer to go in our current direction if there is no obstacle there
                if (Math.Abs(turnDegrees) < 5 && !faceObstacle)
                {
                    // can go in current direction, so let's go and set finishedTurning to false so node counting will choose
                    // direction next time again
                    robot.MoveForward(150);
                    finishedTurning = false;
                    return;
                }
                else
                { /// turn as usual and leave figuring out direction for next time
                    robot.Turn(turnDegrees);
                    finishedTurning = true;
                    return;
                }


        }
        

        /// <summary>
        /// Figure out the direction to move to based on the cell with the largest value
        /// </summary>
        /// <param name="colIndexOfSmallest"></param>
        /// <param name="rowIndexOfSmallest"></param>
        /// <returns></returns>
        public double getBaseDirection(int colIndexOfSmallest, int rowIndexOfSmallest)
        {
            

            double basedirection = 0;

            if (colIndexOfSmallest == 0 && rowIndexOfSmallest == 2)
            {
                // go in northwest direction, taking into account current orientation
                basedirection = 135;
            }
            else if (colIndexOfSmallest == 1 && rowIndexOfSmallest == 2)
            { // North
                basedirection = 90;
            }
            else if (colIndexOfSmallest == 2 && rowIndexOfSmallest == 2)
            { // Northeast
                basedirection = 45;
            }
            else if (colIndexOfSmallest == 0 && rowIndexOfSmallest == 1)
            { // West
                basedirection = 180;
            }
            else if (colIndexOfSmallest == 2 && rowIndexOfSmallest == 1)
            { // East
                basedirection = 0;
            }
            else if (colIndexOfSmallest == 0 && rowIndexOfSmallest == 0)
            { // Southwest
                basedirection = -135;
            }
            else if (colIndexOfSmallest == 1 && rowIndexOfSmallest == 0)
            { // South
                basedirection = -90;
            }
            else if (colIndexOfSmallest == 2 && rowIndexOfSmallest == 0)
            { // Southeast
                basedirection = -45;
            }
            return basedirection;
        }

    }
}
