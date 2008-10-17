using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    class PhysObject
    {
        public PhysObject(int id, String name)
        {
            _id = id;
            _name = name;
        }
        int _id;
        String _name;
        
        /// <summary>
        /// Width, or radius of a round object
        /// </summary>
        public float Width = 1;
        /// <summary>
        /// Height, ignored for round objects
        /// </summary>
        public float Height = 1;
        public bool IsRound = true;
        private Coordinates Center;
        /// <summary>
        /// Heading in degrees from _____
        /// </summary>
        private float heading = 0;

        
    }
}
