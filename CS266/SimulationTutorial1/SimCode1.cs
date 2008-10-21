//-----------------------------------------------------------------------
//  This file is part of the Microsoft Robotics Studio Code Samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  $File: SimulationTutorial1.cs $ $Revision: 1 $
//-----------------------------------------------------------------------
//#define IMMOVABLE 100000000000

#region CODECLIP 01-1
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

#region Simulation namespaces
using Microsoft.Robotics.Simulation;
using Microsoft.Robotics.Simulation.Engine;
using engineproxy = Microsoft.Robotics.Simulation.Engine.Proxy;
using Microsoft.Robotics.Simulation.Physics;
using Microsoft.Robotics.PhysicalModel;
using System.ComponentModel;
#endregion
#endregion

namespace Robotics.SimulationTutorial1
{
    #region CODECLIP 02-1
    [DisplayName("Simulation Tutorial 1")]
    [Description("Simulation Tutorial 1 Service")]
    [Contract(Contract.Identifier)]

    public class SimulationTutorial1 : DsspServiceBase
    {
        [Partner("Engine",
            Contract = engineproxy.Contract.Identifier,
            CreationPolicy = PartnerCreationPolicy.UseExistingOrCreate)]
        private engineproxy.SimulationEnginePort _engineStub =
            new engineproxy.SimulationEnginePort();

        // Main service port
        [ServicePort("/SimulationTutorial1", AllowMultipleInstances = false)]
        private SimulationTutorial1Operations _mainPort =
            new SimulationTutorial1Operations();

        public SimulationTutorial1(DsspServiceCreationPort creationPort) :
            base(creationPort)
        {

        }
    #endregion

        #region CODECLIP 03-1
        protected override void Start()
        {
            
            base.Start();
            List<IRobotCreate> RobotList;
            WorldDimensions WD = new WorldDimensions(14, 14);
            RobotActions Actions = new RobotActions(new RobotAction[4] {
                new RobotAction("A", 90f, 9), 
                new RobotAction("B", 45f, 5),
                new RobotAction("C", 0f, 9), 
                new RobotAction("D", 0f, 9)});
            ObjectState Barrier = new ObjectState("Obstacle1", "obstacle", new float[3] { 2f, 0, WD.zdim / 2f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { .5f, .8f, 6f });
            ObjectState Barrier2 = new ObjectState("Obstacle2", "obstacle", new float[3] { 4f, 0, ((WD.zdim / 2f) + 3f) }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 3f, .8f, .5f });
            ObjectState Robot1 = new ObjectState("A", "robot", new float[3] { (WD.xdim / 2f), 0, WD.zdim / 2f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Robot2 = new ObjectState("B", "robot", new float[3] { (WD.xdim / 2f) + 1, 0, WD.zdim / 2f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Robot3 = new ObjectState("C", "robot", new float[3] { (WD.xdim / 2f) + 2, 0, WD.zdim / 2f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Robot4 = new ObjectState("D", "robot", new float[3] { (WD.xdim / 2f) + 3, 0, WD.zdim / 2f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Robot5 = new ObjectState("E", "robot", new float[3] { (WD.xdim / 2f) + 4, 0, WD.zdim / 2f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            List<ObjectState> o = new List<ObjectState>(7);
            
            o.Add(Barrier);
            o.Add(Barrier2);
            o.Add(Robot1);
            o.Add(Robot2);
            o.Add(Robot3);
            o.Add(Robot4);
            o.Add(Robot5);

            WorldState World = new WorldState(o);
         
            // Orient sim camera view point
            SetupCamera(WD);
            // Add objects (entities) in our simulated world
            RobotList = PopulateWorld(WD, World);
            WorldState W = RobotsAct(RobotList, Actions);

        }
        #endregion

        #region CODECLIP 03-2
        private void SetupCamera(WorldDimensions WD)
        {
            float maxDim;
            if (WD.xdim > WD.zdim)
            {
                maxDim = WD.xdim * 1.25f;
            }
            else
            {
                maxDim = WD.zdim * 1.25f;
            }
            // Set up initial view
            CameraView view = new CameraView();
            view.EyePosition = new Vector3(WD.xdim / 2f, maxDim, WD.zdim / 2f);
            view.LookAtPoint = new Vector3(WD.xdim / 2f, maxDim - 2f, WD.zdim / 2f);
            //view.EyePosition = new Vector3(-1.65f, 1.63f, -0.29f);
            //view.LookAtPoint = new Vector3(-0.68f, 1.38f, -0.18f);
            SimulationEngine.GlobalInstancePort.Update(view);
        }
        #endregion

        private List<IRobotCreate> PopulateWorld(WorldDimensions WD, WorldState World)
        {
            //Priorities: 1. Figure out what version 1 world state spec (us, controller and camera) 
            //            2. Robot and controller team standardize actions
            //            3. Interfaces for robot and world state specification
            //            4. Communication and messages
            //             Order: Modeling the ????, closing the loop, FOOD, OBSTACLES, (brain state and our robot)
            //            5. E-mail SWAPNA a vector of names and XML file
            //
            //
            //Need: Vector of positions for robots, vector of positions for obstacles, vecotr of unique names for each
            //Vector of actions, Vector of orientations


            List<IRobotCreate> RobotList = new List<IRobotCreate>();
            AddSky();
            AddGround();
            AddBoundaries(WD.xdim, WD.zdim);

            for (int i = 0; i < World.objects.Count; i++)
            {
                if (World.objects[i].type == "obstacle")
                {
                    AddObstacle(World.objects[i].dimension, World.objects[i].position, World.objects[i].name);
                }
                else if (World.objects[i].type == "robot")
                {
                    RobotList.Add(AddRobot(World.objects[i]));
                }
            }

           
            return RobotList;
        }

        #region CODECLIP 03-3

        public IRobotCreate AddRobot(ObjectState r)
        {
            IRobotCreate Robot = new IRobotCreate(r.position);
            Robot.State.Name = r.name;
            SimulationEngine.GlobalInstancePort.Insert(Robot);
            return Robot;
        }

        WorldState RobotsAct(List<IRobotCreate> robots, RobotActions actions_vector)
        {
            //Iterate over the robots and the actions 
            for(int i = 0; i < robots.Count; i++)
            {
                for(int j = 0; j < actions_vector.size; j++)
                {
                    //If the name of the robot and the particular action matchup
                    if (robots[i].State.Name == actions_vector.actions[j].name)
                    {
                        //Rotate by the stated number of degrees
                        robots[i].RotateDegrees(actions_vector.actions[j].newdegreesfromx, 10000000f);
                        Thread.Sleep(2200);
                    
                        //Drive the correct distance
                        robots[i].DriveDistance(actions_vector.actions[j].distance, 2f);
                        break;
                    }
                }
            }
            //The Sleep function may be an issue!!!!!!!
            //Thread.Sleep(10000);
            WorldStateParser WP = new WorldStateParser("SimulationEngineState.xml");
            WorldState WS = WP.parse();
            
            return WS;
         }

        void Forward(IRobotCreate[] RobotArray, int id, int distance)
        {
            RobotArray[id].DriveDistance(distance, 2f);
        }

        void Backward(IRobotCreate[] RobotArray, int id, int distance)
        {
            Turn(RobotArray, id, -180f);
            RobotArray[id].DriveDistance(distance, 2f);
        }

        void Turn(IRobotCreate[] RobotArray, int id, float degrees)
        {
            RobotArray[id].RotateDegrees(degrees, 10000000f);
            Thread.Sleep(2200);
        }

        void Speed(IRobotCreate[] RobotArray, int id, int num)
        {
            RobotArray[id].DriveDistance(2, num);
        }

        void AddSky()
        {
            // Add a sky using a static texture. We will use the sky texture
            // to do per pixel lighting on each simulation visual entity
            SkyDomeEntity sky = new SkyDomeEntity("skydome.dds", "sky_diff.dds");
            SimulationEngine.GlobalInstancePort.Insert(sky);

            // Add a directional light to simulate the sun.
            LightSourceEntity sun = new LightSourceEntity();
            sun.State.Name = "Sun";
            sun.Type = LightSourceEntityType.Directional;
            sun.Color = new Vector4(0.8f, 0.8f, 0.8f, 1);
            sun.Direction = new Vector3(0.5f, -.75f, 0.5f);
            SimulationEngine.GlobalInstancePort.Insert(sun);
        }
        #endregion

        #region CODECLIP 03-4
        void AddGround()
        {
            HeightFieldShapeProperties hf = new HeightFieldShapeProperties("height field",
               100, // number of rows 
               1, // distance in meters, between rows
               100, // number of columns
               1, // distance in meters, between columns
               1, // scale factor to multiple height values 
               -1000); // vertical extent of the height field. Should be set to large negative values

            // create a material for the entire field. We could also specify material per sample.
            hf.Material = new MaterialProperties("ground", 0.2f, 0.5f, 0.5f);
            hf.Dimensions = new Vector3(5f, 5f, 5f);
            // insert ground entity in simulation and specify a texture
            SimulationEngine.GlobalInstancePort.Insert(new HeightFieldEntity(hf, "03RamieSc.dds"));
        }
        #endregion

        #region CODECLIP 03-5
        void AddBoundaries(float dimX, float dimY)
        {
            //Right Wall
            Vector3 dimensions = new Vector3(.5f, .5f, dimY);
            Vector3 position = new Vector3(dimX, .5f, (dimY / 2f));
            BoxShapeProperties tBoxShape = new BoxShapeProperties(10000f, new Pose(), dimensions);
            tBoxShape.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity tBoxEntity = new SingleShapeEntity(new BoxShape(tBoxShape), position); ;
            tBoxEntity.State.Assets.DefaultTexture = "env2.bmp";
            tBoxEntity.State.Name = "Wall";

            //Top Wall
            Vector3 dimensions2 = new Vector3(dimX - 1, .5f, .5f);
            Vector3 position2 = new Vector3((dimX / 2f), .5f, 0);
            BoxShapeProperties tBoxShape2 = new BoxShapeProperties(10000f, new Pose(), dimensions2);
            tBoxShape2.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape2.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity tBoxEntity2 = new SingleShapeEntity(new BoxShape(tBoxShape2), position2); ;
            tBoxEntity2.State.Assets.DefaultTexture = "env2.bmp";
            tBoxEntity2.State.Name = "Wall2";

            //Bottom Wall
            Vector3 dimensions3 = new Vector3(dimX - 1, .5f, .5f);
            Vector3 position3 = new Vector3((dimX / 2f), .5f, dimY);
            BoxShapeProperties tBoxShape3 = new BoxShapeProperties(10000f, new Pose(), dimensions3);
            tBoxShape3.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape3.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity tBoxEntity3 = new SingleShapeEntity(new BoxShape(tBoxShape3), position3); ;
            tBoxEntity3.State.Assets.DefaultTexture = "env2.bmp";
            tBoxEntity3.State.Name = "Wall3";

            //Left Wall
            Vector3 dimensions4 = new Vector3(.5f, .5f, dimY);
            Vector3 position4 = new Vector3(0, .5f, (dimY / 2f));
            BoxShapeProperties tBoxShape4 = new BoxShapeProperties(10000f, new Pose(), dimensions4);
            tBoxShape4.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape4.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity tBoxEntity4 = new SingleShapeEntity(new BoxShape(tBoxShape4), position4); ;
            tBoxEntity4.State.Assets.DefaultTexture = "env2.bmp";
            tBoxEntity4.State.Name = "Wall4";

            SimulationEngine.GlobalInstancePort.Insert(tBoxEntity);
            SimulationEngine.GlobalInstancePort.Insert(tBoxEntity2);
            SimulationEngine.GlobalInstancePort.Insert(tBoxEntity3);
            SimulationEngine.GlobalInstancePort.Insert(tBoxEntity4);

        }

        void AddObstacle(Vector3 dimension, Vector3 position, string name)
        {
            BoxShapeProperties tBoxShape = new BoxShapeProperties(10000f, new Pose(), dimension);
            tBoxShape.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity Obstacle = new SingleShapeEntity(new BoxShape(tBoxShape), position); ;
            Obstacle.State.Assets.DefaultTexture = "env2.bmp";
            Obstacle.State.Name = name;

            SimulationEngine.GlobalInstancePort.Insert(Obstacle);
        }

        #endregion

        #region CODECLIP 03-6
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

                float x = orientation[0]; // only for robots, if it's not a robot, just set orientation to [1,0,0]
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

            //public ObjectState(string name, string type, Vector3 position, Vector3 dimension)
            //{
            //    this.name = name; // for all objects
            //    this.type = type; //type is either 'robot', 'obstacle', 'food'
            //    this.position = position; //for all objects
            //    this.dimension = dimension; //dimension is <x,y,z> (y is into the air, z is top down, x is left right),
            //    specified only for obstacles, otherwise set it to [0,0,0]

            //}
        }

        public class WorldState
        {
            public List<ObjectState> objects;
            public WorldState(List<ObjectState> objects)
            {
                this.objects = objects;
            }
        }


        public class WorldDimensions
        {

            public float xdim;
            public float zdim;

            public WorldDimensions(float xdim, float zdim)
            {
                this.xdim = xdim;
                this.zdim = zdim;
            }

        }

        public class RobotAction
        {
            public string name;
            public float newdegreesfromx;
            public float distance;
            public RobotAction(string name, float newdegreesfromx, float distance)
            {
                this.name = name;
                this.newdegreesfromx = newdegreesfromx;
                this.distance = distance;
            }
        }

        public class RobotActions
        {
            public RobotAction[] actions;
            public int size;
            public RobotActions(RobotAction[] acts)
            {
                this.actions = acts;
                this.size = acts.Length;
            }
        }
        // DID HAVE A "}" HERE
        #endregion

        #region CODE-PARSER
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

                while (ienum.MoveNext())
                {
                    XmlNode entity = (XmlNode)ienum.Current;
                    if (entity.Name == "IRobotCreate")
                    {
                        XmlNodeList state = entity.FirstChild.ChildNodes;
                        XmlNode n = state[0];
                        string name = n.InnerText;
                        Console.WriteLine(name);
                        string type = "robot";
                        Console.WriteLine(type);
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
                        Console.WriteLine(p[0].InnerText);
                        position[1] = float.Parse(p[1].InnerText);
                        Console.WriteLine(p[1].InnerText);
                        position[2] = float.Parse(p[2].InnerText);
                        Console.WriteLine(p[2].InnerText);

                        XmlNodeList o = pose[1].ChildNodes;
                        orientation[0] = float.Parse(o[0].InnerText);
                        Console.WriteLine(o[0].InnerText);
                        orientation[1] = float.Parse(o[1].InnerText);
                        Console.WriteLine(o[1].InnerText);
                        orientation[2] = float.Parse(o[2].InnerText);
                        Console.WriteLine(o[2].InnerText);

                        XmlNodeList v = state[3].ChildNodes;
                        velocity[0] = float.Parse(v[0].InnerText);
                        Console.WriteLine(v[0].InnerText);
                        velocity[1] = float.Parse(v[1].InnerText);
                        Console.WriteLine(v[1].InnerText);
                        velocity[2] = float.Parse(v[2].InnerText);
                        Console.WriteLine(v[2].InnerText);

                        ObjectState os = new ObjectState(name, type, position, orientation, velocity, dimension);
                        objects.Add(os);
                    }
                }

                WorldState w = new WorldState(objects);
                return w;
            }
        }
        #endregion


    } //NEW ONE ADDED HERE!!!

    public static class Contract
    {
        public const string Identifier = "http://schemas.tempuri.org/2006/06/simulationtutorial1.html";
    }

    [ServicePort]
    public class SimulationTutorial1Operations : PortSet<DsspDefaultLookup, DsspDefaultDrop>
    {
    }
}
