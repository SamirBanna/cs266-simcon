using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    class ControlLoop
    {
        Dictionary<int, Robot> Robots;
        Dictionary<int, PhysObject> WorldObjects;


        public ControlLoop(List<Robot> robots,  List<PhysObject> worldObjects)
        {
            foreach (Robot robot in Robots)
            {
                Robots.Add(robot.id,robot);
            }

            foreach (PhysObject obj in worldObjects)
            {
                WorldObjects.Add(obj.id, obj);
            }
        }

        public void AcceptInput(List<Robot> robots, List<PhysObject> worldObjects)
        {
            foreach (PhysObject obj in worldObjects)
            {
                this.WorldObjects[obj.Id].Location = obj.Location;
                this.WorldObjects[obj.Id].Orientation = obj.Orientation;
            }

            // TODO
        }
    }
}
