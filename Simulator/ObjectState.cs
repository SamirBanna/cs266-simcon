using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Robotics.PhysicalModel;

namespace CS266.SimCon.Simulator
{
    public class ObjectState
    {
        public string name;
        public string type;
        public Vector3 position;
        public float speed;
        public float degreesfromx;
        public Vector3 dimension;


        public ObjectState(string name, string type, float[] position,
        float[] orientation, float[] velocity, float[] dimension)
        {
            this.name = name; // for all objects
            this.type = type; //type is either 'robot', 'obstacle', 'food'
            // this.position = position;
            this.position.X = position[0]; //for all objects
            this.position.Y = position[1]; //for all objects
            this.position.Z = position[2]; //for all objects

            float x = orientation[0]; //  for robots and obstacles, if it's not a robot, just set orientation to [1,0,0]
            float z = orientation[2]; //radians = Math.Atan(z/x);
            double radians = Math.Atan(z / x);
            double angle = radians * (180 / Math.PI);

            this.degreesfromx = (float)angle;  //only for robots

            this.speed = (float)Math.Sqrt(Math.Pow(velocity[0], 2) + Math.Pow(velocity[2], 2)); //only for robots

            // this.dimension = dimension; //dimension is <x,y,z> (y is into the air, z is top down, x is left right),
            //specified only for obstacles, otherwise set it to [0,0,0]
            this.dimension.X = dimension[0];
            this.dimension.Y = dimension[1];
            this.dimension.Z = dimension[2];

        }
    }

}
