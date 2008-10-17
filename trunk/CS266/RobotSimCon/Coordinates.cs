using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    class Coordinates
    {
        Coordinates(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        // No good reason not to make these public
        float X;
        float Y;

    }
}
