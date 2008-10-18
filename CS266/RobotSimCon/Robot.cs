using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    public class Robot : PhysObject
    {
        public float Speed = 0;

        // Needed
        // A way to determine vision cone, which objects are in it

        public Algorithm CurrentAlgorithm;

        // TODO Figure out units

        public void ProcessInput(SingleProximityInput input)
        {

        }
        
        public void MoveForward(float distance)
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, MoveForward, distance));
        }

        public void MoveBackward(float distance)
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, MoveBackward, distance));
        }

        public void Stop()
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, Stop));
        }

        public void ChangeSpeed(float speed)
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, ChangeSpeed, speed));
        }

        public void Turn(float degrees)
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, Turn, degrees));
        }


        public void SendMessage(int id, Message msg)
        {
            // TODO
        }

        public void SendMessage(Message msg)
        {
            // TODO
        }
    }
}
