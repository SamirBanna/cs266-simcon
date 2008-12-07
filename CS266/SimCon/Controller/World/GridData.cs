using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    // What occupies the grid. Contains information that can be added or taken away
    public class GridData
    {
        public bool isOccupied;
        public int numTimesVisited;

        // If no information given, assume initialized at beginning
        public GridData()
        {
            isOccupied = false;
            numTimesVisited = 0;
        }
        public GridData(bool occupied, int visited)
        {
            this.isOccupied = occupied;
            this.numTimesVisited = visited;
        }
    }
}
