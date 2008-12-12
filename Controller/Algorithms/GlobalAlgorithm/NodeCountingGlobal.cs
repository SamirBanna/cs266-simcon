using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller.Algorithms
{
    public class NodeCountingGlobal : GlobalAlgorithm
    {
        public NodeCountingGlobal() : base()
        {
        }
        public override void Execute()
        {
            bool allCovered = true;
            // Check the global grid
            Grid GlobalGrid = CS266.SimCon.Controller.ControlLoop.robotGrid;
            for (int i = 0; i < GlobalGrid.NumSquaresX; i++)
            {
                for (int j = 0; j < GlobalGrid.NumSquaresY; j++)
                {
                    // Get grid data
                    GridData cell = GlobalGrid.getGridLoc(i, j);
                    foreach (PhysObject obj in cell.objectsInSquare)
                    {
                        if (!(obj is Obstacle))
                        {
                            if (cell.pheromoneLevel == 0)
                            {
                                allCovered = false;
                            }
                        }
                    }
                }
            }

            if (allCovered)
                throw new Exception.GlobalAlgorithmFinishedException();

        }
    }
}
