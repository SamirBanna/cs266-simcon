using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    class GridSensor : ProximitySensor
    {
        //returns length of grid cell edge
        public float getCellLength()
        {
            return 0;
        }

        //returns true if food in any adjacent cells
        public bool detectFoodInAdjCells()
        {
            return false;
        }

        //returns array containing angles corresponding to possible moves: forwards, left, or right                
        public float[] getPossibleMoves()
        {
            return null;
        }
    }
}
