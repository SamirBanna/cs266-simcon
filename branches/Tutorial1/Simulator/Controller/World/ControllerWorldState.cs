using System;
using System.Collections.Generic;
using System.Text;

using CS266.SimCon.Controller.World;

namespace CS266.SimCon.Controller
{
    public class ControllerWorldState
    {
        public List<Robot> robots;
        public List<PhysObject> physobjects;

        public double WorldWidth;
        public double WorldHeight;

        public int NumSquaresX;
        public int NumSquaresY;

        public GridData[][] Grid;

        public ControllerWorldState(List<Robot> robots, List<PhysObject> physobjects)
        {
            this.robots = robots;
            this.physobjects = physobjects;
        }

        public ControllerWorldState(List<Robot> robots, List<PhysObject> physObjects,
            double worldWidth, double worldHeight, int numSquaresX, int numSquaresY)
            : this(robots, physobjects)
        {
            WorldHeight = worldHeight;
            WorldWidth = worldWidth;
            NumSquaresX = numSquaresX;
            NumSquaresY = numSquaresY;

            Grid = new GridData(false)[NumSquaresX][NumSquaresY];
        }



    }
}
