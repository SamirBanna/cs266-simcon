using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    class SingleProximityInput : SensorInput
    {
        public float Distance;
        public ObjectType ObjectType;
        SingleProximityInput(float distance, ObjectType objectType)
        {
            this.Distance = distance;
            this.ObjectType = objectType;
        }
    }
}
