using System;
using System.Collections.Generic;
using System.Threading;
using CS266.SimCon.Controller.WorldOutputInterfaces;

using System.Text;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{   
    /**
     * 
     * Class to transform data from the camera into data structures for the control loop
     * 
     * */
    public class VisionInputInterface : WorldInputInterface
    {
        override public List<Robot> GetRobots()
        {
            return RobotList;
        }

        override public List<PhysObject> GetPhysObjects()
        {
            return PhysObjList;
        }

        public override ControllerWorldState getWorldState()
        {
            return new ControllerWorldState(RobotList, PhysObjList);
        }

        public override void setupInitialState()
        {
            List<shape> cameraData;
            cameraData = getCameraData();
            if (cameraData == null)
            {
                Console.WriteLine("Could not find camera, system shutting down");
                Environment.Exit(0);
            }
            Console.WriteLine("Number of shapes I detect: " + cameraData.Count);
            foreach (shape s in cameraData)
            {
                if (s.shapetype == "robot")
                {
                    Robot r = new Robot(s.id, s.shapetype, new Coordinates(s.x, s.y), s.orientation, s.width, s.height);
                    RobotList.Add(r);
                }
                else if (s.shapetype == "boundary")
                {
                    PhysObject phy = new Obstacle(s.id, s.shapetype, new Coordinates(s.x, s.y), s.orientation, s.width, s.height);
                }
                
            }

        }

        /**
         * 
         * Helper function to grad information from the camera
         * Also processes camera data into data structures for the WorldOutputInterface
         * 
         * */
        private List<shape> getCameraData()
        {
            RR_API rr = new RR_API();
            List<shape> shapeList = new List<shape>();
            System.IO.StreamReader sr;
            string s;

            if (!rr.connect("localhost"))
            {
              Console.WriteLine("Could not connect to localhost!");
              return null;
            }

            if (!rr.loadProgram("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\GetImage.robo"))
                Console.WriteLine("Program didn't run.\n");

            if (!rr.loadProgram("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\BlackStuff_test.robo"))
                Console.WriteLine("Black Program didn't run.\n");
     
            while (rr.getVariable("Program") != "1")
            {
                Thread.Sleep(5);
            }

            sr = System.IO.File.OpenText("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\Black.out");
            s = "";

            
            
            while ((s = sr.ReadLine()) != null)
            {
                // print out for testing
                //System.Console.WriteLine(s);

                shape sh = parseShapes(s);
                if (sh != null)
                    shapeList.Add(sh);
            }

            if (!rr.loadProgram("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\RedStuff_test.robo"))
                Console.WriteLine("Red Program didn't run.\n");

            while (rr.getVariable("RProgram") != "1")
            {
                Thread.Sleep(5);
            }
            
            sr = System.IO.File.OpenText("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\Red.out");
            s = "";
            while ((s = sr.ReadLine()) != null)
            {
                // print out for testing
                //System.Console.WriteLine(s);
                shape sh = parseShapes(s);
                if(sh != null)
                    shapeList.Add(sh);
            }

            return shapeList;
        }

        private shape parseShapes(string s)
        {

                string[] line = s.Split(' ');
                
                if (line.Length < 2)
                    return null;

                shape sh = new shape();

                sh.orientation = float.Parse(line[1]);
                sh.x = float.Parse(line[4]) + (float.Parse(line[4]) - float.Parse(line[2])) / 2;
                sh.y = float.Parse(line[5]) + (float.Parse(line[5]) - float.Parse(line[3])) / 2;
                
                // need to update this once there are more shapes
                if (line[6] == "square") {
                    sh.shapetype = "boundary";
                    sh.id = 0;
                } else {
                    sh.shapetype = "robot";
                    sh.id = int.Parse(line[6]);
                }

                // RIGHT NOW WIDTH AND HEIGHT AREN'T DEFINED

                System.Console.WriteLine("testing");
                System.Console.WriteLine(sh.id);
                System.Console.WriteLine(sh.shapetype);
                System.Console.WriteLine(sh.x);
                System.Console.WriteLine(sh.y);
                System.Console.WriteLine(sh.orientation);
                System.Console.WriteLine(sh.width);
                System.Console.WriteLine(sh.height);

                
                return sh;
    
        }
    }
}
