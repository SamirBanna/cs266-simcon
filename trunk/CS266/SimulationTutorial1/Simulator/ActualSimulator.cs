using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Robotics.Simulation.Physics;
using Microsoft.Robotics.PhysicalModel;
using Microsoft.Robotics.Simulation.Engine;
using CS266.SimCon.Controller;
using CS266.SimCon.RoboticsClasses;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using System.Net;

namespace CS266.SimCon.Simulator
{
    public class ActualSimulator
    {

        public List<IRobotCreate> RobotList;
        public ActualSimulator()
        {
            RobotList = new List<IRobotCreate>();
            CS266.SimCon.Controller.Program.Main();
        }

        public void PopulateWorld(WorldDimensions WD, WorldState World)
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

            // Orient sim camera view point
            this.SetupCamera(WD);

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

        #region CODECLIP 03-4
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
        #endregion

        public void AddBoundaries(float dimX, float dimY)
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
        }
    }


}
