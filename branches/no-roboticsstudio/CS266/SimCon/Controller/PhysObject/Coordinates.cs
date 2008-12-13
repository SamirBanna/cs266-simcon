using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class Coordinates
    {
        public Coordinates(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        // No good reason not to make these public
        public double X;
        public double Y;

    }
}
