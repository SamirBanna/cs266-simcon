// this is the research team's rand walk with obstacle avoidance 
// FileName randWalk.cs
using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller
{

    class RandWalk : Algorithm
    {
        public RandWalk(Robot r)
            : base(r)
        {

        }

        public RandWalk(Robot r, int degInterval, float probTurn, float moveDistance)
            : base(r)
        {
            this.degInterval = degInterval; // interval at which to randomize angles. 
            // E.g. if 30, rand would only give 0, 30, 60, 90, ..., 360
            this.probTurn = probTurn;   // probability of turning when agent isn't moving
            this.moveDistance = moveDistance; // distance to move forward if moving
        }

        int degInterval = 15; // assume divisible by 360
        bool isFinished = false;
        double probTurn = 0.5;
        float moveDistance = 5;

        public override void Execute()
        {
            if (isFinished)
            {
                return;
            }
            RandomWalkSensorInput input = null;
            if (input.senseFood == true)
            {
                isFinished = true; // throw global termination (TODO: check if isFinished does this)
            }
            else if (input.isMoving == true && input.faceObstacle == false)
            {
                // do nothing TODO:  is this isFinished also?
                return;
            }
            else if (input.isMoving == true && input.faceObstacle == true)
            {
                robot.Stop();
            }
            else if (input.faceObstacle == false)
            { // and agent is not moving    
                double prob = new Random().Next(0, 100) / (double)100; // discretizing probabilities to within 100 b/c we are lazy
                if (prob > probTurn)
                { // shouldn't turn 
                    robot.MoveForward(moveDistance);
                }
                else
                { // turn in random direction at interval
                    int howManyIntervals = (int)(360 / degInterval);
                    float turnDegrees = (float)(new Random().Next(0, howManyIntervals)) * degInterval;
                    if (turnDegrees > 180)
                    {
                        turnDegrees = turnDegrees - 360;
                    }
                    robot.Turn(turnDegrees);
                }
            }
            else
            { // face obstacle and isn't moving
                int howManyIntervals = (int)(360 / degInterval);
                float turnDegrees = (float)(new Random().Next(0, howManyIntervals)) * degInterval;

                if (turnDegrees > 180)
                {
                    turnDegrees = turnDegrees - 360;
                }
                robot.Turn(turnDegrees);
            }
        }
    }
}



