using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    // What occupies the grid. Contains information that can be added or taken away
    public class GridData
    {
        public int numTimesVisited;
        public int pheromoneLevel;
        public List<PhysObject> objectsInSquare;
        public int row = 0;
        public int col = 0;

        // If no information given, assume initialized at beginning
        public GridData()
        {
            pheromoneLevel = 0;
            numTimesVisited = 0;
            objectsInSquare = new List<PhysObject>();
        }
        public GridData(int col, int row)
        {
            pheromoneLevel = 0;
            numTimesVisited = 0;
            objectsInSquare = new List<PhysObject>();
            this.col = col;
            this.row = row;
            
        }

        public GridData(int visited)
        {
            this.numTimesVisited = visited;
        }
    }
}
