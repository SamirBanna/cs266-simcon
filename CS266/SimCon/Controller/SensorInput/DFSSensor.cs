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

        public int nextID()
        {
            return 0;
            // add robot to world state
        }

  
        public void addRobotToList(Robot newRobot){
            // add robot to world state
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
