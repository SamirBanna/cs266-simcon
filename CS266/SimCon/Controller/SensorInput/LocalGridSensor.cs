using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller
{
    public class LocalGridSensor : SensorInput
    {
        public double[,] localgrid;
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

            GridData[,] grid = CS266.SimCon.Controller.ControlLoop.robotGrid.gridData;

            GridData robotLocation = ControlLoop.robotGrid.getGridLoc(this.robot.Location);
            int row = robotLocation.row;
            int col = robotLocation.col;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int gridrow = row - 1 + i;
                    int gridcol = col - 1 + j;
                    if (0 <= gridrow && 0 <= gridcol && gridrow < grid.GetLength(0) && gridcol < grid.GetLength(1))
                    {
                        localgrid[i, j] = (double)grid[gridrow, gridcol].pheromoneLevel;
                    }
                    else
                    {
                        localgrid[i, j] = -1;
                    }
                }
            }
        }
    }
}

