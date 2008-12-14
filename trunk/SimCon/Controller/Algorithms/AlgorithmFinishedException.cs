using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller.Algorithms
{
    /// <summary>
    /// Exception sometimes thrown to indicate the end of an algorithm's execution.
    /// Yes, this is atrocious programming style.
    /// </summary>
    class AlgorithmFinishedException : Exception
    {
        public int robotid;
        
        public AlgorithmFinishedException(int robotId):base(){

            this.robotid = robotId;
        }
    }
}
