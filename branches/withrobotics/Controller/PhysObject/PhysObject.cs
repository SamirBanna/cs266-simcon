using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    /// <summary>
    /// An class describing an abstract physical object. This is the superclass for all object types in the world.
    /// </summary>
    public abstract class PhysObject
    {
        public PhysObject(int id, String name, Coordinates location, float orientation, float width, float height)
        {
            this.Id = id;
            this.Name = name;
            this.Location = location;
            this.Orientation = orientation;
            this.Width = width;
            this.Height = height;
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
