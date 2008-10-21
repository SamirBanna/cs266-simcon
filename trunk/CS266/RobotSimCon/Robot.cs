using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class Robot : PhysObject
    {
        public Robot(int id, String name, Coordinates location, float orientation, float width, float height) :
            base(id, name, location, orientation, width, height)
        {
            Stop();
        }

        bool CanMoveForward = true;
        bool CanMoveBackward = true;

        public float Speed = 0;

        public PhysicalRobotAction CurrentAction;
        public SensorInput CurrentInput;

        // Needed
        // A way to determine vision cone, which objects are in it

        public Algorithm CurrentAlgorithm;

        // TODO Figure out units

        public void ProcessInput(SingleProximityInput input)
        {

        }
        
        public void MoveForward(float distance)
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, PhysicalActionType.MoveForward, distance));
        }

        public void MoveBackward(float distance)
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, PhysicalActionType.MoveBackward, distance));
        }

        public void Stop()
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, PhysicalActionType.Stop));
        }

        public void ChangeSpeed(float speed)
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, PhysicalActionType.SetSpeed, speed));
        }

        public void Turn(float degrees)
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, PhysicalActionType.Turn, degrees));
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
