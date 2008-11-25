//-----------------------------------------------------------------------
//  This file is part of the Microsoft Robotics Studio Code Samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  $File: SimulationTutorial1.cs $ $Revision: 1 $
//-----------------------------------------------------------------------

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
            base.Start();
            // Orient sim camera view point
            SetupCamera();
            // Add objects (entities) in our simulated world
            PopulateWorld();
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

        private void PopulateWorld()
        {
            AddSky();
            AddGround();
            AddBox(new Vector3(1, 1, 1));
            AddTexturedSphere(new Vector3(3, 1, 0));
        }

        #region CODECLIP 03-3
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
        void AddBox(Vector3 position)
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
            box.State.Name = "box";

            // Insert entity in simulation.  
            SimulationEngine.GlobalInstancePort.Insert(box);
        }
        #endregion

        #region CODECLIP 03-6
        void AddTexturedSphere(Vector3 position)
        {
            SingleShapeEntity entity = new SingleShapeEntity(
                new SphereShape(
                    new SphereShapeProperties(10, // mass in kg
                    new Pose(), // pose of shape within entity
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
