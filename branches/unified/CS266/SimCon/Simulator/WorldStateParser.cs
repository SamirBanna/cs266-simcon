using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Xml;

using CS266.SimCon.RoboticsClasses;

namespace CS266.SimCon.Simulator
{
    public class WorldStateParser
    {
        public string file;
        public WorldStateParser(string file)
        {
            this.file = file;
        }
        public WorldState parse()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.file);

            XmlNodeList entities = doc.GetElementsByTagName("SerializedEntities")[0].ChildNodes;
            IEnumerator ienum = entities.GetEnumerator();

            List<ObjectState> objects = new List<ObjectState>();

            int barriers = 0;

            while (ienum.MoveNext())
            {
                XmlNode entity = (XmlNode)ienum.Current;
                if (entity.Name == "IRobotCreate")
                {
                    XmlNodeList state = entity.FirstChild.ChildNodes;
                    XmlNode n = state[0];
                    string name = n.InnerText;
                    //Console.WriteLine(name);
                    string type = "robot";
                    //Console.WriteLine(type);
                    float[] position = new float[3];
                    float[] orientation = new float[3];
                    float[] velocity = new float[3];
                    float[] dimension = new float[3];
                    dimension[0] = 0;
                    dimension[1] = 0;
                    dimension[2] = 0;

                    XmlNodeList pose = state[2].ChildNodes;

                    XmlNodeList p = pose[0].ChildNodes;
                    position[0] = float.Parse(p[0].InnerText);
                    //Console.WriteLine(p[0].InnerText);
                    position[1] = -float.Parse(p[2].InnerText);
                    //Console.WriteLine(p[2].InnerText);
                    position[2] = float.Parse(p[1].InnerText);
                    //Console.WriteLine(p[1].InnerText);

                    XmlNodeList o = pose[1].ChildNodes;
                    orientation[0] = float.Parse(o[0].InnerText);
                    //Console.WriteLine(o[1].InnerText);
                    orientation[1] = -float.Parse(o[2].InnerText);
                    //Console.WriteLine(o[2].InnerText);
                    orientation[2] = float.Parse(o[1].InnerText);
                    //Console.WriteLine(o[1].InnerText);

                    XmlNodeList v = state[3].ChildNodes;
                    velocity[0] = float.Parse(v[0].InnerText);
                    //Console.WriteLine(v[0].InnerText);
                    velocity[1] = -float.Parse(v[2].InnerText);
                    //Console.WriteLine(v[2].InnerText);
                    velocity[2] = float.Parse(v[1].InnerText);
                    //Console.WriteLine(v[1].InnerText);

                    ObjectState os = new ObjectState(name, type, position, orientation, velocity, dimension);
                    objects.Add(os);
                }
                //else if (entity.Name == "ENTER ENTITY NAME HERE")
                //{
                //    XmlNodeList state = entity.FirstChild.ChildNodes;
                //    XmlNode n = state[0];
                //    string name = n.InnerText;
                //    //Console.WriteLine(name);
                //    string type = "robot";
                //    //Console.WriteLine(type);
                //    float[] position = new float[3];
                //    float[] orientation = new float[3];
                //    float[] velocity = new float[3];
                //    float[] dimension = new float[3];
                //    dimension[0] = 0;
                //    dimension[1] = 0;
                //    dimension[2] = 0;

                //    XmlNodeList pose = state[2].ChildNodes;

                //    XmlNodeList p = pose[0].ChildNodes;
                //    position[0] = float.Parse(p[0].InnerText);
                //    //Console.WriteLine(p[0].InnerText);
                //    position[1] = -float.Parse(p[2].InnerText);
                //    //Console.WriteLine(p[2].InnerText);
                //    position[2] = float.Parse(p[1].InnerText);
                //    //Console.WriteLine(p[1].InnerText);

                //    XmlNodeList o = pose[1].ChildNodes;
                //    orientation[0] = float.Parse(o[0].InnerText);
                //    //Console.WriteLine(o[1].InnerText);
                //    orientation[1] = -float.Parse(o[2].InnerText);
                //    //Console.WriteLine(o[2].InnerText);
                //    orientation[2] = float.Parse(o[1].InnerText);
                //    //Console.WriteLine(o[1].InnerText);

                //    XmlNodeList v = state[3].ChildNodes;
                //    velocity[0] = float.Parse(v[0].InnerText);
                //    //Console.WriteLine(v[0].InnerText);
                //    velocity[1] = -float.Parse(v[2].InnerText);
                //    //Console.WriteLine(v[2].InnerText);
                //    velocity[2] = float.Parse(v[1].InnerText);
                //    //Console.WriteLine(v[1].InnerText);

                //    ObjectState os = new ObjectState(name, type, position, orientation, velocity, dimension);
                //    objects.Add(os);
                //}
                else if (entity.Name == "SingleShapeEntity")
                {
                    if (barriers == 4)
                    {
                        XmlNodeList state = entity.FirstChild.ChildNodes;
                        XmlNode n = state[0];
                        string name = n.InnerText;
                        //Console.WriteLine(name);
                        string type = "obstacle";
                        //Console.WriteLine(type);
                        float[] position = new float[3];
                        float[] orientation = new float[3];
                        float[] velocity = new float[3];
                        float[] dimension = new float[3];
                        dimension[0] = 0;
                        dimension[1] = 0;
                        dimension[2] = 0;

                        XmlNodeList pose = state[2].ChildNodes;

                        XmlNodeList p = pose[0].ChildNodes;
                        position[0] = float.Parse(p[0].InnerText);
                        //Console.WriteLine(p[0].InnerText);
                        position[1] = -float.Parse(p[2].InnerText);
                        //Console.WriteLine(p[2].InnerText);
                        position[2] = float.Parse(p[1].InnerText);
                        //Console.WriteLine(p[1].InnerText);

                        XmlNodeList o = pose[1].ChildNodes;
                        orientation[0] = float.Parse(o[0].InnerText);
                        //Console.WriteLine(o[1].InnerText);
                        orientation[1] = -float.Parse(o[2].InnerText);
                        //Console.WriteLine(o[2].InnerText);
                        orientation[2] = float.Parse(o[1].InnerText);
                        //Console.WriteLine(o[1].InnerText);

                        XmlNodeList v = state[3].ChildNodes;
                        velocity[0] = float.Parse(v[0].InnerText);
                        //Console.WriteLine(v[0].InnerText);
                        velocity[1] = -float.Parse(v[2].InnerText);
                        //Console.WriteLine(v[2].InnerText);
                        velocity[2] = float.Parse(v[1].InnerText);
                        //Console.WriteLine(v[1].InnerText);

                        ObjectState os = new ObjectState(name, type, position, orientation, velocity, dimension);
                        objects.Add(os);
                    }
                    else
                    {
                        barriers++;
                    }
                }
            }

            WorldState w = new WorldState(objects);
            return w;
        }
    }
}
