using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller;
using CS266.SimCon.Controller.InputSensors;

namespace CS266.SimCon.Controller.Algorithms
{
    /// <summary>
    /// This DFS algorithm assumes a grid cell structure.
    /// Each robot can move forward or to the left or to the right.
    /// Robots needs to know which robot is in front (predecessor) and behind (sucessor).
    /// </summary>
    class BasicDFS : Algorithm 
    {

        /// <summary>
        /// Each robot can be either a leader of a follower. In this basic DFS we assume that there is always exactly only one leader.
        /// </summary>
        public bool isLeader = false;

        public bool isActive;
        public bool isTail;
        public Robot pred;
        public Robot succ;      
        public bool isTurnPhase;

        /// <summary>
        /// True if leader should move randomly, false, if leader always should move in directions its facing too
        /// </summary>
        private bool moveRandom;
        private Random rand;

        //public BasicDFS(Robot r)
        //    : base(r)
        //{

        //}

        /// <summary>
        /// The moveDistance that is passed in should be the length of one grid cell edge
        /// the very first robot created must be a leader
        /// </summary>
        /// <param name="r"></param>
        /// <param name="isLeader"></param>
        /// <param name="moveRandom"></param>
        public BasicDFS(Robot r, bool isLeader, bool moveRandom)
            : base(r)
        {         
            this.isLeader = isLeader;
            this.moveRandom = moveRandom;
            this.isActive = true;
            this.isTurnPhase = true;
            this.isTail = true;
            rand = new Random();
        }

         public override void Execute(){
            ((BoundarySensor)this.robot.Sensors["BoundarySensor"]).SetSensingDistance(1.75);

            Console.WriteLine("****************DFS Output*********************");
            Console.WriteLine("x robot    = " + this.robot.Location.X);
            Console.WriteLine("y robot    = " + this.robot.Location.Y);
            Console.WriteLine("orientation robot = " + robot.Orientation);
            Console.WriteLine("robot " + robot.Id + ": turnPhase = " + isTurnPhase);

            if (isActive)
            {
                ControlLoop.robotGrid.GridUpdate(this.robot);
              
                //sensors all robots need
                double moveDistance = ((GridSensor)this.robot.Sensors["GridSensor"]).getCellLength();
                Console.WriteLine("move distance = " + moveDistance);
                
                
                //LEADER behavior
                if (isLeader){

                    //assuming we get back the coordinates of the food source, to be used for statistics and evaluation
                    //double[][] foodCoordinates = ((GridSensor)this.robot.Sensors["GridSensor"]).getFoodCoordinatesInAdjCells;

                    //for now, just return bool
                    bool foundFood = ((GridSensor)this.robot.Sensors["GridSensor"]).detectFoodInAdjCells();
                    //returns array containing angles corresponding to possible moves: forwards, left, or right 
                    List<double> possibleMoves = ((GridSensor)this.robot.Sensors["GridSensor"]).getPossibleMoves();


                    //assume that only the leader can find food
                    if (foundFood == true)
                    {
                        Console.WriteLine("*******DFS found food!******");
                        Finished();
                    }

                    //if leader has no moves, it must assign leadership to successor
                    if (possibleMoves.Count == 0)
                    {
                        if (isTurnPhase) // also, must be in Turn phase
                        {
                            robot.Stop();
                            if (succ != null)
                            {
                                ((BasicDFS)succ.CurrentAlgorithm).isLeader = true;
                                //changes status active and leadership
                                isActive = false;
                            }
                            else
                            {
                                Finished();
                            }
                        }
                    }
                    
                    else if (moveRandom)
                    {
                        if (isTurnPhase)
                        {
                            //moveIndex indicated the index of the possibleMoves field assuming that possibleMoves includes the degrees the robot can possible move to
                            Console.WriteLine("****In MoveRandom: Num possible moves: " + possibleMoves.Count);
                            int moveIndex = rand.Next(0, possibleMoves.Count);
                            Console.WriteLine("****In MoveRandom: random index: " + moveIndex);
                            robot.Turn(possibleMoves[moveIndex]);
                        }
                        else
                        {
                            robot.MoveForward(moveDistance);
                        }
                    }
                    //move determinstically
                    else
                    {
                        if (isTurnPhase)
                        {
                            bool forward = false;
                            bool left = false;
                            bool right = false;
                            for (int i = 0; i < possibleMoves.Count; i++)
                            {
                                Console.WriteLine("possible Move[i]: " + possibleMoves[i]);
                                //possibleMoves returns array of turnDegrees
                                //orientation is imprecise because of physical simulation of robots 
                                //therefore test for whole range of angle
                                if (possibleMoves[i] >= -45 && possibleMoves[i] <= 45)
                                    forward = true;
                                else if (possibleMoves[i] >= 45 && possibleMoves[i] <= 135)
                                    left = true;
                                else if (possibleMoves[i] >= -135 && possibleMoves[i] <= -45)
                                    right = true;
                            }
                            Console.WriteLine("===================================Forward =: " + forward);
                            ((BoundarySensor)this.robot.Sensors["BoundarySensor"]).UpdateSensor();
                            bool senseBoundary = ((BoundarySensor)this.robot.Sensors["BoundarySensor"]).detectObject;
                            double senseDist = ((BoundarySensor)this.robot.Sensors["BoundarySensor"]).objectSensingDist;
                            Console.WriteLine("===================================BOUNDARY SENSOR senseDistance = " + senseDist);
                            Console.WriteLine("===================================BOUNDARY SENSOR detectObject = " + senseBoundary);
                            
                            if (senseBoundary) forward = false;
                            Console.WriteLine("robot id: " + this.robot.Id);
                            Console.WriteLine("Num possible moves: " + possibleMoves.Count);
                            Console.WriteLine("forward is equal to: " + forward);
                            Console.WriteLine("left is equal to: " + left);
                            Console.WriteLine("right is equal to: " + right);

                            if (!forward)
                            {
                                if (left)
                                {
                                    robot.Turn(90);
                                }
                                else if (right)
                                    robot.Turn(-90);
                            }
                        }
                        else
                        {
                            robot.MoveForward(moveDistance);
                        }

                    }	
            }
			
            //FOLLOWER behavior
            else
            {
                if (isTurnPhase)
                {
                    //this function returns the turnDegrees the robot must turn to in order to do next move
                    double angleForNextMove = ((DFSSensor)this.robot.Sensors["DFSSensor"]).getDirectionPred();
                    robot.Turn(angleForNextMove);
                    Console.WriteLine("DFS: Robot " + robot.Id + " turning " + angleForNextMove);
                }
                else
                {
                    //if (!((BoundarySensor)this.robot.Sensors["BoundarySensor"]).detectObject)
                        robot.MoveForward(moveDistance);
                }
            }

            //If at door, create new robot (regardless of whether leader/follower)
            if (isTail && !isTurnPhase){   
                //this method MUST assign predecessor (for new robot) and succesor (for this robot)
                //and reassign the chaintail to the new robot

                // create new robot
                int id = ((DFSSensor)this.robot.Sensors["DFSSensor"]).nextID();
                //this is for the simulator
                robot.createNewRobot(id);
                Robot nextRobot = clone(id);
                ((DFSSensor)this.robot.Sensors["DFSSensor"]).addRobotToList(nextRobot);

                //this exception tells the simulator not to overwrite previous action with "createNewRobot"  
                isTurnPhase = !isTurnPhase;
                throw new NewRobotException(nextRobot);               
            }          
        }
            // switch Turn phase <-> Move phase
            isTurnPhase = !isTurnPhase;
        }



        /// <summary>
        /// This <code>clone</code> method is only used by the DFS algorithm and therefore includes some specifics!!!!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Robot clone(int id)
        {
             
             //this is for adding the robot to the control loop
             Coordinates coord = ((DFSSensor)this.robot.Sensors["DFSSensor"]).getDoor();
             double orientation = 90;
             //size of robot is 5,5

           
             Robot newRobot = new Robot(id,"", coord, orientation,5,5);
             newRobot.CurrentAlgorithm = new BasicDFS(newRobot,false, moveRandom);
             newRobot.Sensors = new Dictionary<string, InputSensor>();
             foreach (String s in this.robot.Sensors.Keys){
                 InputSensor sens = SensorList.makeSensor(s);
                 sens.robot = newRobot;
                 newRobot.Sensors.Add(s, sens);
             }
             //assign attributes to the newly created robot
             ((BasicDFS)newRobot.CurrentAlgorithm).isTail = true;
             ((BasicDFS)newRobot.CurrentAlgorithm).pred = this.robot;
             ((BasicDFS)newRobot.CurrentAlgorithm).succ = null;
             
             //change attributes of second to last robot (this robot)
             succ = newRobot;        
             isTail = false;
             
             return newRobot;

         }
    }
        
}

