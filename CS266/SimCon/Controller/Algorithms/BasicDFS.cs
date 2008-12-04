//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace CS266.SimCon.Controller.Algorithms
//{
//    //This algorithm assumes a grid cell structure. Each robot can move forward or to the left or to the right.
//    //Robots needs to know which robot is in front (predecessor) and behind (sucessor).
//    class BasicDFS : Algorithm
//    {

//        //each robot can be either a leader of a follower. In this basic DFS we assume that there is always exactly only one leader.
//        bool isLeader = false;
//        //true if leader should move randomly, false, if leader always should move in directions its facing too
//        bool moveRandom = false;


      
//        Robot r;

//        public BasicDFS(Robot r)
//            : base(r)
//        {

//        }

//        // The moveDistance that is passed in should be the length of one grid cell edge
//        //the very first robot created must be a leader
//        public BasicDFS(Robot r, double moveDistance, bool isLeader, bool moveRandom)
//            : base(r)
//        {
//            this.r = r;
//            this.isLeader = isLeader;
//            this.moveRandom = moveRandom;
//            if (isLeader == true)
//            {
//                r.isChainTrail = true;
//                r.active = true;
//            }

//        }


        
//        public void Execute(){

//            if (r.active){

//                //sensors all robots need
//                float moveDistance = (GridSensor).this.robot.Sensors["GridSensor"].getCellLength();			
            	
//                if (leader == true){

//                //leader sensor
//                //assuming we get back the coordinates of the food source, to be used for statistics and evaluation
//                //double[][] foodCoordinates = ((GridSensor)this.robot.Sensors["GridSensor"]).getFoodCoordinatesInAdjCells;
//                bool foundFood = ((GridSensor)this.robot.Sensors["GridSensor"]).detectFoodInAdjCells;
//                //Robot needs to sense whether he can move forwads, backwards, left or right (90 degree angels) input.getPossibleMoves(moveDistance);
//                //the best would be if that functions return 0 for move forward possible, 90 for move right possible, 180 for move backward possible and 270 for move left possibe
//                float[] possibleMoves = (GridSensor).this.robot.Sensors["GridSensor"].getPossibleMoves();
               

//                //assume that only the leader can find food
//                if(foundFood == true)
//                    Finished();
                
//                //if robot was inactivated in this round, it must assign leadership to follower
//                if (possibleMoves.GetLength(0)==0){
//                    r.Stop();
//                    Robot succ = ((DFSSensor)this.robot.Sensors["DFSSensor"]).getSucc();
//                    succ.Sensors["DFSSensor"].setLeadership();
//                    //changes status active and leadership
//                    this.robot.Sensors["DFSSensor"].deactivate();
//                }
//                else if(moveRandom){
//                    //prob indicated the index of the possibleMoves field assuming that possibleMoves includes the degrees the robot can possible move to
//                    int moveIndex = new Random().Next(0, possibleMoves.GetLength(0));
//                    r.Turn(possibleMoves[moveIndex]);
//                    r.MoveForward(moveDistance);
//                }
//                //move determinstic
//                else{
//                    bool forward = false;
//                    bool left = false;
//                    bool right = false;
//                    for (int i = 0; i < possibleMoves.GetLength(0); i++){
//                        //define degrees as constants later
//                        if (possibleMoves[i] == 0)
//                            forward = true;
//                        else if (possibleMoves[i] == 270)
//                            left = true;
//                        else if (possibleMoves[i] == 90)
//                            right = true;
//                    }
//                    if (!forward){
//                        if (left){
//                            r.Turn(270);
//                        }
//                        else if (right)
//                            r.Turn(90);
//                    }
//                    r.MoveForward(moveDistance);
					
//                }	
			

//            }//end leader loop
			
//            //start follower loop
//            else{
//                //where did my predecessor go to?
//                //assume that this function already returns the direction the robot must turn to in order to do next move
//                float angleForNextMove = (DFSSensor).this.robot.Sensors["DFSSensor"].getPrevPositionPred();
//                r.Turn(angleForNextMove);
//                r.MoveForward(moveDistance);
//            }
//            if ((DFSSensor).this.robot.Sensors["DFSSensor"].isTail()){
   	        
//                //this method MUST assign predecessor (for new robot) and succesor (for this robot)
//                //and reassign the chaintail to the new robot
//                (DFSSensor).this.robot.Sensors["DFSSensor"].createNewRobot();
               
//            }
           
//        }
		
//        }
//    }
//}

////isLeader


////Robot needs to know the predecessor and the successor

////Robot needs to sense whether he can move forwads, backards, left or right (90 degree angels) input.getPossibleMoves(moveDistance);
////the best would be if that functions return 0 for move forward possible, 90 for move right possible, 180 for move backward possible and 270 for move left possibe

////we need attribte "active" in robot. If a robot is stoppped once in the algorithm (it's a leader and can't move anywhere else), it is deactivated for the end of the run.

////the sensor must be able to return the direction the predecessor robot moved to: input.getPrevPosition(Robot pred); 



