// this is the research team's rand walk with obstacle avoidance 
// FileName randWalk.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS266.SimCon.Controller.Algorithms
{

  class RandWalk : Algorithm 
    {
        public RandWalk(Robot r) : base(r)
        {

        }

      public RandWalk(Robot r, int degInterval, double probTurn, double moveDistance) : base(r)
        {
	  this.degInterval = degInterval; // interval at which to randomize angles. 
	                                  // E.g. if 30, rand would only give 0, 30, 60, 90, ..., 360
	  this.probTurn = probTurn;   // probability of turning when agent isn't moving
	  this.moveDistance = moveDistance; // distance to move forward if moving
        }
      
      int degInterval = 15; // assume divisible by 360
      bool isFinished = false;
      double probTurn = 0.5;
      double moveDistance = 5;
      
      public void Execute()
      {
	if (isFinished)
	  {
	    return;
	  }
	
	RandomWalkSensorInput input = (RandomWalkSensorInput)Robot.CurrentInput;

	if(input.senseFood == true){
	  isFinished = true; // throw global termination (TODO: check if isFinished does this)
	}else if(input.isMoving == true && input.faceObstacle == false){
	  // do nothing TODO:  is this isFinished also?
	  return;
	}else if(input.isMoving == true && input.faceObstacle == true){
	  Robot.Stop();
	}else if(input.faceObstacle == false){ // and agent is not moving     
	  double prob = new Random().Next(0, 100) / (double)100; // discretizing probabilities to within 100 b/c we are lazy 
	  
	  if(prob > probTurn){ // shouldn't turn 
	    Robot.MoveForward(moveDistance);
	  }else{ // turn in random direction at interval
	    int howManyIntervals = (int)(360 / degInterval);
	    float turnDegrees = (float)(new Random().Next(0, howManyIntervals)) * degInterval;   
	    if(turnDegrees > 180){ 
	      turnDegrees = turnDegrees - 360;  
	    }
	    Robot.Turn(turnDegrees);
	  }
	}else{ // face obstacle and isn't moving
	  int howManyIntervals = (int)(360 / degInterval);
	  float turnDegrees = (float)(new Random().Next(0, howManyIntervals)) * degInterval;   
	  if(turnDegrees > 180){ 
	    turnDegrees = turnDegrees - 360;  
	  }
	  Robot.Turn(turnDegrees);
	}
      }
    }
}



