using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.RoboticsClasses;
using CS266.SimCon.Simulator;

namespace CS266.SimCon.Controller.WorldOutputInterfaces
{
    public abstract class WorldOutputInterface
    {
        private SimulationTutorial1 sim;
        public void DoActions(Queue<PhysicalRobotAction> actions)
        {
            RobotActions allNewActions = new RobotActions();
            // Translate PhysicalRobotAction to RobotAction
            foreach(PhysicalRobotAction action in actions)
            {
                float degreestoturn;
                float distance;
                
                // Figure out which action
                if(action.ActionType == PhysicalActionType.MoveForward)
                {
                    degreestoturn = 0;
                    distance = action.ActionValue;
                }else if (action.ActionType == PhysicalActionType.MoveBackward){
                    degreestoturn = 180;
                    distance = action.ActionValue;
                }else if (action.ActionType == PhysicalActionType.Turn){
                    degreestoturn = action.ActionValue;
                    distance = 0;
                }else if(action.ActionType == PhysicalActionType.Stop){
                    degreestoturn = 0;
                    distance = 0;
                }
                // Translate this robot action to a member of RobotAction class
                RobotAction newaction = new RobotAction(action.RobotId, degreestoturn, distance);
                // Add to list of actions to pass to simulator
                allNewActions.Add(newaction);
            }
            // Send list of actions to pass to simulator
            // Need to check what function this actually references
            CS266.SimCon.Simulator.SimulationTutorial1.doActions(allNewActions);
        }
    }
}
