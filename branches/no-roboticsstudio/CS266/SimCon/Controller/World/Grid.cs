using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller
{
    public class Grid
    {
        public GridData[][] gridData;
        public double WorldWidth;
        public double WorldHeight;

        // public double minX;
        // public double minY;

        public int NumSquaresX;
        public int NumSquaresY;

        // Previous location of robot. Only exists if robot has continuous markings on
        private Dictionary<Robot, Coordinates> prevLocations;

        //public static Grid GetGrid(double WorldWidth, double WorldHeight, int numX, int numY)
        //{
        //    if (g == null)
        //    {
        //        g = Grid(WorldWidth, WorldHeight, numX, numY);

        //    }
        //    return g;
        //}

        public Grid(double WorldWidth, double WorldHeight, int numX, int numY)
        {
            this.WorldWidth = WorldWidth;
            this.WorldHeight = WorldHeight;
            this.NumSquaresX = numX;
            this.NumSquaresY = numY;

            gridData = new GridData()[NumSquaresX, NumSquaresY];
        }
        // Update the robot's current position on the grid
        public void MarkLocation(int robotId)
        {

        }

        public GridData getGridLoc(Coordinates location)
        {
            // Get grid coordinates
            int gridX = (int)NumSquaresX * location.X / WorldWidth;
        }
    }
}
