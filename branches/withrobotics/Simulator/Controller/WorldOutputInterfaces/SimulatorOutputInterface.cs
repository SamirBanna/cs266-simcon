using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller.WorldOutputInterfaces;
using Robotics.SimulationTutorial1;

namespace CS266.SimCon.Controller.WorldOutputInterfaces
{
    public class SimulatorOutputInterface:WorldOutputInterface
    {
        public static int counter = 0;
        Robotics.SimulationTutorial1.SimulationTutorial1.OurSimulator os;

        public SimulatorOutputInterface(Robotics.SimulationTutorial1.SimulationTutorial1.OurSimulator os)
            : base()
        {
            this.os = os;
        }

        public override void DoActions(PhysicalRobotAction action)
        {

            Console.WriteLine("SEnding action to simulator");
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
                counter++;

                if (action.ActionType == PhysicalActionType.MoveForward)
                {
                    Robotics.SimulationTutorial1.SimulationTutorial1.RobotActions rb = new SimulationTutorial1.RobotActions();
                    rb.Add(new SimulationTutorial1.RobotAction(action.RobotId.ToString(), 0, action.ActionValue));
                    os.ExecuteActions(rb);
                }
                else if (action.ActionType == PhysicalActionType.MoveBackward)
                {
                    Robotics.SimulationTutorial1.SimulationTutorial1.RobotActions rb = new SimulationTutorial1.RobotActions();
                    rb.Add(new SimulationTutorial1.RobotAction(action.RobotId.ToString(), 0, -action.ActionValue));
                    os.ExecuteActions(rb);
                }
                else if (action.ActionType == PhysicalActionType.Turn)
                {
                    Robotics.SimulationTutorial1.SimulationTutorial1.RobotActions rb = new SimulationTutorial1.RobotActions();
                    rb.Add(new SimulationTutorial1.RobotAction(action.RobotId.ToString(), action.ActionValue, 0));
                    os.ExecuteActions(rb);
                }
                else if (action.ActionType == PhysicalActionType.SetSpeed) { }
                //System.Threading.Thread.Sleep(500);
            }
        }
    }
}
