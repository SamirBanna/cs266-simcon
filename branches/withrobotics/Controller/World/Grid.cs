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

            gridData = new GridData[NumSquaresX, NumSquaresY];
            for (int i = 0; i < NumSquaresX; i++)
            {
                for (int j = 0; j < NumSquaresY; j++)
                {
                    gridData[i, j] = new GridData(i, j);
                }
            }
        }

        public void Mark(Robot robot, bool continuous)
        {


            if (!continuous || !prevLocations.ContainsKey(robot))
            {
                getGridLoc(robot.Location).pheromoneLevel++;
                return;
            }
            Coordinates newLoc = new Coordinates(robot.Location.X, robot.Location.Y);
            Coordinates prevLoc = new Coordinates(prevLocations[robot].X, prevLocations[robot].Y);
            Coordinates curLoc = new Coordinates(prevLocations[robot].X, prevLocations[robot].Y);

            // Mark continuously with interpolation
            // Assume previous location is already marked
            GridData finalSpot = getGridLoc(newLoc);
            GridData curSpot = getGridLoc(curLoc);


            float incr = (float)Math.Min((WorldHeight / NumSquaresY), (WorldWidth / NumSquaresX)) / 2;
            float dX = robot.Location.X - prevLocations[robot].X;
            float dY = robot.Location.Y - prevLocations[robot].Y;
            float incrX = incr * dX / (float)Math.Sqrt(dX * dX + dY * dY);
            float incrY = incr * dY / (float)Math.Sqrt(dX * dX + dY * dY);



            do
            {
                GridData nextSpot;
                // Increment X and Y
                curLoc.X += incrX;
                curLoc.Y += incrY;

                // Get next grid spot
                nextSpot = getGridLoc(curLoc);

                // If different mark it
                if (nextSpot != curSpot)
                {
                    nextSpot.pheromoneLevel++;
                    curSpot = nextSpot;
                }
            } while (curSpot != finalSpot);


            return;
        }

        public void GridUpdate(Robot robot)
        {
            // Take out robot from locations of where robot is in the grid
            findObj(robot).objectsInSquare.Remove(robot);

            // Add robot
            getGridLoc(robot.Location).objectsInSquare.Add(robot);

            Coordinates newLoc = new Coordinates(robot.Location.X, robot.Location.Y);
            GridData finalSpot = getGridLoc(newLoc);

            if (!prevLocations.ContainsKey(robot))
            {
                prevLocations[robot] = newLoc;
                finalSpot.numTimesVisited++;
                return;
            }

            Coordinates prevLoc = new Coordinates(prevLocations[robot].X, prevLocations[robot].Y);
            Coordinates curLoc = new Coordinates(prevLocations[robot].X, prevLocations[robot].Y);
            GridData curSpot = getGridLoc(curLoc);

            if (finalSpot == curSpot)
            {
                // don't mark anything
                return;
            }

            float incr = (float)Math.Min((WorldHeight / NumSquaresY), (WorldWidth / NumSquaresX)) / 2;
            float dX = robot.Location.X - prevLocations[robot].X;
            float dY = robot.Location.Y - prevLocations[robot].Y;
            float incrX = incr * dX / (float)Math.Sqrt(dX * dX + dY * dY);
            float incrY = incr * dY / (float)Math.Sqrt(dX * dX + dY * dY);



            do
            {
                GridData nextSpot;
                // Increment X and Y
                curLoc.X += incrX;
                curLoc.Y += incrY;

                // Get next grid spot
                nextSpot = getGridLoc(curLoc);

                // If different mark it
                if (nextSpot != curSpot)
                {
                    nextSpot.numTimesVisited++;
                    curSpot = nextSpot;
                }
            } while (curSpot != finalSpot);

            prevLocations[robot] = curLoc;
            return;
        }

        // Turn on continuous marking
        public void TurnOnContinuousMarking(Robot robot)
        {
            // Make a copy of the location
            Coordinates locCopy = new Coordinates(robot.Location.X, robot.Location.Y);

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
            int gridX = (int)Math.Floor(NumSquaresX * location.X / WorldWidth);
            int gridY = (int)Math.Floor(NumSquaresY * location.Y / WorldHeight);

            return gridData[gridX, gridY];
        }

        public GridData getGridLoc(int gridX, int gridY)
        {
            return gridData[gridX, gridY];
        }

        // Find an object in the grid (right now, only robots)
        public GridData findObj(PhysObject obj)
        {
            for (int i = 0; i < NumSquaresX; i++)
            {
                for (int j = 0; j < NumSquaresY; j++)
                {
                    if (gridData[i, j].objectsInSquare.Contains(obj))
                        return gridData[i, j];
                }
            }
            return null;
        }

        public int[] getLocObj(PhysObject obj)
        {
            for (int i = 0; i < NumSquaresX; i++)
            {
                for (int j = 0; j < NumSquaresY; j++)
                {
                    if (gridData[i, j].objectsInSquare.Contains(obj))
                    {
                        int[] loc = { i, j };
                        return loc;
                    }
                }
            }
            return null;
        }

        // returns the coordinates to the center of a grid cell
        public Coordinates getCenterOfCell(int gridX, int gridY)
        {
            float x = ((float)gridX + (float).5) * getLengthGridX();
            float y = ((float)gridY + (float).5) * getLengthGridY();
            return new Coordinates(x, y);
        }

        public float getLengthGridX()
        {
            return (float)WorldWidth / (float)NumSquaresX;
        }

        public float getLengthGridY()
        {
            return (float)WorldHeight / (float)NumSquaresY;
        }

        public List<PhysObject> getObjectsInCell(int x, int y)
        {
            return gridData[x, y].objectsInSquare;
        }

        public bool gridVisited()
        {
            for (int i = 0; i < NumSquaresX; i++)
            {
                for (int j = 0; j < NumSquaresY; j++)
                {
                    if (gridData[i, j].numTimesVisited == 0)
                        return false;
                }
            }
            return true;
        }
    }
}

