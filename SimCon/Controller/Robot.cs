using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class Robot : PhysObject
    {
        public Robot(int id, String desc, Coordinates location, double orientation, double width, double height) :
            base(id, desc, location, orientation, width, height)
        {
            Stop();
        }

        bool CanMoveForward = true;
        bool CanMoveBackward = true;
      
      
        public PhysicalRobotAction CurrentAction;
       
        public Dictionary<String, SensorInput> Sensors;

        // Needed
        // A way to determine vision cone, which objects are in it

        public Algorithm CurrentAlgorithm;

        // TODO Figure out units

      

        public void MoveForward(double distance)
        {
            if (CanMoveForward)
            {
                ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, PhysicalActionType.MoveForward, distance));
            }
        }

        public void MoveBackward(double distance)
        {
            if (CanMoveBackward)
            {
                ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, PhysicalActionType.MoveBackward, distance));
            }
        }

        public void Stop()
        {
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, PhysicalActionType.Stop));
        }

        //public void ChangeSpeed(double speed)
        //{
        //    ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(this.Id, PhysicalActionType.SetSpeed, speed));
        //}

        public void Turn(double degrees)
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


         public void createNewRobot(int IdOfNewRobot)
        {
            //what is the value we should pass here? Maybe the door position?
            double value = 90;
            ControlLoop.ActionQueue.Enqueue(new PhysicalRobotAction(IdOfNewRobot, PhysicalActionType.CreateRobot, value));

        }

  


    }
}
