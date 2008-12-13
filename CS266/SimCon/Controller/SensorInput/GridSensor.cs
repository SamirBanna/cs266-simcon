using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    class GridSensor : SensorInput
    {
        //returns length of grid cell edge
        public double getCellLength()
        {
            return ControlLoop.robotGrid.getLengthGridX();
        }

        //returns true if food in any adjacent cells
        public bool detectFoodInAdjCells()
        {
            int[] loc = ControlLoop.robotGrid.getLocObj(this.robot);
            int x = loc[0];
            int y = loc[1];

            //TODO: check if orientation gives the following information:
            //0 = facing east, 90 = facing north, 180 = facing west, -90 = facing south
            double orientation = this.robot.Orientation;
            //if facing to east we do not want to look backwards (west)
            if ((orientation != 0) && (x - 1 >= 0)){
                GridData west = ControlLoop.robotGrid.getGridLoc(x - 1, y);
                foreach (PhysObject physObj in west.objectsInSquare)
                {
                    if (physObj.GetType() == typeof(Food))
                        return true;
                }
            }
            //if facing to west we do not want to look backwards (east) 
            //TODO. is it 180 or -180
            if ((orientation != 180) && (orientation != -180) && (x + 1 < ControlLoop.robotGrid.NumSquaresX))
            {
                GridData east = ControlLoop.robotGrid.getGridLoc(x + 1, y);
                foreach (PhysObject physObj in east.objectsInSquare)
                {
                    if (physObj.GetType() == typeof(Food))
                        return true;
                }
            }
            //if facing to south we do not want to look backwards (north)
            if ((orientation != -90) && (y + 1 < ControlLoop.robotGrid.NumSquaresY))
            {
                GridData north = ControlLoop.robotGrid.getGridLoc(x, y + 1);
                foreach (PhysObject physObj in north.objectsInSquare)
                {
                    if (physObj.GetType() == typeof(Food))
                        return true;
                }
               
            }
            //if facing to north we do not want to look backwards (south)
            if ((orientation != 90) && (y - 1 >= 0))
            {
                GridData south = ControlLoop.robotGrid.getGridLoc(x, y - 1);
                foreach (PhysObject physObj in south.objectsInSquare)
                {
                    if (physObj.GetType() == typeof(Food))
                        return true;
                }
            }
            return false;
        }

        //returns array containing angles corresponding to possible moves: forwards, left, or right                
        public List<double> getPossibleMoves()
        {
            int[] loc = ControlLoop.robotGrid.getLocObj(this.robot);
            int x = loc[0];
            int y = loc[1];

            List<double> possibleMoves = new List<double>();

            //TODO. check if orientation does give the following information:
            //orientation = 0 means robot is facing east, 90 means is facing north,...
            double orientation = this.robot.Orientation;

            //we do not check the direction we came from

            //check if we can go west
            if (orientation != 0)
            {
                try
                {
                    GridData west = ControlLoop.robotGrid.getGridLoc(x - 1, y);
                    if (west.objectsInSquare.Count == 0)
                    {
                        int turnDegrees = 180 - (int)orientation; // west = 180 degrees
                        if (turnDegrees > 180) turnDegrees = turnDegrees - 360;
                        possibleMoves.Add(turnDegrees);
                    }
                }catch(System.Exception)
                {
                }
            }
            //check if we can go east
            if (orientation != 180 && orientation != -180)
            {
                try
                {
                    GridData east = ControlLoop.robotGrid.getGridLoc(x + 1, y);
                    if (east.objectsInSquare.Count == 0)
                    {
                        int turnDegrees = 0 - (int)orientation; // east = 0 degrees
                        if (turnDegrees > 180) turnDegrees = turnDegrees - 360;
                        possibleMoves.Add(turnDegrees);
                    }
                }
                catch (System.Exception)
                {
                }
            }
            //check if we can go north
            if (orientation != -90)
            {
                try
                {
                    GridData north = ControlLoop.robotGrid.getGridLoc(x, y + 1);
                    if (north.objectsInSquare.Count == 0)
                    {
                        int turnDegrees = 90 - (int)orientation; // north = 90 degrees
                        if (turnDegrees > 180) turnDegrees = turnDegrees - 360;
                        possibleMoves.Add(turnDegrees);
                    }
                }
                catch (System.Exception)
                {
                }
            }
            //check if we can go south
            if (orientation != 90)
            {
                try
                {
                    GridData south = ControlLoop.robotGrid.getGridLoc(x, y - 1);
                    if (south.objectsInSquare.Count == 0)
                    {
                        int turnDegrees = -90 - (int)orientation; // south = -90 degrees
                        if (turnDegrees > 180) turnDegrees = turnDegrees - 360;
                        possibleMoves.Add(turnDegrees);
                    }
                }
                catch (System.Exception)
                {
                }
            }
            return possibleMoves;
        }
        public override void UpdateSensor()
        {
            //TODO
        }
    }

}
