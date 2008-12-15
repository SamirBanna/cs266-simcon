using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller.WorldInputInterfaces;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public class SimulatorInputInterface:WorldInputInterface
    {
        CS266.SimCon.Simulator.OurSimulator os;

        //Scale factor of actual robot model
        public int scale = 10;

        public SimulatorInputInterface(Simulator.OurSimulator os)
            : base()
        {
            //For conversion into controller units
            worldWidth = (os.Algo.dim[0] * 100) / this.scale;
            worldHeight = (os.Algo.dim[1] * 100) / this.scale;
            this.os = os;
        }

        public SimulatorInputInterface(Simulator.OurSimulator os, double worldWidth)
        {

        }

        public CS266.SimCon.Simulator.OurSimulator getOurSimulator()
        {
            return os;
        }

        public void setOurSimulator(Simulator.OurSimulator osNew)
        {
            this.os = osNew;
        }


        public override ControllerWorldState getWorldState()
        {
            Console.WriteLine("Making new world state");
            RobotList.Clear();
            PhysObjList.Clear();
            FoodList.Clear();
            setupInitialState();

            return new ControllerWorldState(RobotList, PhysObjList, FoodList, worldHeight, worldWidth);
            // fixed params to new convention: height, then width
        }


        public override void setupInitialState()
        {
            CS266.SimCon.Simulator.WorldState simWS = os.GetWorldState();
            int x = 0;
            int y = 0;
            int z = 0;

            foreach (CS266.SimCon.Simulator.ObjectState obj in simWS.objects)
            {
                //Positions are now scaled for the controller
                if (obj.type == "robot")
                {
                    Console.WriteLine("Making ROBOTS");
                    Console.WriteLine(obj.position.X);
                    Console.WriteLine(obj.position.Y);
                    RobotList.Add(new Robot(x++, obj.name, new Coordinates(((obj.position.X *100) / this.scale), ((obj.position.Y *100)/this.scale)), obj.degreesfromx, 7,7));
                    
                    // USE this line for DFSExperiment:
                    //RobotList.Add(new Robot(x++, obj.name, new Coordinates(DFSExperiment.doorX, DFSExperiment.doorY), obj.degreesfromx, 0, 0));

                }
                else if (obj.type == "obstacle" || obj.type == "Wall")
                {
                    PhysObjList.Add(new Obstacle(y++, "", new Coordinates(((obj.position.X * 100)/ this.scale), ((obj.position.Y * 100)/ this.scale)), obj.degreesfromx, 12, 12));
                }
                //THIS IS CORRECT -AJB
                else if (obj.type == "FoodUnit")
                {
                    FoodList.Add(new Food(z++, "", new Coordinates(((obj.position.X * 100) / this.scale), ((obj.position.Y * 100) / this.scale)), obj.degreesfromx, 0, 0));
                }
                
            }
            System.Threading.Thread.Sleep(5000);
        }
    }
}
