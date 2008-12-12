using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public class SimulatorInputInterface:WorldInputInterface
    {
        CS266.SimCon.Simulator.OurSimulator os;
        public double worldWidth = 200;
        public double worldHeight = 90;

        public SimulatorInputInterface(CS266.SimCon.Simulator.OurSimulator os)
            : base()
        {
            this.os = os;
        }

        public CS266.SimCon.Simulator.OurSimulator getOurSimulator()
        {
            return os;
        }

        public void setOurSimulator(CS266.SimCon.Simulator.OurSimulator osNew)
        {
            this.os = osNew;
        }

        public override List<CS266.SimCon.Controller.Robot> GetRobots()
        {
            return RobotList;
        }

        public override List<CS266.SimCon.Controller.PhysObject> GetPhysObjects()
        {
            return PhysObjList;
        }

        public override List<CS266.SimCon.Controller.Food> GetFood()
        {
            return FoodList;
        }

        public override ControllerWorldState getNewWorldState()
        {
            Console.WriteLine("Making new world state");
            RobotList.Clear();
            PhysObjList.Clear();
            FoodList.Clear();
            setupInitialState();

            return new ControllerWorldState(RobotList, PhysObjList, FoodList, worldWidth, worldHeight);
        }

        public override CS266.SimCon.Controller.ControllerWorldState getWorldState()
        {

            RobotList.Clear();
            PhysObjList.Clear();
            FoodList.Clear();
            
            //this.os.Finished();
            
            setupInitialState();
            return new ControllerWorldState(RobotList, PhysObjList, FoodList, worldWidth, worldHeight);
        }

        public override void setupInitialState()
        {
            CS266.SimCon.Simulator.WorldState simWS = os.GetWorldState();
            int x = 0;
            int y = 0;

            foreach (CS266.SimCon.Simulator.ObjectState obj in simWS.objects)
            {
                if (obj.type == "robot")
                {
                    Console.WriteLine("Making ROBOTS");
                    Console.WriteLine(obj.position.X);
                    Console.WriteLine(obj.position.Y);
                    RobotList.Add(new Robot(x++, obj.name, new Coordinates(obj.position.X, obj.position.Y), obj.degreesfromx, 7,7));
                    
                    // USE this line for DFSExperiment:
                    //RobotList.Add(new Robot(x++, obj.name, new Coordinates(DFSExperiment.doorX, DFSExperiment.doorY), obj.degreesfromx, 0, 0));

                }
                else if (obj.type == "food")
                {
                    FoodList.Add(new Food(y++, "", new Coordinates(obj.position.X, obj.position.Y), obj.degreesfromx, 0, 0));
                }
                else if (obj.type == "obstacle" || obj.type == "Wall")
                {
                    PhysObjList.Add(new Obstacle(y++, "", new Coordinates(obj.position.X, obj.position.Y), obj.degreesfromx, 12, 12));
                }
                
            }
            System.Threading.Thread.Sleep(5000);
        }
    }
}
