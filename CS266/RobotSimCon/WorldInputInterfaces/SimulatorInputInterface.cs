using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Robotics.SimulationTutorial1;

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

                    Coordinates location = new Coordinates(ws.objects[i].position[0], ws.objects[i].position[1]);
                    float orientation = ws.objects[i].degreesfromx;
                    float width = 1;
                    float height = 1;
                    Robot r = new Robot(id, name, location, orientation, width, height);
                    rlist.Add(r);
                }
            }
            return rlist;
        }

        public override List<PhysObject> GetPhysObjects()
        {
            throw new NotImplementedException();
        }
    }
}
