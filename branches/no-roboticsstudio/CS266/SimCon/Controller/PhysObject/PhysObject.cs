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
        public PhysObject(int id, String desc, Coordinates location, double orientation, double width, double height)
        {
            this.Id = id;
            this.Description = desc;
            this.Location = location;
            this.Orientation = orientation;
            this.Width = width;
            this.Height = height;
        }
        public int Id;
        public String Description;
        
        /// <summary>
        /// Width, or radius of a round object
        /// </summary>
        public double Width = 1;
        /// <summary>
        /// Height, ignored for round objects
        /// </summary>
        public double Height = 1;
        public bool IsRound = true;
        public Coordinates Location;
        /// <summary>
        /// Heading in degrees from _____
        /// </summary>
        public double Orientation = 0;

        
    }
}
