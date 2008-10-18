﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    class ControlLoop
    {
        Dictionary<int, Robot> Robots;
        Dictionary<int, PhysObject> WorldObjects;
        WorldInputInterface Wii;

        public static Queue<PhysicalRobotAction> ActionQueue = new Queue<PhysicalRobotAction>();


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

        /// <summary>
        /// Runs the actual control loop
        /// </summary>
        public void RunLoop()
        {
            this.GetInput();
        }

        private void GetInput()
        {
            PhysObject worldObjects = Wii.GetPhysObjects();
            Robot robots = Wii.GetRobots();

            // New positions of objects get updated immediately
            foreach (PhysObject obj in worldObjects)
            {
                this.WorldObjects[obj.Id].Location = obj.Location;
                this.WorldObjects[obj.Id].Orientation = obj.Orientation;
            }

            foreach (Robot r in robots)
            {
                this.Robots[r.Id].Location = r.Location;
                this.Robots[r.Id].Orientation = r.Orientation;
            }

            
        }
    }
}