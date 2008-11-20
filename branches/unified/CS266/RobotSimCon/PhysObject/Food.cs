using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class Food : PhysObject
    {
        public Food(int id, String name, Coordinates location, float orientation, float width, float height) :
            base(id, name, location, orientation, width, height)
        {
            //TODO Add food-specific constructor parameters
        }
    }
}
