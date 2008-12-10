using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class Food : PhysObject
    {
        public Food(int id, Coordinates location, float orientation, float width, float height) :
            base(id, ObjectType.Food, location, orientation, width, height)
        {
            //TODO Add food-specific constructor parameters
        }
    }
}
