using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class DFSSensor : SensorInput
    {

       
        int doorX;
        int doorY;

        public DFSSensor(int doorX, int doorY)
        {
               
        }

   
        public float getDirectionPred(){
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
            // call grid method to turn (doorX,doorY) -> floats
            return null;
        }


        public override void UpdateSensor()
        {
          //TODO
        }
    }
}
