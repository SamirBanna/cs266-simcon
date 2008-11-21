using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class SingleProximityInput : SensorInput
    {
        public float Distance;
        public ObjectType ObjectType;
        public SingleProximityInput(float distance, ObjectType objectType)
        {
            this.Distance = distance;
            this.ObjectType = objectType;
        }

        //public SingleProximityInput(World

    }
}
