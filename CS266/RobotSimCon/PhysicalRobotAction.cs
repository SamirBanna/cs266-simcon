using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    class PhysicalRobotAction
    {
        enum PhysicalActionType { MoveForward, MoveBackward, Turn, SetSpeed, Stop };

        public PhysicalRobotAction(int id, PhysicalActionType actionType, float value)
        {
            this.RobotId = id;
            this.PhysicalActionType = actionType;
            // Value is meaningful only if it's not a Stop action
            this.ActionValue = (actionType == PhysicalActionType.Stop) ? 0 : value;
        }

        public int RobotId;
        public PhysicalActionType ActionType;
        public float ActionValue;

    }


}
