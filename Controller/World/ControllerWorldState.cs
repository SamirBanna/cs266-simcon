using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Controller
{
    public class ControllerWorldState
    {
        public List<Robot> robots;
        public List<PhysObject> physobjects;
        public List<Food> foodlist;
        public double maxX;
        public double maxY;

        public ControllerWorldState(List<Robot> robots, List<PhysObject> physobjects, List<Food> foodlist, double maxX, double maxY)
        {
            this.maxX = maxX;
            this.maxY = maxY; 
            this.robots = robots;
            this.physobjects = physobjects;
            this.foodlist = foodlist;
        }

    }
}
