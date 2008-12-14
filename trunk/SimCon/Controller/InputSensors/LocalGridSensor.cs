using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller.InputSensors
{
    public class LocalGridSensor : InputSensor
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

            Grid grid = CS266.SimCon.Controller.ControlLoop.robotGrid;

            GridData robotLocation = ControlLoop.robotGrid.getGridLoc(this.robot.Location);
            
            int row = robotLocation.row;
            int col = robotLocation.col;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int gridcol = col - 1 + i;
                    int gridrow = row - 1 + j;

                    if (0 <= gridrow && 0 <= gridcol && gridrow < grid.NumSquaresY && gridcol < grid.NumSquaresX)
                    {
                        localgrid[i, j] = (double)grid.gridData[gridcol, gridrow].numTimesVisited;
                    }
                    else
                    {
                        localgrid[i, j] = -1;
                    }
                }
            }


            //for (int j = 2; j >= 0; j--)
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        Console.Write((int)localgrid[i,j]  + " ");
            //    }
            //    Console.WriteLine();
            //}       
        }
    }
}

