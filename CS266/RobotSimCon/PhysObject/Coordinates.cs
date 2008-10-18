using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    public class Coordinates
    {
        public Coordinates(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        // No good reason not to make these public
        public float X;
        public float Y;

    }
}
