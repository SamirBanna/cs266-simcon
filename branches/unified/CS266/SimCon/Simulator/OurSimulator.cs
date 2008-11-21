using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Robotics.Simulation.Physics;
using Microsoft.Robotics.PhysicalModel;
using Microsoft.Robotics.Simulation.Engine;
using CS266.SimCon.Controller;
using CS266.SimCon.RoboticsClasses;

using System.Threading;
using System.Xml;
using System.Net;
using System.IO;

namespace CS266.SimCon.Simulator
{

    public class OurSimulator
    {
        //NEW!!!!!
        public List<SingleShapeEntity> BoundaryList;
        public List<IRobotCreate> RobotList;
        public List<SingleShapeEntity> FoodList;
        public HeightFieldEntity Ground;
        public GenAlgorithm Algo;
        private WorldState W;
        public int BatchMode;
        public TextReader tr;

        public OurSimulator()
        {
            BoundaryList = new List<SingleShapeEntity>();
            RobotList = new List<IRobotCreate>();
            FoodList = new List<SingleShapeEntity>();
        }

        public WorldState WorldStateGetter()
        {
            return W;
        }
        public WorldState GetWorldState()
        {
            List<ObjectState> objs = new List<ObjectState>();
            string name = "";
            string type = "obstacle";
            float[] position = new float[3];
            float[] orientation = new float[3];
            float[] velocity = new float[3];
            float[] dimension = new float[3];

            for (int i = 0; i < this.BoundaryList.Count; i++)
            {
                name = this.BoundaryList[i].State.Name;
                type = "obstacle";
                position[0] = this.BoundaryList[i].State.Pose.Position.X;
                position[1] = -this.BoundaryList[i].State.Pose.Position.Z;
                position[2] = this.BoundaryList[i].State.Pose.Position.Y;
                orientation[0] = this.BoundaryList[i].State.Pose.Orientation.X;
                orientation[1] = -this.BoundaryList[i].State.Pose.Orientation.Z;
                orientation[2] = this.BoundaryList[i].State.Pose.Orientation.Y;
                velocity[0] = this.BoundaryList[i].State.Velocity.X;
                velocity[1] = -this.BoundaryList[i].State.Velocity.Z;
                velocity[2] = this.BoundaryList[i].State.Velocity.Y;
                for (int j = 0; j < this.W.objects.Count; j++)
                {
                    if (name == this.W.objects[j].name)
                    {
                        dimension[0] = this.W.objects[j].dimension.X;
                        dimension[1] = this.W.objects[j].dimension.Y;
                        dimension[2] = this.W.objects[j].dimension.Z;
                    }
                }

                ObjectState o = new ObjectState(name, type, position, orientation, velocity, dimension);
                objs.Add(o);
            }

            for (int i = 0; i < this.RobotList.Count; i++)
            {
                name = this.RobotList[i].State.Name;
                type = "robot";
                position[0] = this.RobotList[i].State.Pose.Position.X;
                position[1] = -this.RobotList[i].State.Pose.Position.Z;
                position[2] = this.RobotList[i].State.Pose.Position.Y;
                orientation[0] = this.RobotList[i].State.Pose.Orientation.X;
                orientation[1] = -this.RobotList[i].State.Pose.Orientation.Z;
                orientation[2] = this.RobotList[i].State.Pose.Orientation.Y;
                velocity[0] = this.RobotList[i].State.Velocity.X;
                velocity[1] = -this.RobotList[i].State.Velocity.Z;
                velocity[2] = this.RobotList[i].State.Velocity.Y;
                dimension[0] = 1;
                dimension[1] = 1;
                dimension[2] = 1;

                ObjectState o = new ObjectState(name, type, position, orientation, velocity, dimension);
                objs.Add(o);
            }

            for (int i = 0; i < this.FoodList.Count; i++)
            {
                name = this.FoodList[i].State.Name;
                type = "robot";
                position[0] = this.FoodList[i].State.Pose.Position.X;
                position[1] = -this.FoodList[i].State.Pose.Position.Z;
                position[2] = this.FoodList[i].State.Pose.Position.Y;
                orientation[0] = this.FoodList[i].State.Pose.Orientation.X;
                orientation[1] = -this.FoodList[i].State.Pose.Orientation.Z;
                orientation[2] = this.FoodList[i].State.Pose.Orientation.Y;
                velocity[0] = this.FoodList[i].State.Velocity.X;
                velocity[1] = -this.FoodList[i].State.Velocity.Z;
                velocity[2] = this.FoodList[i].State.Velocity.Y;
                dimension[0] = 0.5f;
                dimension[1] = 0.4f;
                dimension[2] = 0.5f;

                ObjectState o = new ObjectState(name, type, position, orientation, velocity, dimension);
                objs.Add(o);
            }

            WorldState ws = new WorldState(objs);
            this.W = ws;

            // create an updated world state
            // set the simulator field

            return W;
        }

        //NEW!!!!! Changed!
        public void SetWorldState(WorldState World)
        {
            this.W = World;
        }

        //NEW!!!!! Changed!
        public void ExecuteActions(RobotActions ActionsVector)
        {
            this.W = this.RobotsAct(ActionsVector);
        }


        #region DETERMINE ALGO
        GenAlgorithm DetermineAlgo(string[] DelimitedParameters)
        {
            //Place this separate if-else in another function!
            if (DelimitedParameters[0] == "GenAlgorithm")
            {
                return new GenAlgorithm(
                    new float[] { (float)System.Convert.ToSingle(DelimitedParameters[1]), (float)System.Convert.ToSingle(DelimitedParameters[2]) },
                    new int[] { Int32.Parse(DelimitedParameters[3]), Int32.Parse(DelimitedParameters[4]) },
                    new int[] { Int32.Parse(DelimitedParameters[5]), Int32.Parse(DelimitedParameters[6]) },
                    new int[] { Int32.Parse(DelimitedParameters[7]), Int32.Parse(DelimitedParameters[8]) });

            }
            else if (DelimitedParameters[0] == "DFS")
            {
                return new GenAlgorithm(
                    new float[] { (float)System.Convert.ToSingle(DelimitedParameters[1]), (float)System.Convert.ToSingle(DelimitedParameters[2]) },
                    new int[] { Int32.Parse(DelimitedParameters[3]), Int32.Parse(DelimitedParameters[4]) },
                    new int[] { Int32.Parse(DelimitedParameters[5]), Int32.Parse(DelimitedParameters[6]) },
                    new int[] { Int32.Parse(DelimitedParameters[7]), Int32.Parse(DelimitedParameters[8]) },
                    (float)System.Convert.ToSingle(DelimitedParameters[9]));
            }
            else
            {
                return new GenAlgorithm(
                    new float[] { (float)System.Convert.ToSingle(DelimitedParameters[0]), (float)System.Convert.ToSingle(DelimitedParameters[1]) },
                    new int[] { Int32.Parse(DelimitedParameters[2]), Int32.Parse(DelimitedParameters[3]) },
                    new int[] { Int32.Parse(DelimitedParameters[4]), Int32.Parse(DelimitedParameters[5]) },
                    new int[] { Int32.Parse(DelimitedParameters[6]), Int32.Parse(DelimitedParameters[7]) });
            }
        }
        #endregion
        #region DELETE ENTITIES
        //NEW!!!!!!!
        void DeleteEntities(OurSimulator os)
        {
            //for (int i = 0; i < W.objects.Count; i++)
            //Timing is off!!!!
            for (int i = 0; i < os.BoundaryList.Count; i++)
            {
                SimulationEngine.GlobalInstancePort.Delete(os.BoundaryList[i]);
            }

            for (int i = 0; i < os.RobotList.Count; i++)
            {
                SimulationEngine.GlobalInstancePort.Delete(os.RobotList[i]);
            }

            for (int i = 0; i < os.FoodList.Count; i++)
            {
                SimulationEngine.GlobalInstancePort.Delete(os.FoodList[i]);
            }

            SimulationEngine.GlobalInstancePort.Delete(os.Ground);

            return;
        }
        #endregion
        #region RANDOM BOARD
        WorldPair RandomBoard(int[] robots, int[] barriers, int[] food, float[] dim)
        {
            Random rng = new Random();
            WorldPair MyWorld;
            List<ObjectState> o = new List<ObjectState>();

            //Make return a random float
            float xdim = dim[0];
            float ydim = dim[1];


            int total_objects = (int)(xdim - 2) * (int)(ydim - 2);
            //Make random number bounded by total_objects
            int num_robots = rng.Next(robots[0], robots[1]);

            for (int i = 0; i < num_robots; i++)
            {
                int position_x = rng.Next(1, (int)(xdim - 1));
                int position_y = rng.Next(1, (int)(ydim - 1));
                string name = "robot" + i.ToString();
                //Make have random location
                o.Add(new ObjectState(name, "robot", new float[3] { (float)position_x, (float)-position_y, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 }));
            }

            total_objects -= num_robots;
            //Make random number bounded by total_objects
            int num_obstacles = rng.Next(barriers[0], barriers[1]);

            for (int j = 0; j < num_obstacles; j++)
            {
                string name = "Obstacle" + j.ToString();


                //Make Random or standard?
                //Both vols were 2
                float x_vol = rng.Next(1, (int)(xdim - 1));
                float y_vol = rng.Next(1, (int)(ydim - 1));
                int position_x = rng.Next(1, (int)(xdim - 1));
                int position_y = rng.Next(1, (int)(ydim - 1));

                //Consider making z-oriented obstacles
                if (rng.NextDouble() < .5) //probability .5
                {
                    //Limits the width/length of the barrier so we do not collide with the boundaries
                    while ((x_vol / 2) + (float)position_x >= xdim || (float)position_x - (x_vol / 2) <= 0)
                    {
                        x_vol = rng.Next(1, (int)(xdim - 1));
                    }
                    //objects oriented along the x axis
                    o.Add(new ObjectState(name, "obstacle", new float[3] { (float)position_x, (float)-position_y, .5f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { x_vol, -.5f, .8f }));
                }
                else
                {
                    //Limits the width/length of the barrier so so we don't collide with the boundaries
                    while ((y_vol / 2) + (float)position_y >= ydim || (float)position_y - (y_vol / 2) <= 0)
                    {
                        y_vol = rng.Next(1, (int)(ydim - 1));
                    }
                    //objects oriented along the y axis
                    o.Add(new ObjectState(name, "obstacle", new float[3] { (float)position_x, (float)-position_y, .5f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { .5f, -y_vol, .8f }));
                }
            }

            total_objects -= num_obstacles;
            //Make random number bounded by total objects
            int num_food = rng.Next(food[0], food[1]);
            for (int k = 0; k < num_food; k++)
            {
                string name = "FoodUnit" + k.ToString();
                int position_x = rng.Next(1, (int)(xdim - 1));
                int position_y = rng.Next(1, (int)(ydim - 1));
                //make have random location
                o.Add(new ObjectState(name, "food", new float[3] { (float)position_x, (float)-position_y, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 }));
            }

            MyWorld = new WorldPair(new WorldDimensions(xdim, ydim), new WorldState(o));
            return MyWorld;

        }
        #endregion


        //NEW!!!!! Changed!
        public OurSimulator Finished()
        {
            //Decrement and run the next environment
            this.BatchMode--;

            //We are finished
            if (this.BatchMode < 1)
            {
                return null;
            }

            DeleteEntities(this);

            //Reads a line in the text file and parses the parameters
            string RawParameters = this.tr.ReadLine();
            string[] DelimitedParameters = RawParameters.Split(',');

            this.Algo = DetermineAlgo(DelimitedParameters);

            Console.WriteLine(this.BatchMode);

            WorldPair MyWorld = RandomBoard(this.Algo.robots, this.Algo.barriers, this.Algo.food, this.Algo.dim);
            Thread.Sleep(3000);
            Console.WriteLine("Not Sleeping anymore");

            this.SetWorldState(MyWorld.WS);

            this.PopulateWorld(MyWorld.WD, MyWorld.WS, this.Algo);

            return this;

        }

        //NEW!!!!!! Changed!
        public void PopulateWorld(WorldDimensions WD, WorldState World, GenAlgorithm Algo)
        {
            // Orient sim camera view point
            this.SetupCamera(WD);

            AddSky();
            AddGround(Algo.GridSquareSize);
            AddBoundaries(WD.xdim, WD.zdim);

            for (int i = 0; i < World.objects.Count; i++)
            {
                if (World.objects[i].type == "obstacle")
                {
                    AddObstacle(World.objects[i].dimension, World.objects[i].position, World.objects[i].name);
                }
                else if (World.objects[i].type == "robot")
                {
                    this.RobotList.Add(AddRobot(World.objects[i]));
                }
                else if (World.objects[i].type == "food")
                {
                    AddFood(World.objects[i].position, World.objects[i].name);
                }
            }
        }

        #region CODECLIP 03-2
        public void SetupCamera(WorldDimensions WD)
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

        #region CODECLIP 03-3

        public IRobotCreate AddRobot(ObjectState r)
        {
            Vector3 position = new Vector3(r.position.X, r.position.Z, -(r.position.Y));
            IRobotCreate Robot = new IRobotCreate(position); // Was r.position
            Robot.State.Name = r.name;
            SimulationEngine.GlobalInstancePort.Insert(Robot);
            return Robot;
        }

        public WorldState RobotsAct(RobotActions actions_vector)
        {
            //NEW!!!!!!!!!!
            if (actions_vector == null)
            {
                Thread.Sleep(15000);
                return null;
            }


            //Iterate over the robots and the actions 
            for (int i = 0; i < this.RobotList.Count; i++)
            {
                for (int j = 0; j < actions_vector.size; j++)
                {
                    //If the name of the robot and the particular action matchup
                    if (this.RobotList[i].State.Name == actions_vector.actions[j].name)
                    {
                        //Rotate by the stated number of degrees
                        this.RobotList[i].RotateDegrees(actions_vector.actions[j].newdegreesfromx, 10000000f);
                        Thread.Sleep(2200);

                        //Drive the correct distance
                        this.RobotList[i].DriveDistance(actions_vector.actions[j].distance, 2f);
                        break;
                    }
                }
            }
            //The Sleep function may be an issue!!!!!!!
            //Thread.Sleep(10000);
            WorldStateParser WP = new WorldStateParser("SimulationEngineState.xml");
            //WorldStateParser WP = new WorldStateParser("http://localhost:50000/simulationengine");
            WorldState WS = WP.parse();

            return WS;
        }

        public void Forward(IRobotCreate[] RobotArray, int id, int distance)
        {
            RobotArray[id].DriveDistance(distance, 2f);
        }

        public void Backward(IRobotCreate[] RobotArray, int id, int distance)
        {
            Turn(RobotArray, id, -180f);
            RobotArray[id].DriveDistance(distance, 2f);
        }

        public void Turn(IRobotCreate[] RobotArray, int id, float degrees)
        {
            RobotArray[id].RotateDegrees(degrees, 10000000f);
            Thread.Sleep(2200);
        }

        public void Speed(IRobotCreate[] RobotArray, int id, int num)
        {
            RobotArray[id].DriveDistance(2, num);
        }

        public void AddSky()
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

        #region ADD GROUND FUNCTIONS
        public void AddGround()
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

        public void AddGround(float WidthLength)
        {
            HeightFieldShapeProperties hf = new HeightFieldShapeProperties("height field",
               100, // number of rows 
               WidthLength, // distance in meters, between rows
               100, // number of columns
               WidthLength, // distance in meters, between columns
               1, // scale factor to multiple height values 
               -1000); // vertical extent of the height field. Should be set to large negative values

            // create a material for the entire field. We could also specify material per sample.
            hf.Material = new MaterialProperties("ground", 0.2f, 0.5f, 0.5f);
            hf.Dimensions = new Vector3(5f, 5f, 5f);
            // insert ground entity in simulation and specify a texture
            HeightFieldEntity ground = new HeightFieldEntity(hf, "03RamieSc.dds");
            this.Ground = ground;
            SimulationEngine.GlobalInstancePort.Insert(ground);
        }
        #endregion

        #region CODECLIP 03-5
        public void AddBoundaries(float dimX, float dimY)
        {
            //Right Wall
            Vector3 dimensions = new Vector3(.5f, .5f, dimY);
            Vector3 position = new Vector3(dimX, .5f, (dimY / 2f));
            BoxShapeProperties tBoxShape = new BoxShapeProperties(10000000f, new Pose(), dimensions);
            tBoxShape.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity tBoxEntity = new SingleShapeEntity(new BoxShape(tBoxShape), position); ;
            tBoxEntity.State.Assets.DefaultTexture = "env2.bmp";
            tBoxEntity.State.Name = "Wall";

            //Top Wall
            Vector3 dimensions2 = new Vector3(dimX - 1, .5f, .5f);
            Vector3 position2 = new Vector3((dimX / 2f), .5f, 0);
            BoxShapeProperties tBoxShape2 = new BoxShapeProperties(10000000f, new Pose(), dimensions2);
            tBoxShape2.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape2.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity tBoxEntity2 = new SingleShapeEntity(new BoxShape(tBoxShape2), position2); ;
            tBoxEntity2.State.Assets.DefaultTexture = "env2.bmp";
            tBoxEntity2.State.Name = "Wall2";

            //Bottom Wall
            Vector3 dimensions3 = new Vector3(dimX - 1, .5f, .5f);
            Vector3 position3 = new Vector3((dimX / 2f), .5f, dimY);
            BoxShapeProperties tBoxShape3 = new BoxShapeProperties(10000000f, new Pose(), dimensions3);
            tBoxShape3.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape3.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity tBoxEntity3 = new SingleShapeEntity(new BoxShape(tBoxShape3), position3); ;
            tBoxEntity3.State.Assets.DefaultTexture = "env2.bmp";
            tBoxEntity3.State.Name = "Wall3";

            //Left Wall
            Vector3 dimensions4 = new Vector3(.5f, .5f, dimY);
            Vector3 position4 = new Vector3(0, .5f, (dimY / 2f));
            BoxShapeProperties tBoxShape4 = new BoxShapeProperties(10000000f, new Pose(), dimensions4);
            tBoxShape4.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape4.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity tBoxEntity4 = new SingleShapeEntity(new BoxShape(tBoxShape4), position4); ;
            tBoxEntity4.State.Assets.DefaultTexture = "env2.bmp";
            tBoxEntity4.State.Name = "Wall4";

            SimulationEngine.GlobalInstancePort.Insert(tBoxEntity);
            SimulationEngine.GlobalInstancePort.Insert(tBoxEntity2);
            SimulationEngine.GlobalInstancePort.Insert(tBoxEntity3);
            SimulationEngine.GlobalInstancePort.Insert(tBoxEntity4);

            this.BoundaryList.Add(tBoxEntity);
            this.BoundaryList.Add(tBoxEntity2);
            this.BoundaryList.Add(tBoxEntity3);
            this.BoundaryList.Add(tBoxEntity4);

        }

        public void AddFood(Vector3 position_passed, string name)
        {
            Vector3 position = new Vector3(position_passed.X, position_passed.Z, -(position_passed.Y));
            SingleShapeEntity food = new SingleShapeEntity(
               new SphereShape(
                  new SphereShapeProperties(5, // mass in kg
                  new Pose(), // pose of shape within entity
                  .1f)), //default radius
               position);

            food.SphereShape.SphereState.Material =
               new MaterialProperties("sphereMaterial", 0.5f, 0.4f, 0.5f);

            // Name the entity
            food.State.Name = name;

            // Insert entity in simulation.
            SimulationEngine.GlobalInstancePort.Insert(food);
            this.FoodList.Add(food);
        }



        public void AddObstacle(Vector3 dimension_passed, Vector3 position_passed, string name)
        {
            Vector3 position = new Vector3(position_passed.X, position_passed.Z, -(position_passed.Y));
            Vector3 dimension = new Vector3(dimension_passed.X, dimension_passed.Z, -(dimension_passed.Y));
            BoxShapeProperties tBoxShape = new BoxShapeProperties(10000f, new Pose(), dimension);
            tBoxShape.Material = new MaterialProperties("tbox", 0.4f, 0.4f, 0.6f);
            tBoxShape.DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            SingleShapeEntity Obstacle = new SingleShapeEntity(new BoxShape(tBoxShape), position); ;
            Obstacle.State.Assets.DefaultTexture = "env2.bmp";
            Obstacle.State.Name = name;

            SimulationEngine.GlobalInstancePort.Insert(Obstacle);
            this.BoundaryList.Add(Obstacle);
        }
    }

        #endregion
    #region ALGORITHM PARAMETERS CLASS
    public class GenAlgorithm
    {
        public int[] robots = new int[2];
        public int[] barriers = new int[2];
        public int[] food = new int[2];
        public float[] dim = new float[2];
        public float GridSquareSize;

        public GenAlgorithm(float[] d, int[] r, int[] b, int[] f)
        {
            this.dim = d;
            this.robots = r;
            this.barriers = b;
            this.food = f;
            this.GridSquareSize = 1;
        }

        public GenAlgorithm(float[] d, int[] r, int[] b, int[] f, float g)
        {
            this.dim = d;
            this.robots = r;
            this.barriers = b;
            this.food = f;
            this.GridSquareSize = g;
        }
    }

    public class DFS
    {
        public int[] robots = new int[2];
        public int[] barriers = new int[2];
        public int[] food = new int[2];
        public float[] dim = new float[2];
        float GridSquareSize;

        public DFS(float[] d, int[] r, int[] b, int[] f, float g)
        {
            this.dim = d;
            this.robots = r;
            this.barriers = b;
            this.food = f;
            this.GridSquareSize = g;
        }
    }
    #endregion


}

