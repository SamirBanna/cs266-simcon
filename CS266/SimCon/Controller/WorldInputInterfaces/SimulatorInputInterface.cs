using System;
using System.Collections.Generic;

using System.Text;

using CS266.SimCon.RoboticsClasses;


namespace CS266.SimCon.Controller.WorldInputInterfaces
{
    public class SimulatorInputInterface : WorldInputInterface
    {
        private WorldState ws;
        public SimulatorInputInterface(WorldState ws)
        {
            this.ws = ws;

        }
        public override List<Robot> GetRobots()
        {
            List<Robot> rlist = new List<Robot>();
            for (int i = 0; i < ws.objects.Count; i++)
            {
                if (ws.objects[i].type == "robot")
                {
                    /* type 2 = robot type */
                    int id = 2;
                    /* id for robot */
                    String name = ws.objects[i].name;

                    Coordinates location = new Coordinates(ws.objects[i].position.X, ws.objects[i].position.Y);  
                    float orientation = ws.objects[i].degreesfromx;
                    float width = 1;
                    float height = 1;
                    Robot r = new Robot(id, name, location, orientation, width, height);
                    rlist.Add(r);
                }
            }
            return rlist;
        }

        /* GetPhysObjects() uses the WorldState from the simulator and 
         * returns a list of non-robot physical objects in the controller */
        public override List<PhysObject> GetPhysObjects()
        {
            List<PhysObject> olist = new List<PhysObject>();
            for (int i = 0; i < ws.objects.Count; i++)
            {
                if (ws.objects[i].type != "robot")
                {
                    PhysObject o;
                    /* id */
                    String name = ws.objects[i].name;

                    Coordinates location = new Coordinates(ws.objects[i].position.X, ws.objects[i].position.Y);  
                    float orientation = ws.objects[i].degreesfromx;
                    if (ws.objects[i].type == "obstacle")
                    {
                        /* type 2 = robot type */
                        int id = 1;
                        float width = 5;
                        float height = 5;
                        o = new Obstacle(id, name, location, orientation, width, height);
                    }
                    else if (ws.objects[i].type == "food")
                    {
                        int id = 3;
                        // Find out how big food is
                        float width = 5;
                        float height = 5;
                        o = new Food(id, name, location, orientation, width, height);
                    }
                    else
                    {
                        // Unknown
                        int id = 0;
                        float width = 1;
                        float height = 1;
                        // Probably an obstacle
                        o = new Obstacle(id, name, location, orientation, width, height);
                    }
                    
                    olist.Add(o);
                }
            }
            return olist;
        }
    }
}
