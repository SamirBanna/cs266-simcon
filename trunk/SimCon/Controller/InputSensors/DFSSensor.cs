
using CS266.SimCon.Controller.Algorithms;
using CS266.SimCon.Controller.Experiments;

namespace CS266.SimCon.Controller.InputSensors
{
    public class DFSSensor : InputSensor
    {


        // assume grid coordinates are passed in, e.g., (0,0)
        public DFSSensor()
        {
        }

   
        public double getDirectionPred()
        {
            double turnDegrees = 0;

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
            double orientation = calcClosestOrientation(this.robot.Orientation);

            // if pred is in north cell
            if (x == predx && y == (predy - 1))
            {
                if (orientation == 90) // assume orientation in simulator is precise
                    turnDegrees = 0;
                else if (orientation == 0)
                    turnDegrees = 90;
                else if (orientation == 180 || orientation == -180)
                    turnDegrees = 270;
            }

            // if pred is in south cell
            if (x == predx && y == (predy + 1))
            {
                if (orientation == -90)
                    turnDegrees = 0;
                else if (orientation == 0)
                    turnDegrees = 270;
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
                    turnDegrees = 270;
            }

            // if pred is in east cell
            if (x == (predx - 1) && y == predy)
            {
                if (orientation == 0)
                    turnDegrees = 0;
                else if (orientation == 90)
                    turnDegrees = 270;
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
            //ControlLoop..robots.Add(newRobot);
            
        }

  
        public Coordinates getDoor()
        {
            return new Coordinates(DFSExperiment.doorX, DFSExperiment.doorY);
        }

        public double calcClosestOrientation(double actual) {
            if (actual > 45 && actual <= 135)
                return 90;
            else if (actual > 135 && (actual <= 180 || actual < -135))
                return 180;
            else if (actual < -45 && actual >= -135)
                return -90;
            else if (actual <= 45 && actual >= -45)
                return 0;
            else
                return actual;
        }

        public override void UpdateSensor()
        {
          //TODO
        }
    }
}
