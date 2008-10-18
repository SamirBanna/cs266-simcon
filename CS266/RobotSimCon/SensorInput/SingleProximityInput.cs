using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
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
    }
}
