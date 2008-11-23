using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class Obstacle : PhysObject
    {
        public Obstacle(int id, ObjectType type, Coordinates location, float orientation, float width, float height) :
            base(id, type, location, orientation, width, height)
        {
            //TODO Add obstacle-specific constructor parameters
        }
    }
}
