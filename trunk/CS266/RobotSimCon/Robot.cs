using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    class Robot : PhysObject
    {
        public float Speed = 0;

        // Needed
        // A way to determine vision cone, which objects are in it

        public SensorSet Sensor;
        public Algorithm CurrentAlgorithm;

        // TODO Figure out units

        public void ProcessInput(SingleProximityInput input)
        {

        }
        
        public void MoveForward(float distance)
        {
        }

        public void MoveBackward(float distance)
        {

        }

        public void Stop()
        {

        }

        public void ChangeSpeed(float speed)
        {

        }

        public void Turn(float degrees)
        {

        }


        public void SendMessage(int id, Message msg)
        {
        }

        public void SendMessage(Message msg)
        {
        }
    }
}
