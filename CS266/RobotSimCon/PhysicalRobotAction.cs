using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    public class PhysicalRobotAction
    {
        public enum PhysicalActionType { MoveForward, MoveBackward, Turn, SetSpeed, Stop };

        public PhysicalRobotAction(int id, PhysicalActionType actionType, float value)
        {
            this.RobotId = id;
            this.ActionType = actionType;
            // Value is meaningful only if it's not a Stop action
            this.ActionValue = (actionType == PhysicalActionType.Stop) ? 0 : value;
        }

        public PhysicalRobotAction(int id, PhysicalActionType actionType)
        {
            if (actionType != PhysicalActionType.Stop)
            {
                throw new ArgumentException("Only Stop action works wihtout a value.");
            }
            this.RobotId = id;
            this.ActionType = actionType;
        }

        public int RobotId;
        public PhysicalActionType ActionType;
        public float ActionValue;

    }


}
