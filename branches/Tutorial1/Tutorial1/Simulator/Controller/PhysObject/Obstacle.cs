using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class Obstacle : PhysObject
    {
        public Obstacle(int id, String name, Coordinates location, float orientation, float width, float height) :
            base(id, name, location, orientation, width, height)
        {
            //TODO Add obstacle-specific constructor parameters
        }
    }
}
