﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class DFSSensor : SensorInput
    {
        int doorX; // in grid coordinates
        int doorY; // in grid coordinates

        // assume grid coordinates are passed in, e.g., (0,0)
        public DFSSensor(int doorX, int doorY)
        {
            this.doorX = doorX;
            this.doorY = doorY;
        }

   
        public float getDirectionPred()
        {
            float turnDegrees = 0;

            // get my grid coordinates
            int[] myLoc = ControlLoop.robotGrid.getLocObj(this.robot);
            int x = myLoc[0];
            int y = myLoc[1];

            // get pred grid coordinates
            Robot pred = ((BasicDFS)this.robot.CurrentAlgorithm).pred;
            int[] predLoc = ControlLoop.robotGrid.getLocObj(pred);
            int predx = predLoc[0];
            int predy = predLoc[1];

            // get my orientation
            float orientation = this.robot.Orientation;

            // if pred is in north cell
            if (x == predx && y == (predy - 1))
            {
                if (orientation == 90) // assume orientation in simulator is precise
                    turnDegrees = 0;
                else if (orientation == 0)
                    turnDegrees = 90;
                else if (orientation == 180 || orientation == -180)
                    turnDegrees = -90;
            }

            // if pred is in south cell
            if (x == predx && y == (predy + 1))
            {
                if (orientation == -90) 
                    turnDegrees = 0;
                else if (orientation == 0)
                    turnDegrees = -90;
                else if (orientation == 180 || orientation == -180)
                    turnDegrees = 90;
            }

            // if pred is in west cell
            if (x == (predx+1) && y == predy)
            {
                if (orientation == 180 || orientation == -180)
                    turnDegrees = 0;
                else if (orientation == 90)
                    turnDegrees = 90;
                else if (orientation == -90)
                    turnDegrees = -90;
            }

            // if pred is in east cell
            if (x == (predx - 1) && y == predy)
            {
                if (orientation == 0)
                    turnDegrees = 0;
                else if (orientation == 90)
                    turnDegrees = -90;
                else if (orientation == -90)
                    turnDegrees = 90;
            }
            return turnDegrees;
        }

        // Returns the new id of a robot to be created
        public int nextID()
        {
            int maxId = -1;
            foreach (Robot robot in worldState.robots)
            {
                if (robot.Id > maxId)
                {
                    maxId = robot.Id;
                }
            }
            return maxId + 1;
        }

  
        public void addRobotToList(Robot newRobot){
            // add robot to world state
            worldState.robots.Add(newRobot);
            
        }

  
        public Coordinates getDoor()
        {
            return ControlLoop.robotGrid.getCenterOfCell(doorX, doorY);
        }


        public override void UpdateSensor()
        {
          //TODO
        }
    }
}