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
        public static Boolean connected = false;
        private RR_API rr = new RR_API();
        public double worldWidth = 200;
        public double worldHeight = 90;

        override public List<Robot> GetRobots()
        {
            return RobotList;
        }

        override public List<PhysObject> GetPhysObjects()
        {
            return PhysObjList;
        }


        public  override ControllerWorldState getNewWorldState()
        {

            Console.WriteLine("Making new world state");
            RobotList.Clear();
            PhysObjList.Clear();

            setupInitialState();

            return new ControllerWorldState(RobotList, PhysObjList, FoodList, worldWidth, worldHeight);

        }

        public override ControllerWorldState getWorldState()
        {
            
            //Thread.Sleep(10000);
            RobotList.Clear();
            PhysObjList.Clear();

            setupInitialState();
            return new ControllerWorldState(RobotList, PhysObjList, FoodList, worldWidth, worldHeight); ;
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
            int objcount = 0;
            foreach (shape s in cameraData)
            {
                if (s.shapetype == "robot")
                {
                    //Console.WriteLine("Making a new robot");

                    Robot r = new Robot(s.id, s.shapetype, new Coordinates(s.x, s.y), s.orientation, s.width, s.height);

                    bool flag = false;
                    foreach (Robot x in RobotList)
                    {
                        if (s.id == x.Id)
                            flag = true;
                    }
                    if (!flag)
                    {// robot hasn't already been inserted
                        RobotList.Add(r);
                    }
                    else
                    {
                        //shouldn't happen!!!!
                    }
                    
                //    System.Console.WriteLine("******************************************************");
                 //   System.Console.WriteLine("Robot x= " + s.x);
                 //   System.Console.WriteLine("Robot y= " + s.y);
                  //  System.Console.WriteLine("Robot degree in camera= " + s.orientation);
                   // System.Console.WriteLine("******************************************************");
                }
                else if (s.shapetype == "boundary")
                {
                    PhysObject phy = new Obstacle(objcount++, s.shapetype, new Coordinates(s.x, s.y), s.orientation, s.width, s.height);
                    PhysObjList.Add(phy);
                    
                }
                
            }
            //ws.physobjects = PhysObjList;
            //ws.robots = RobotList;
        }

        /**
         * 
         * Helper function to grad information from the camera
         * Also processes camera data into data structures for the WorldOutputInterface
         * 
         * */
        private List<shape> getCameraData()
        {
            
            
            List<shape> shapeList = new List<shape>();
            System.IO.StreamReader sr;
            string s;

            if (!connected)
            {
                if (!rr.connect("localhost"))
                {
                    Console.WriteLine("Could not connect to localhost!");
                    return null;
                }
                connected = true;
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
                System.Console.WriteLine(s);

                shape sh = parseShapes(s);
                if (sh != null)
                    shapeList.Add(sh);
            }
            sr.Close();

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
                System.Console.WriteLine("Printing red");
                System.Console.WriteLine(s);
                shape sh = parseShapes(s);
                if(sh != null)
                    shapeList.Add(sh);
            }

            shape s_stored = new shape();
            s_stored.id = -1;
            for (int i = 1; i <= 10; i++)
            {
                foreach (shape sha in shapeList)
                {
                    if (sha.shapetype == "robot")
                    {
                        if (s_stored.id == -1 && sha.id==i)
                        {
                            s_stored = sha;
                        }
                        else if(sha.id==i)
                        {
                            if (s_stored.confidence < sha.confidence)
                                shapeList.Remove(s_stored);
                            else
                                shapeList.Remove(sha);
                        }
                    }
                }
            }

            sr.Close();
            return shapeList;
        }

        private shape parseShapes(string s)
        {

                string[] line = s.Split(' ');
                
                if (line.Length < 2)
                    return null;

                shape sh = new shape();

                sh.orientation = (450+float.Parse(line[1]))%360;
                if (sh.orientation > 180)
                    sh.orientation -= 360;

                sh.x = float.Parse(line[2]) + (float.Parse(line[3]) - float.Parse(line[2])) / 2;
                sh.y = float.Parse(line[4]) + (float.Parse(line[5]) - float.Parse(line[4])) / 2;

                sh.confidence = float.Parse(line[0]);

                // need to update this once there are more shapes
                if (line[6] == "square") {
                    sh.shapetype = "boundary";
                    sh.id = 0;
                } else {
                    Console.WriteLine("++++++++++++++Line 6:" +line[6]);
                    sh.shapetype = "robot";
                    sh.id = int.Parse(line[6]);
                }

                // RIGHT NOW WIDTH AND HEIGHT AREN'T DEFINED
                //Console.WriteLine("==========================new camera data==================================");
                //System.Console.WriteLine("testing");
                //System.Console.WriteLine(sh.id);
                //System.Console.WriteLine(sh.shapetype);
                //System.Console.WriteLine("xmin" + line[2]);
                //System.Console.WriteLine("xmax" + line[3]);
                //System.Console.WriteLine("ymin" + line[4]);
                //System.Console.WriteLine("ymax" + line[5]);
                //System.Console.WriteLine(sh.x);
                //System.Console.WriteLine(sh.y);
                //System.Console.WriteLine("orientation unfixed" + float.Parse(line[1]));
                //System.Console.WriteLine("orientation"+sh.orientation);
                //System.Console.WriteLine(sh.width);
                //System.Console.WriteLine(sh.height);

                
                return sh;
    
        }
        // Just for compilation

        public override List<Food> GetFood()
        {
            
            throw new NotImplementedException();
        }
    }
}
