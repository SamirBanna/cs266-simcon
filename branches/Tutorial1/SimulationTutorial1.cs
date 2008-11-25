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
using System.Collections.Generic;

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
        [ServicePort("/SimulationTutorial1", AllowMultipleInstances=false)]
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
            string junk;
            base.Start();
            // Orient sim camera view point
            SetupCamera();
            // Add objects (entities) in our simulated world
            junk = PopulateWorld();
            Console.WriteLine(junk);
            
        }
        #endregion

        #region CODECLIP 03-2
        private void SetupCamera()
        {
            // Set up initial view
            CameraView view = new CameraView();
            view.EyePosition = new Vector3(-1.65f, 1.63f, -0.29f);
            view.LookAtPoint = new Vector3(-0.68f, 1.38f, -0.18f);
            SimulationEngine.GlobalInstancePort.Update(view);
        }
        #endregion

        private string PopulateWorld()
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

            //Takes in positions of the Robots from the camera. . . dynamically allocate memory
            IRobotCreate[] RobotArray = new IRobotCreate[5];
            string[] RobotNames = new string[5];
            //Takes the positions of all obstacles in the world space
            Vector3[] ObstaclePositionArray = new Vector3[4];
            string[] ObstacleNames = new string[4];

            RobotNames[0] = "A";
            RobotNames[1] = "B";
            RobotNames[2] = "C";
            RobotNames[3] = "D";
            RobotNames[4] = "E";
            
            ObstacleNames[0] = "box1";
            ObstacleNames[1] = "box2";
            ObstacleNames[2] = "box3";
            ObstacleNames[3] = "box4";

            AddSky();
            AddGround();
            //AddBoundaries();
            for (int i = 0; i < 5; i++)
            {
                //Places the robots in a position
                RobotArray[i] = new IRobotCreate(new Vector3(i, 0, 0));
                RobotArray[i].State.Name = RobotNames[i];
                //Inserts them into the world
                SimulationEngine.GlobalInstancePort.Insert(RobotArray[i]);
            }

            for (int j = 0; j < 4; j++)
            {
               // ObstaclePositionArray[j] = new Vector3(j, 0, 0);
               // AddBox(ObstaclePositionArray[j], ObstacleNames[j]);
            }

            //AddBox(new Vector3(1, 0, 0), "box");
            //AddBox(new Vector3(0, 1, 0), "box2");
            //AddBox(new Vector3(0, 0, 1), "box3");
            //AddBox(new Vector3(0, 0, 0), "box4");
            //AddTexturedSphere(new Vector3(3, 10, 0));
            //IRobotCreate iRobot = new IRobotCreate(new Vector3(2, 0, 2));
            //SimulationEngine.GlobalInstancePort.Insert(RobotArray[3]);
            //SimulationEngine.GlobalInstancePort.Insert(RobotArray[1]);

            //RobotArray[0].RotateDegrees(90, .5f);
            //RobotArray[0].DriveDistance(5, 2f);
            //RobotArray[1].DriveDistance(5, 2f);
            //RobotArray[2].DriveDistance(5, 2f);

            //Turn(RobotArray, 1, 90f);
            Forward(RobotArray, 2, 5);
            //RobotArray[1].RotateDegrees(45, .5f);
            Forward(RobotArray, 1, 5);
            //Backward(RobotArray, 2, 5);
            return "Hello";
        }

        #region CODECLIP 03-3
        void Forward(IRobotCreate [] RobotArray, int id, int distance)
        {
            RobotArray[id].DriveDistance(distance, 2f);
        }

        void Backward(IRobotCreate [] RobotArray, int id, int distance)
        {
            //Turn(RobotArray, id, 180);
            RobotArray[id].RotateDegrees(180, .5f);
            RobotArray[id].DriveDistance(distance, 2f);
        }

        void Turn(IRobotCreate [] RobotArray, int id, float degrees)
        {
            RobotArray[id].RotateDegrees(degrees, .5f);
        }

        void Speed(IRobotCreate [] RobotArray, int id, int num)
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
            // create a large horizontal plane, at zero elevation.
            HeightFieldEntity ground = new HeightFieldEntity(
                "simple ground", // name
                "03RamieSc.dds", // texture image
                new MaterialProperties("ground",
                    0.2f, // restitution
                    0.5f, // dynamic friction
                    0.5f) // static friction
                );
            SimulationEngine.GlobalInstancePort.Insert(ground);
        }
        #endregion

        #region CODECLIP 03-5
        void AddBox(Vector3 position, string name)
        {
            Vector3 dimensions = 
                new Vector3(0.2f, 0.2f, 0.2f); // meters

            // create simple movable entity, with a single shape
            SingleShapeEntity box = new SingleShapeEntity(
                new BoxShape(   
                    new BoxShapeProperties(
                    100, // mass in kilograms.
                    new Pose(), // relative pose
                    dimensions)), // dimensions
                position);

            box.State.MassDensity.Mass = 0;
            box.State.MassDensity.Density = 0;

            // Name the entity. All entities must have unique names
            box.State.Name = name;

            // Insert entity in simulation.  
            SimulationEngine.GlobalInstancePort.Insert(box);
        }

        void AddConfines(Vector3 position, string name)
        {
            Vector3 dimensions =
                new Vector3(.99f, .5f, .5f); // meters

            // create simple movable entity, with a single shape
            SingleShapeEntity box = new SingleShapeEntity(
                new BoxShape(
                    new BoxShapeProperties(
                    100, // mass in kilograms.
                    new Pose(position), // relative pose
                    dimensions)), // dimensions
                position);

            box.State.MassDensity.Mass = 0;
            box.State.MassDensity.Density = 0;

            // Name the entity. All entities must have unique names
            box.State.Name = name;

            // Insert entity in simulation.  
            SimulationEngine.GlobalInstancePort.Insert(box);
        }

        void AddBoundaries()
        {
            for (int i = -5; i < 6; i++)
            {
                string name = Convert.ToString(i);
                AddConfines(new Vector3(i, 0, 5), (name + "5"));
                //AddConfines(new Vector3(-5, 0, i), ("-5" + name));
                AddConfines(new Vector3(i, 0, -5), (name + "-5"));
                //AddConfines(new Vector3(5, 0, i), ("5" + name));
            }
        }
        #endregion

        #region CODECLIP 03-6
        void AddTexturedSphere(Vector3 position)
        {
            SingleShapeEntity entity = new SingleShapeEntity(
                new SphereShape(
                    new SphereShapeProperties(10, // mass in kg
                    new Pose(new Vector3(0, 0, 0)), // pose of shape within entity
                    1)), //default radius
                position);

            entity.State.Assets.Mesh = "earth.obj";
            entity.SphereShape.SphereState.Material = new MaterialProperties("sphereMaterial", 0.5f, 0.4f, 0.5f);

            // Name the entity
            entity.State.Name = "detailed sphere";

            // Insert entity in simulation.
            SimulationEngine.GlobalInstancePort.Insert(entity);
        }
        #endregion
    }

    public static class Contract
    {
        public const string Identifier = "http://schemas.tempuri.org/2006/06/simulationtutorial1.html";
    }

    [ServicePort]
    public class SimulationTutorial1Operations : PortSet<DsspDefaultLookup, DsspDefaultDrop>
    {
    }
}
