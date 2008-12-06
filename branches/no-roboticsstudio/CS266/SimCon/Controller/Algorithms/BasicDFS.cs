using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller
{
    //This DFS algorithm assumes a grid cell structure. 
    //Each robot can move forward or to the left or to the right.
    //Robots needs to know which robot is in front (predecessor) and behind (sucessor).
    class BasicDFS : Algorithm 
    {

        //each robot can be either a leader of a follower. In this basic DFS we assume that there is always exactly only one leader.
        public bool isLeader = false;
        public bool isActive;
        public bool isTail;
        public Robot pred;
        public Robot succ;
        //TODO!!!!
        public bool isTurnPhase;


        //true if leader should move randomly, false, if leader always should move in directions its facing too
        bool moveRandom = false;

        public BasicDFS(Robot r)
            : base(r)
        {

        }

        // The moveDistance that is passed in should be the length of one grid cell edge
        //the very first robot created must be a leader
        public BasicDFS(Robot r, bool isLeader, bool moveRandom)
            : base(r)
        {
          
            this.isLeader = isLeader;
            this.moveRandom = moveRandom;
            //no longer needed?
            //if (isLeader == true)
            //{
            //    r.isTail = true;
            //    r.active = true;
            //}

        }

         public override void Execute(){

            if (isActive)
            {

                //sensors all robots need
                float moveDistance = ((GridSensor)this.robot.Sensors["GridSensor"]).getCellLength();
                
                //LEADER behavior
                if (isLeader){

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
                        robot.Stop();
                 
                        ((BasicDFS)succ.CurrentAlgorithm).isLeader=true;
                        //changes status active and leadership
                        isActive=false;
                    }
                    else if(moveRandom){
                        //moveIndex indicated the index of the possibleMoves field assuming that possibleMoves includes the degrees the robot can possible move to
                        int moveIndex = new Random().Next(0, possibleMoves.GetLength(0));
                        robot.Turn(possibleMoves[moveIndex]);
                        robot.MoveForward(moveDistance);
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
                                robot.Turn(270);
                            }
                            else if (right)
                                robot.Turn(90);
                        }
                        robot.MoveForward(moveDistance);
    					
                    }	
            }
			
            //FOLLOWER behavior
            else{
                //this function returns the direction the robot must turn to in order to do next move
                float angleForNextMove = ((DFSSensor)this.robot.Sensors["DFSSensor"]).getDirectionPred();
                robot.Turn(angleForNextMove);
                robot.MoveForward(moveDistance);
            }

            //If at door, create new robot (regardless of whether leader/follower)
            if (isTail){   	        
                //this method MUST assign predecessor (for new robot) and succesor (for this robot)
                //and reassign the chaintail to the new robot

                // create new robot
                int id = ((DFSSensor)this.robot.Sensors["DFSSensor"]).nextID();
                //this is for the simulator
                robot.createNewRobot(id);

              
                Robot nextRobot = clone(id);
                
               
               
                ((DFSSensor)this.robot.Sensors["DFSSensor"]).addRobotToList(nextRobot);
                
                // copy my algorithm
                // update attributes

                //((DFSSensor)this.robot.Sensors["DFSSensor"]).createNewRobot();         
                throw new NewRobotException();
            }          
        }
		
        }
        //this Clone is only used by the DFS algorithm and therefore includes some specifics!!!!
        public Robot clone(int id)
         {
             
             //this is for adding the robot to the control loop
             Coordinates coord = ((DFSSensor)this.robot.Sensors["DFSSensor"]).getDoor();
             float orientation = 90;
             //size of robot is 5,5

           
             Robot newRobot = new Robot(id,"", coord, orientation,5,5);
             newRobot.CurrentAlgorithm = new BasicDFS(newRobot,false, moveRandom);
             newRobot.Sensors = new Dictionary<string, SensorInput>();
             foreach (String s in this.robot.Sensors.Keys){
                 SensorInput sens = SensorList.makeSensor(s);
                 sens.robot = newRobot;
                 newRobot.Sensors.Add(s, sens);
             }
             //assign attributes to the newly created robot
             ((BasicDFS)newRobot.CurrentAlgorithm).isTail = true;
             ((BasicDFS)newRobot.CurrentAlgorithm).pred = this.robot;
             ((BasicDFS)newRobot.CurrentAlgorithm).isLeader = false;
             ((BasicDFS)newRobot.CurrentAlgorithm).isActive = true;
             ((BasicDFS)newRobot.CurrentAlgorithm).succ = null;
             
             //change attributes of second to last robot (this robot)
             succ = newRobot;        
             isTail = false;
             
             return newRobot;

         }
    }
        
}

