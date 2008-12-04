using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class DFSSensor : SensorInput
    {

        bool isActive;
        bool isLeader;
        bool isTail;

        public Robot getSucc(){
            return null;
        }

        public float getDirectionPred(){
            return 0;
        }

        public void createNewRobot(){
        }

        public bool isLeader(){
            return false;
        }

        public bool isActive(){
            return false;
        }

        public bool isTail(){
            return false;
        }

        public void setLeadership(){
        }
          
        public void deactivate(){
        }

    
    }
}
