using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public class SimulatorInputInterface:WorldInputInterface
    {
        Robotics.SimulationTutorial1.SimulationTutorial1.OurSimulator os;

        public SimulatorInputInterface(Robotics.SimulationTutorial1.SimulationTutorial1.OurSimulator os)
            : base()
        {
            this.os = os;
        }
        
        public override List<CS266.SimCon.Controller.Robot> GetRobots()
        {
            return RobotList;
        }

        public override List<CS266.SimCon.Controller.PhysObject> GetPhysObjects()
        {
            return PhysObjList;
        }
        public override ControllerWorldState getNewWorldState()
        {
            Console.WriteLine("Making new world state");
            RobotList.Clear();
            PhysObjList.Clear();

            setupInitialState();

            return new ControllerWorldState(RobotList, PhysObjList);
        }

        public override CS266.SimCon.Controller.ControllerWorldState getWorldState()
        {

            RobotList.Clear();
            PhysObjList.Clear();
            
            //this.os.Finished();
            
            setupInitialState();
            return new ControllerWorldState(RobotList, PhysObjList);
        }

        public override void setupInitialState()
        {
            Robotics.SimulationTutorial1.SimulationTutorial1.WorldState simWS = os.GetWorldState();
            int x = 0;
            int y = 0;

            foreach (Robotics.SimulationTutorial1.SimulationTutorial1.ObjectState obj in simWS.objects)
            {
                int i = 0;
                if (obj.type == "robot")
                {
                    Console.WriteLine("Making ROBOTS");
                    Console.WriteLine(obj.position.X);
                    Console.WriteLine(obj.position.Y);
                    //RobotList.Add(new Robot(x++, obj.name, new Coordinates(obj.position.X, obj.position.Y), obj.degreesfromx, 0,0));
                    if (i < 1)
                    {
                        RobotList.Add(new Robot(x++, obj.name, new Coordinates(5, 5), obj.degreesfromx, 0, 0));
                        i++;
                    }
                    else
                    {
                        RobotList.Add(new Robot(x++, obj.name, new Coordinates(4, 5), obj.degreesfromx, 0, 0));
                    }

                }
                else if (obj.type == "obstacle" || obj.type == "Wall")
                {
                    PhysObjList.Add(new Obstacle(y++, "", new Coordinates(obj.position.X, obj.position.Y), obj.degreesfromx, 0, 0));
                }
                
            }
            System.Threading.Thread.Sleep(5000);
        }
    }
}
