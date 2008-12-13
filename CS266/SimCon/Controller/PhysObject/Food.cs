using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class Food : PhysObject
    {
        public Food(int id, String desc, Coordinates location, double orientation, double width, double height) :
            base(id, desc, location, orientation, width, height)
        {
            //TODO Add food-specific constructor parameters
        }
    }
}
