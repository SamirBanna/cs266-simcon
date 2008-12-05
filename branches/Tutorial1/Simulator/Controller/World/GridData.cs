using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    // What occupies the grid. Contains information that can be added or taken away
    public class GridData
    {
        public bool isOccupied;
        public bool hasVisited;

        // If no information given, assume initialized at beginning
        public GridData()
        {
            isOccupied = 0;
            hasVisited = 0;
        }
        public GridData(bool occupied, bool visited)
        {
            this.isOccupied = occupied;
            this.hasVisited = visited;
        }
    }
}
