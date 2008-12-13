using System;
using System.Collections.Generic;
using System.Text;

namespace CS266.SimCon.Simulator
{
    public class WorldPair
    {
        public WorldDimensions WD;
        public WorldState WS;

        public WorldPair(WorldDimensions WD, WorldState WS)
        {
            this.WD = WD;
            this.WS = WS;
        }
    }
}
