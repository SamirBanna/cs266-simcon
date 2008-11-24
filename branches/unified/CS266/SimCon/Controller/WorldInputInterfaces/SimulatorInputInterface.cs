using System;
using System.Collections.Generic;

using System.Text;

using CS266.SimCon.RoboticsClasses;


namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public class SimulatorInputInterface : WorldInputInterface
    {
        private WorldState ws;
        public SimulatorInputInterface()
        {
        }
        public override List<Robot> GetRobots()
        {
            this.ws = CS266.SimCon.Simulator.getWorldState();
            // Get all robots from world state and convert them into Robot format
            List<Robot> rlist = new List<Robot>();
            for (int i = 0; i < ws.objects.Count; i++)
            {
                if (ws.objects[i].type == "robot")
                {
                    /* type 2 = robot type */
                    int id = Int16.Parse(ws.objects[i].name);

                    Coordinates location = new Coordinates(ws.objects[i].position.X, ws.objects[i].position.Y);  
                    float orientation = ws.objects[i].degreesfromx;
                    float width = 1;
                    float height = 1;
                    Robot r = new Robot(id, ObjectType.Robot, location, orientation, width, height);
                    rlist.Add(r);
                }
            }
            return rlist;
        }

        /* GetPhysObjects() uses the WorldState from the simulator and 
         * returns a list of non-robot physical objects in the controller */
        public override List<PhysObject> GetPhysObjects()
        {
            // Update the world state. This may have to be moved out
            this.ws = CS266.SimCon.Simulator.getWorldState();

            // Get all non-robot objects and convert them into PhysObject format
            List<PhysObject> olist = new List<PhysObject>();
            for (int i = 0; i < ws.objects.Count; i++)
            {
                if (ws.objects[i].type != "robot")
                {
                    PhysObject o;
                    /* id */
                    int id = Int16.Parse(ws.objects[i].name);

                    Coordinates location = new Coordinates(ws.objects[i].position.X, ws.objects[i].position.Y);  
                    float orientation = ws.objects[i].degreesfromx;
                    if (ws.objects[i].type == "obstacle")
                    {
                        /* type 2 = robot type */
                        float width = 5;
                        float height = 5;
                        o = new Obstacle(id, ObjectType.Obstacle, location, orientation, width, height);
                    }
                    else if (ws.objects[i].type == "food")
                    {
                        // Find out how big food is
                        float width = 5;
                        float height = 5;
                        o = new Food(id, ObjectType.Food, location, orientation, width, height);
                    }
                    else
                    {
                        // Unknown
                        float width = 1;
                        float height = 1;
                        // Probably an obstacle
                        o = new Obstacle(id, ObjectType.Unknown, location, orientation, width, height);
                    }
                    
                    olist.Add(o);
                }
            }
            return olist;
        }
    }
}
