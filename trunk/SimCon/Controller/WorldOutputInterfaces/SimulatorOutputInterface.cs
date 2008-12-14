using System;
using CS266.SimCon.Controller.Experiments;
using CS266.SimCon.Simulator;


namespace CS266.SimCon.Controller.WorldOutputInterfaces
{
    public class SimulatorOutputInterface : WorldOutputInterface
    {
        public static int counter = 0;
        CS266.SimCon.Simulator.OurSimulator os;

        public SimulatorOutputInterface(CS266.SimCon.Simulator.OurSimulator os)
            : base()
        {
            this.os = os;
        }

        public override void DoActions(PhysicalRobotAction action)
        {

            //Console.WriteLine("Sending action to simulator");
            Random rng = new Random();

            if (counter >= 100)
            {
                counter = 0;
                if (os.BatchMode != 1)
                {
                    System.Threading.Thread.Sleep(5000);
                    os.Finished();
                }
            }
            else
            {
                if (action.ActionType == PhysicalActionType.MoveForward)
                {
                    CS266.SimCon.Simulator.RobotActions rb = new RobotActions();
                    rb.Add(new RobotAction(action.RobotId.ToString(), 0, (float)action.ActionValue));
                    os.ExecuteActions(rb);
                }
                else if (action.ActionType == PhysicalActionType.MoveBackward)
                {
                    CS266.SimCon.Simulator.RobotActions rb = new RobotActions();
                    rb.Add(new RobotAction(action.RobotId.ToString(), 0, (float)-action.ActionValue));
                    os.ExecuteActions(rb);
                }
                else if (action.ActionType == PhysicalActionType.Turn)
                {
                    CS266.SimCon.Simulator.RobotActions rb = new RobotActions();
                    rb.Add(new RobotAction(action.RobotId.ToString(), (float)action.ActionValue, 0));
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
}
