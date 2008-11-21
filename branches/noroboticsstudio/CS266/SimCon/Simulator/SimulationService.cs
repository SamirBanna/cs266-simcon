//-----------------------------------------------------------------------
//
//  $File: SimulationService.cs $ $Revision: 1 $
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
using System.Net;

using CS266.SimCon;
using CS266.SimCon.RoboticsClasses;

#region Simulation namespaces
using Microsoft.Robotics.Simulation;
using Microsoft.Robotics.Simulation.Engine;
using engineproxy = Microsoft.Robotics.Simulation.Engine.Proxy;
using Microsoft.Robotics.Simulation.Physics;
using Microsoft.Robotics.PhysicalModel;
using System.ComponentModel;
#endregion
#endregion

namespace CS266.SimCon.Simulator
{
    [DisplayName("CS266 Simulation Service")]
    [Description("CS266 Simulation Service")]
    [Contract(Contract.Identifier)]
    public class SimulationService : DsspServiceBase
    {
        [Partner("Engine",
            Contract = engineproxy.Contract.Identifier,
            CreationPolicy = PartnerCreationPolicy.UseExistingOrCreate)]
        private engineproxy.SimulationEnginePort _engineStub =
            new engineproxy.SimulationEnginePort();

        // Main service port
        [ServicePort("/SimulationService", AllowMultipleInstances = false)]
        private SimulationTutorial1Operations _mainPort =
            new SimulationTutorial1Operations();

        public SimulationService(DsspServiceCreationPort creationPort) :
            base(creationPort)
        {

        }

        
        protected override void Start()
        {

            base.Start();

            WorldDimensions WD = new WorldDimensions(14, 14);

            ObjectState Barrier = new ObjectState("Obstacle1", "obstacle", new float[3] { 2f, -WD.zdim / 2f, .5f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { .5f, -6f, .8f });
            //To make a diagonla object leave the y field as 0 otherwise use .5f as a dummy
            ObjectState Barrier2 = new ObjectState("Obstacle2", "obstacle", new float[3] { 4f, -((WD.zdim / 2f) + 4f), 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 3.5f, -.5f, .8f });
            ObjectState Barrier3 = new ObjectState("Obstacle3", "obstacle", new float[3] { 11f, -((WD.zdim / 2f) - 4f), .5f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 3f, -.5f, .8f });

            ObjectState Food1 = new ObjectState("FoodUnit1", "food", new float[3] { 10f, -10f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Food2 = new ObjectState("FoodUnit2", "food", new float[3] { 10f, -10.2f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Food3 = new ObjectState("FoodUnit3", "food", new float[3] { 10.2f, -10f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Food4 = new ObjectState("FoodUnit4", "food", new float[3] { 9.8f, -10f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Food5 = new ObjectState("FoodUnit5", "food", new float[3] { 10f, -9.8f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Food6 = new ObjectState("FoodUnit6", "food", new float[3] { 10f, -10f, .2f }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Food7 = new ObjectState("FoodUnit7", "food", new float[3] { 9.8f, -9.8f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });

            ObjectState Robot1 = new ObjectState("A", "robot", new float[3] { (WD.xdim / 2f), -WD.zdim / 2f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Robot2 = new ObjectState("B", "robot", new float[3] { (WD.xdim / 2f) + 1, -WD.zdim / 2f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Robot3 = new ObjectState("C", "robot", new float[3] { (WD.xdim / 2f) + 2, -WD.zdim / 2f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Robot4 = new ObjectState("D", "robot", new float[3] { (WD.xdim / 2f) + 3, -WD.zdim / 2f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });
            ObjectState Robot5 = new ObjectState("E", "robot", new float[3] { (WD.xdim / 2f) + 4, -WD.zdim / 2f, 0 }, new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 0 }, new float[3] { 0, 0, 0 });

            List<ObjectState> o = new List<ObjectState>();

            o.Add(Barrier);
            o.Add(Barrier2);
            o.Add(Barrier3);
            o.Add(Food1);
            o.Add(Food2);
            o.Add(Food3);
            o.Add(Food4);
            o.Add(Food5);
            o.Add(Food6);
            o.Add(Food7);
            o.Add(Robot1);
            o.Add(Robot2);
            o.Add(Robot3);
            o.Add(Robot4);
            o.Add(Robot5);

            WorldState World = new WorldState(o);
            ActualSimulator os = new ActualSimulator();

            // Add objects (entities) in our simulated world
            os.PopulateWorld(WD, World);
            WorldState W = World;
            RobotActions Actions;

            int i;
            for (i = 0; i < 5; i++)
            {
                Actions = getActions(W);
                W = os.RobotsAct(Actions);
            }
        }

        public RobotActions getActions(WorldState W)
        {
            RobotActions Actions = new RobotActions(new RobotAction[5] {
                new RobotAction("A", 90f, 9), 
                new RobotAction("B", 45f, 5),
                new RobotAction("C", 0f, 9), 
                new RobotAction("D", 0f, 9),
                new RobotAction("E", -180f, 8)});
            return Actions;
        }

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
