using System;
using CS266.SimCon.Controller.Experiments;
using CS266.SimCon.Simulator;


namespace CS266.SimCon.Controller.WorldOutputInterfaces
{
    public class SimulatorOutputInterface : WorldOutputInterface
    {
        public static int counter = 0;
        CS266.SimCon.Simulator.OurSimulator os;
        public int scale = 10;

        public SimulatorOutputInterface(CS266.SimCon.Simulator.OurSimulator os)
            : base()
        {
            this.os = os;
        }

        public override void DoActions(PhysicalRobotAction action)
        {

            //Console.WriteLine("Sending action to simulator");
            Random rng = new Random();

          
            if (action.ActionType == PhysicalActionType.MoveForward)
            {
                //Accounts for scaling differences in the simulator and the controller
                action.ActionValue = (action.ActionValue / 1000) * this.scale;
                Console.WriteLine("=====================MOVING FORWARD THIS DISTANCE: " + action.ActionValue);
                CS266.SimCon.Simulator.RobotActions rb = new RobotActions();
                rb.Add(new RobotAction(action.RobotId.ToString(), 0, (float)action.ActionValue));
                os.ExecuteActions(rb);
            }
            else if (action.ActionType == PhysicalActionType.MoveBackward)
            {
                //Accounts for scaling differences in the simulator and controller
                action.ActionValue = (action.ActionValue / 1000) * this.scale;
                CS266.SimCon.Simulator.RobotActions rb = new RobotActions();
                rb.Add(new RobotAction(action.RobotId.ToString(), 0, (float)-action.ActionValue));
                os.ExecuteActions(rb);
            }
            else if (action.ActionType == PhysicalActionType.Turn)
            {
                Console.WriteLine("=====================TURNING THIS ANGLE: " + action.ActionValue);
                
                if (action.ActionValue < 0)
                {
                    action.ActionValue = 360 + action.ActionValue;
                }

                CS266.SimCon.Simulator.RobotActions rb = new RobotActions();
                rb.Add(new RobotAction(action.RobotId.ToString(), (-1)*(float)action.ActionValue, 0));
                // multiply by -1 to undo what Robot.cs does!
                os.ExecuteActions(rb);
            }
            else if (action.ActionType == PhysicalActionType.CreateRobot)
            {
                os.AddNewRobot(new ObjectState("N1", "robot", new float[3] { DFSExperiment.doorX, DFSExperiment.doorY, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 }));
            }
            else if (action.ActionType == PhysicalActionType.SetSpeed) { }
            //System.Threading.Thread.Sleep(500);
            
        }
    }
}
