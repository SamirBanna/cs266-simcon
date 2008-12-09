using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    // What occupies the grid. Contains information that can be added or taken away
    public class GridData
    {
        public int numTimesVisited;
        public List<PhysObject> objectsInSquare;

        // If no information given, assume initialized at beginning
        public GridData()
        {
            numTimesVisited = 0;
            objectsInSquare = new List<PhysObject>();
        }
        public GridData(int visited)
        {
            this.numTimesVisited = visited;
        }
    }
}
