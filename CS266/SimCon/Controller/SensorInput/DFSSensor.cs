using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class DFSSensor : SensorInput
    {
        int doorX; // in grid coordinates
        int doorY; // in grid coordinates

        // assume grid coordinates are passed in, e.g., (0,0)
        public DFSSensor(int doorX, int doorY)
        {
            this.doorX = doorX;
            this.doorY = doorY;
        }

   
        public float getDirectionPred(){
            //TODO
            return 0;
        }

        // Returns the new id of a robot to be created
        public int nextID()
        {
            int maxId = -1;
            foreach (Robot robot in worldState.robots)
            {
                if (robot.Id > maxId)
                {
                    maxId = robot.Id;
                }
            }
            return maxId + 1;
        }

  
        public void addRobotToList(Robot newRobot){
            // add robot to world state
            worldState.robots.Add(newRobot);
        }

  
        public Coordinates getDoor()
        {
            return ControlLoop.robotGrid.getCenterOfCell(doorX, doorY);
        }


        public override void UpdateSensor()
        {
          //TODO
        }
    }
}
