using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    class AlgorithmFinishedException:System.Exception
    {
        public int robotid;
        
        public AlgorithmFinishedException(int robotId):base(){

            this.robotid = robotId;
        }
    }
}
