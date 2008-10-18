using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCon.PolygonIntersection
{
    public partial class CollisionDetector
    {
        public static void Detect(List<Robot> worldRobots, List<PhysObject> worldObjects)
        {
            int numRobots = worldRobots.Count;
            List<Polygon> worldPolys = new List<Polygon>();

            List<PhysObject> allObjects = new List<PhysObject>();
            allObjects = List<PhysObject>.Concat(worldRobots,worldObjects);


            List<List<PhysObject>> collisions = new List<List<PhysObject>>();

            foreach (PhysObject o in allObjects)
            {
                Polygon p = new Polygon();

                // NOTE: Currently assuming a top-to-bottom coordinate system

                // Top left
                p.Points.Add(new Vector(o.Location.X - o.Width/2, o.Location.Y + o.Height/2));

                // Top right
                p.Points.Add(new Vector(o.Location.X + o.Width/2, o.Location.Y + o.Height/2));

                // Bottom right
                p.Points.Add(new Vector(o.Location.X + o.Width/2, o.Location.Y - o.Height/2));

                // Bottom left
                p.Points.Add(new Vector(o.Location.X - o.Width/2, o.Location.Y - o.Height/2));

                p.BuildEdges();

                worldPolys.Add(p);
               
            }

            int numRobots = worldRobots.Count;
            int numAllObjects = allObjects.Count;

            for (int rIndex = 0; rIndex < numRobots; ++rIndex)
            {
                List<PhysObject> myCollisions = new List<PhysObject>();
                for (int objIndex = rIndex + 1; objIndex < numAllObjects; ++objIndex)
                {
                    bool isColliding = PolygonCollision(worldPolys[rIndex],worldPolys[objIndex],0).Intersect;
                    if (isColliding)
                    {
                        myCollisions.Add(allObjects[objIndex]);
                    }
                }

                collisions.Add(myCollisions);
            }

        }
    }
}
