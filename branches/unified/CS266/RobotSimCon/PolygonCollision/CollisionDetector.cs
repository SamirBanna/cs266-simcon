using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS266.SimCon.Controller.PolygonIntersection
{
    public partial class CollisionDetector
    {
        public static List<List<PhysObject>> Detect(List<Robot> worldRobots, List<PhysObject> allObjects)
        {
            int numRobots = worldRobots.Count;
            List<Polygon> worldPolys = new List<Polygon>();

            List<List<PhysObject>> collisions = new List<List<PhysObject>>();

            foreach (PhysObject o in allObjects)
            {
                Polygon p = new Polygon();
                
                float colScale = 1;

                // NOTE: Currently assuming a bottom-to-top coordinate system

                // Top left
                p.Points.Add(new Vector(o.Location.X - colScale * o.Width / 2, o.Location.Y + colScale * o.Height / 2));

                // Top right
                p.Points.Add(new Vector(o.Location.X + colScale * o.Width / 2, o.Location.Y + colScale * o.Height / 2));

                // Bottom right
                p.Points.Add(new Vector(o.Location.X + colScale * o.Width / 2, o.Location.Y - colScale * o.Height / 2));

                // Bottom left
                p.Points.Add(new Vector(o.Location.X - colScale * o.Width / 2, o.Location.Y - colScale * o.Height / 2));

                p.BuildEdges();

                worldPolys.Add(p);
               
            }

            int numAllObjects = allObjects.Count;

            for (int rIndex = 0; rIndex < numRobots; ++rIndex)
            {
                List<PhysObject> myCollisions = new List<PhysObject>();
                for (int objIndex = rIndex + 1; objIndex < numAllObjects; ++objIndex)
                {
                    bool isColliding = PolygonCollision(worldPolys[rIndex],worldPolys[objIndex],new Vector()).Intersect;
                    if (isColliding)
                    {
                        myCollisions.Add(allObjects[objIndex]);
                    }
                }

                collisions.Add(myCollisions);
            }

            return collisions;
        }
    }
}
