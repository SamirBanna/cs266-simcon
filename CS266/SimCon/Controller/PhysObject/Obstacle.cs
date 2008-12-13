using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class Obstacle : PhysObject
    {
        public Obstacle(int id, String desc, Coordinates location, double orientation, double width, double height) :
            base(id, desc, location, orientation, width, height)
        {
        }
    }
}
