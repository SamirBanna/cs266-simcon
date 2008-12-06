using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller
{
    public class Grid
    {
        // World state to figure out locations of current robots
        public ControllerWorldState ws;

        // Actual grid
        public GridData[,] gridData;

        // Dimensions of the world
        public double WorldWidth;
        public double WorldHeight;

        // public double minX;
        // public double minY;

        // Dimensions of the grid
        public int NumSquaresX;
        public int NumSquaresY;

        // Previous location of robot. Only exists if robot has continuous markings on
        public Dictionary<Robot, Coordinates> prevLocations;

        // constructor gets a reference to the world state
        public Grid(ControllerWorldState ws, double WorldWidth, double WorldHeight, int numX, int numY)
        {
            this.ws = ws;
            this.WorldWidth = WorldWidth;
            this.WorldHeight = WorldHeight;
            this.NumSquaresX = numX;
            this.NumSquaresY = numY;

            gridData = new GridData()[NumSquaresX, NumSquaresY];
        }
        // Update the robot's current position on the grid
        public void MarkLocation(Robot robot)
        {
            
            // Check if robot has a previous location. This means continuous marking
            // is turned on
            if (prevLocations.ContainsKey(robot))
            {
                int prevX = prevLocations[robot].X;
                int prevY = prevLocations[robot].Y;
                int newX = robot.Location.X;
                int newY = robot.Location.Y;
                
                int curX = prevX;
                int curY = prevY;
                // Mark continuously with interpolation
                // Assume previous location is already marked
                GridData finalSpot = getGridLoc(newX, newY);
                GridData curSpot = getGridLoc(curX, curY);

                incr = .3;
                do
                {
                    GridData nextSpot;
                    // Increment X and Y
                    if (newX > prevX)
                        curX += incr;
                    else
                        curX -= incr;
                    
                    if (newY > prevY)
                        curY += incr;
                    else
                        curY -= incr;
                    // Get next grid spot
                    nextSpot = getGridLoc(curX, curY);

                    // If different mark it
                    if (nextSpot != curSpot)
                    {
                        nextSpot.numTimesVisited++;
                        curSpot = nextSpot;
                    }
                } while (curSpot != finalSpot);

                // At the end, update previous location to current location
                prevLocations.Item(robot) = new Coordinates(newX, newY);
            }
            else
            {
                // Mark just at current location
                getGridLoc(robot.Location).numTimesVisited++;
            }

        }

        // Turn on continuous marking
        public void TurnOnContinuousMarking(Robot robot)
        {
            // Make a copy of the location
            Coordinates locCopy = new Coordinates;
            locCopy.X = robot.Location.X;
            locCopy.Y = robot.Location.Y;

            // Get location of robot and store in prevLocations
            prevLocations.Add(robot, locCopy);
        }

        // Turn off continuous marking, take robot off of prevLocations list
        public void TurnOffContinuousMarking(Robot robot)
        {
            prevLocations.Remove(robot);
        }


        /* Get the location on the grid (actually the GridData object at that
         * location, which contains all the information)
         */ 
        public GridData getGridLoc(Coordinates location)
        {
            // Get grid coordinates. Round down
            int gridX = (int)NumSquaresX * location.X / WorldWidth;
            int gridY = (int)NumSquaresY * location.Y / WorldHeight;

            return gridData[gridX, gridY];
        }
    }
}
