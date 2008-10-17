using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon
{
    abstract class PhysObject
    {
        public PhysObject(int id, String name)
        {
            Id = id;
            Name = name;
        }
        public int Id;
        public String Name;
        
        /// <summary>
        /// Width, or radius of a round object
        /// </summary>
        public float Width = 1;
        /// <summary>
        /// Height, ignored for round objects
        /// </summary>
        public float Height = 1;
        public bool IsRound = true;
        public Coordinates Location;
        /// <summary>
        /// Heading in degrees from _____
        /// </summary>
        public float Orientation = 0;

        
    }
}
