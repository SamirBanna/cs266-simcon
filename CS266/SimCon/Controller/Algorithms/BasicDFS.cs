using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller.Algorithms
{
    //This DFS algorithm assumes a grid cell structure. 
    //Each robot can move forward or to the left or to the right.
    //Robots needs to know which robot is in front (predecessor) and behind (sucessor).
    class BasicDFS : Algorithm
    {

        //each robot can be either a leader of a follower. In this basic DFS we assume that there is always exactly only one leader.
        bool isLeader = false;
        //true if leader should move randomly, false, if leader always should move in directions its facing too
        bool moveRandom = false;
        Robot r;

        public BasicDFS(Robot r)
            : base(r)
        {

        }

        // The moveDistance that is passed in should be the length of one grid cell edge
        //the very first robot created must be a leader
        public BasicDFS(Robot r, double moveDistance, bool isLeader, bool moveRandom)
            : base(r)
        {
            this.r = r;
            this.isLeader = isLeader;
            this.moveRandom = moveRandom;
            //no longer needed?
            //if (isLeader == true)
            //{
            //    r.isTail = true;
            //    r.active = true;
            //}

        }

        public void Execute(){

            if (((DFSSensor)this.robot.Sensors["DFSSensor"]).isActive())
            {

                //sensors all robots need
                float moveDistance = ((GridSensor)this.robot.Sensors["GridSensor"]).getCellLength();
                
                //LEADER behavior
                if (((DFSSensor)this.robot.Sensors["DFSSensor"]).isLeader()){

                    //assuming we get back the coordinates of the food source, to be used for statistics and evaluation
                    //double[][] foodCoordinates = ((GridSensor)this.robot.Sensors["GridSensor"]).getFoodCoordinatesInAdjCells;

                    //for now, just return bool
                    bool foundFood = ((GridSensor)this.robot.Sensors["GridSensor"]).detectFoodInAdjCells();
                    //returns array containing angles corresponding to possible moves: forwards, left, or right 
                    float[] possibleMoves = ((GridSensor)this.robot.Sensors["GridSensor"]).getPossibleMoves();
                   
                    //assume that only the leader can find food
                    if(foundFood == true)
                        Finished();
                        
                    //if leader has no moves, it must assign leadership to follower
                    if (possibleMoves.GetLength(0)==0){
                        r.Stop();
                        Robot succ = ((DFSSensor)this.robot.Sensors["DFSSensor"]).getSucc();
                        ((DFSSensor)succ.Sensors["DFSSensor"]).setLeadership();
                        //changes status active and leadership
                        ((DFSSensor)this.robot.Sensors["DFSSensor"]).deactivate();
                    }
                    else if(moveRandom){
                        //moveIndex indicated the index of the possibleMoves field assuming that possibleMoves includes the degrees the robot can possible move to
                        int moveIndex = new Random().Next(0, possibleMoves.GetLength(0));
                        r.Turn(possibleMoves[moveIndex]);
                        r.MoveForward(moveDistance);
                        }
                    //move determinstically
                    else{
                        bool forward = false;
                        bool left = false;
                        bool right = false;
                        for (int i = 0; i < possibleMoves.GetLength(0); i++){
                            //define degrees as constants later
                            if (possibleMoves[i] == 0)
                                forward = true;
                            else if (possibleMoves[i] == 270)
                                left = true;
                            else if (possibleMoves[i] == 90)
                                right = true;
                        }
                        if (!forward){
                            if (left){
                                r.Turn(270);
                            }
                            else if (right)
                                r.Turn(90);
                        }
                        r.MoveForward(moveDistance);
    					
                    }	
            }
			
            //FOLLOWER behavior
            else{
                //this function returns the direction the robot must turn to in order to do next move
                float angleForNextMove = ((DFSSensor)this.robot.Sensors["DFSSensor"]).getDirectionPred();
                r.Turn(angleForNextMove);
                r.MoveForward(moveDistance);
            }

            //If at door, create new robot (regardless of whether leader/follower)
            if (((DFSSensor)this.robot.Sensors["DFSSensor"]).isTail()){   	        
                //this method MUST assign predecessor (for new robot) and succesor (for this robot)
                //and reassign the chaintail to the new robot
                ((DFSSensor)this.robot.Sensors["DFSSensor"]).createNewRobot();               
            }          
        }
		
        }
    }
}

