using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class LocalGridSensor : SensorInput
    {
        double[,] localgrid;
        int localnumrows = 3;
        int localnumcols = 3;


        public LocalGridSensor()
        {
            localgrid = new double[localnumrows, localnumcols];
        }

        public LocalGridSensor(int rows, int cols)
        {
            localgrid = new double[rows, cols];
            localnumrows = rows;
            localnumcols = cols;
        }


        public LocalGridSensor(ControllerWorldState worldState)
        {
            this.worldState = worldState;
        }

        public LocalGridSensor(ControllerWorldState worldState, int rows, int cols)
        {
            this.worldState = worldState;
            localgrid = new double[rows, cols];
            localnumrows = rows;
            localnumcols = cols;
        }


        public override void UpdateSensor()
        {
            double[][] grid = CS266.SimCon.Controller.ControlLoop.RobotGrid.grid;

            Coordinates robotLocation = getGridLoc(this.robot.Location);


        }
    }
}

