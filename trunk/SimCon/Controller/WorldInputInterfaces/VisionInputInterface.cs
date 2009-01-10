using System;
using System.Collections.Generic;
using System.Threading;
using CS266.SimCon.Controller.WorldOutputInterfaces;

using System.Text;

namespace CS266.SimCon.Controller.WorldInputInterfaces
{   
    ///
    ///<summary>Class to transform data from the camera into data structures for the control loop</summary>
    ///
    public class VisionInputInterface : WorldInputInterface
    {
        public static Boolean connected = false;
        private RR_API rr = new RR_API();


        public VisionInputInterface()
        {
            worldHeight = 90;//114;
            worldWidth = 200;// 235;

            //114
            //235

            //110
            //232

            //90
            //200
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ControllerWorldState getWorldState()
        {
            
            //Thread.Sleep(10000);
            RobotList.Clear();
            PhysObjList.Clear();
            FoodList.Clear();

            SetupInitialState();
            return new ControllerWorldState(RobotList, PhysObjList, FoodList, worldHeight, worldWidth);
        }

        public override void SetupInitialState()
        {
            
            
            List<shape> cameraData;
            cameraData = getCameraData();//returns positions in units of pixels, correctly zeroed
            
            if (cameraData == null)
            {
                Console.WriteLine("Could not find camera, system shutting down");
                Environment.Exit(0);
            }
            Console.WriteLine("camera sees the folowing world state (dimensions in cm):");
            Console.WriteLine("number of shapes - " + cameraData.Count);
            int objcount = 0;
            foreach (shape s in cameraData)
            {
                if (s.shapetype == "robot")
                {
                    //these conversions appear to convert from pixels to cm
                    //Console.WriteLine("Position (x,y) in pixels: " + s.x + " " + s.y);
                    double robotx = (s.x / 5);// +15;
                    //This conversion scale requires a +2
                    double roboty = (s.y / 5);// +15;
                    //Console.WriteLine("Position (x,y) in cm: " + robotx + " " + roboty);
                    Console.WriteLine("robot id# " + s.id + "   x - " + robotx + "   y - " + roboty + "   orientation - " + s.orientation);

                    //if (robotx > 235 || roboty > 114)
                    //    continue;
                    //if (robotx < 0 || roboty < 0)
                    //    continue;
                    
                    if(s.id == 8){

                        Console.WriteLine("food" + "   x - " + robotx + "   y - " + roboty);
                        Food f = new Food(s.id, "food", new Coordinates(robotx,roboty), s.orientation, s.width, s.height);
                        FoodList.Add(f);
                        //PhysObjList.Add(f);
                        continue;

                    }

                    Robot r = new Robot(s.id, "robot", new Coordinates(robotx,roboty), s.orientation, s.width, s.height);

                    bool flag = false;
                    foreach (Robot x in RobotList)
                    {
                        if (s.id == x.Id)
                            flag = true;
                    }
                    if (!flag)
                    {// robot hasn't already been inserted
                        
                            RobotList.Add(r);
                            //Console.WriteLine("Robot's X is: " + robotx);
                            //Console.WriteLine("Robot's Y is: " + roboty);
                            //Console.WriteLine("Robot's id is: " + s.id);
                            //Console.WriteLine("ROBOT ANGLE!!!!!!!!!!!!!!!!!!!!!: " + r.Orientation);
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

                    double robotx = (s.x / 5);// + 15;
                    double roboty = (s.y / 5);// +15;
                    
                    PhysObject phy = new Obstacle(objcount++, "obstactle", new Coordinates(robotx, roboty), s.orientation, s.width, s.height);
                    PhysObjList.Add(phy);

                    Console.WriteLine("obstacle" + "   x - " + robotx + "   y - " + roboty + "   orientation - " + s.orientation);
                }
                
            }
            Console.WriteLine("Number of physical objects in the world: " + PhysObjList.Count);
            Console.WriteLine("Number of robots in the world: " + RobotList.Count);
            Console.WriteLine("end camera world state");
            //ws.physobjects = PhysObjList;
            //ws.robots = RobotList;
            Console.WriteLine("just read world state from camera, continue?");
            Console.ReadLine();
        }

        /**
         * 
         * Helper function to grad information from the camera
         * Also processes camera data into data structures for the WorldOutputInterface
         * 
         * */

        
        public List<shape> oldShapeList = new List<shape>();

        private void takeNewCameraImage()
        {
            //rr.run("on");
            if (!rr.loadProgram("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\GetImage.robo"))
                Console.WriteLine("Image capture program didn't run.\n");
            rr.run("once");

            //wait until image capture is complete
            while (rr.getVariable("imageCaptureComplete") != "1")
            {
                Thread.Sleep(50);
                Console.WriteLine("sleeping in takeNewCameraImage()");
            }
            rr.run("off");
        }

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

            takeNewCameraImage();
            //sometimes the next roborealm loadProgram() gets stuck, regardless of which program it is
            //the sleep_counter thing in the wait loop below is a hack to restart roborealm in this case
            
            //Thread.Sleep(100);
            //begin red obstacles
            ///*Start blob processing
            if (!rr.loadProgram("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\Red_blobs.robo"))
                Console.WriteLine("Red Blobs Program didn't run.\n");
            rr.run("on");//unknown whether this should be "once" or "on".  "once" gets stuck sometimes

            int sleep_counter = 0;

            while (rr.getVariable("ObProgram") != "1")
            {
                Thread.Sleep(50);
                Console.WriteLine("sleeping in obstacles"+"   roborealm running.. " + rr.getVariable("counter"));
                sleep_counter++;

                if (sleep_counter > 20)
                {
                    Console.WriteLine("Roborealm seems to stuck, rerun..");
                    rr.run("off");
                    rr.run("on");
                }

            }
            rr.run("off");
            System.IO.StreamReader sblob = System.IO.File.OpenText("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\Obstacles.out");
            //int[] Blobx = new int[100];
            //int[] Bloby = new int[100];

            s = "";
            //int bcount = 0;
            while ((s = sblob.ReadLine()) != null)
            {
                shape obs = new shape();

                obs.shapetype = "boundary";
                obs.id = 0;

                string[] toks = s.Split(' ');

                //Blobx[bcount] = int.Parse(toks[0]);
                //Bloby[bcount] = int.Parse(toks[1]);7

                obs.x = double.Parse(toks[0]) - 100;
                obs.y = double.Parse(toks[1]) - 280;
                obs.orientation = 0;

                if (obs != null)
                    shapeList.Add(obs);

                //bcount ++;
            }

            // End red obstacles


            //begin blue robots
			// TODO: Make the location of the vision output VARIABLE!
            do
            {
                if (!rr.loadProgram("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\BlueStuff_smoothLarge_test.robo"))
                    Console.WriteLine("Blue Program didn't run.\n");
                rr.run("once");

                while (rr.getVariable("BProgram") != "1")
                {
                    Thread.Sleep(50);
                    Console.WriteLine("sleeping in blue robots");
                }
                rr.run("off");
                if (rr.getVariable("berror") != "0")
                {
                    Console.WriteLine("Fail recognition. Retake camera image, might take a longer time.\n");
                    takeNewCameraImage();
                }
            } while (rr.getVariable("berror") != "0");

            sr = System.IO.File.OpenText("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\Blue.out");
            s = "";

            //System.Console.WriteLine("hey");
            while ((s = sr.ReadLine()) != null)
            {
                // print out for testing
                //System.Console.WriteLine("blue shapes");
                //System.Console.WriteLine(s);
                Console.WriteLine("reading line " +s);
                shape sh = parseShapes(s);
                if (sh != null)
                    shapeList.Add(sh);
            }
            sr.Close();
            //end blue robots

            ///*Start food processing
            if (!rr.loadProgram("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\Blue_blobs.robo"))
                Console.WriteLine("Blue Blobs Program didn't run.\n");
            rr.run("once");

            while (rr.getVariable("FoProgram") != "1")
            {
                Thread.Sleep(50);
                Console.WriteLine("sleeping in blue blobs");
            }
            rr.run("off");
            System.IO.StreamReader sfood = System.IO.File.OpenText("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\Food.out");
            //int[] Blobx = new int[100];
            //int[] Bloby = new int[100];

            s = "";
            //int bcount = 0;
            while ((s = sfood.ReadLine()) != null)
            {
                shape food = new shape();

                food.shapetype = "robot";
                food.id = 8;
                string[] toks = s.Split(' ');

                //Blobx[bcount] = int.Parse(toks[0]);
                //Bloby[bcount] = int.Parse(toks[1]);7

                food.x = double.Parse(toks[0]) - 100;
                food.y = double.Parse(toks[1]) - 280;
                food.orientation = 0;

                if (food != null)
                    shapeList.Add(food);

                //bcount ++;
            }
// end of food

            /*
            //start of red robot processing
            do
            {
                if (!rr.loadProgram("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\RedStuff_smoothLarge_test.robo"))
                    Console.WriteLine("Red Program didn't run.\n");
                rr.run("once");

                while (rr.getVariable("RProgram") != "1")
                {
                    Thread.Sleep(50);
                    Console.WriteLine("sleeping in red robots");
                }
                rr.run("off");
                if(rr.getVariable("rerror") != "0")
                {
                    Console.WriteLine("Fail recognition. Retake camera image, might take a longer time.\n");
                    takeNewCameraImage();
                }
            } while (rr.getVariable("rerror") != "0");


            sr = System.IO.File.OpenText("c:\\Documents and Settings\\cs266\\Desktop\\API\\API\\Python\\Red.out");
            s = "";
            while ((s = sr.ReadLine()) != null)
            {
                // print out for testing
                //System.Console.WriteLine("Printing red");
                //System.Console.WriteLine(s);
                shape sh = parseShapes(s);
                if (sh != null)
                    shapeList.Add(sh);
            }
            //end of red robot processing
            */
            

            //shape s_stored = new shape();
            //s_stored.id = -1;
            //for (int i = 1; i <= 10; i++)
            //{
            //    foreach (shape sha in shapeList)
            //    {
            //        if (sha.shapetype == "robot")
            //        {
            //            if (s_stored.id == -1 && sha.id==i)
            //            {
            //                s_stored = sha;
            //            }
            //            else if(sha.id==i)
            //            {
            //                if (s_stored.confidence < sha.confidence)
            //                    shapeList.Remove(s_stored);
            //                else
            //                    shapeList.Remove(sha);
            //            }
            //        }
            //    }
            //}

            sr.Close();
        
            /*
            List<shape> anOldshapeList = shapeList;

            int[] foundb = new int[bcount];
            for(int i = 0; i < bcount; i++)
                foundb[i] = -1;

            int foundcount = 0;
            
            for(int i = 0; i < bcount; i++) {
                int found = -1;
                
                foreach (shape sh in shapeList)
                {
                  if ((Blobx[i] - 15 <= sh.x && Blobx[i] + 15 >= sh.x) &&
                      (Bloby[i] - 15 <= sh.y && Bloby[i] + 15 >= sh.y))
                    {
                      found = 1;
                    }
                }

                if(found != 1) {
                 shape myShape = new shape();
                 double distance = -1;
                 foreach (shape sh in oldShapeList){
                     double mydistance = Math.Sqrt( ((Blobx[i] - sh.x)*(Blobx[i] - sh.x)) + ((Bloby[i] - sh.y)*(Bloby[i] - sh.y)));
                     if(distance == -1 || mydistance < distance) {
                         distance = mydistance;
                         myShape = sh;
                     }
                 }
                    if(distance < 300)
                     shapeList.Add(myShape);
                }


            }

            oldShapeList = anOldshapeList; */

            //Console.WriteLine("VISION: Number of detected objects: "+shapeList.Count+"\n");

            return shapeList;
        }
            
        private shape parseShapes(string s)
        {
            //this is only called for robot shapes
            string[] line = s.Split(' ');
            
            if (line.Length < 2)
                return null;

            shape sh = new shape();

            sh.orientation = (450+double.Parse(line[1]))%360;
            if (sh.orientation > 180)
                sh.orientation -= 360;
            
            //line 2,3,4,5 are the limits of the bounding box, these lines find the center
            //line 2,4 is bottom left
            //line 3,5 is the top right
            //bounding box does not take into account robot orientation
            sh.x = double.Parse(line[2]) + (double.Parse(line[3]) - double.Parse(line[2])) / 2;
            sh.y = double.Parse(line[4]) + (double.Parse(line[5]) - double.Parse(line[4])) / 2;
            //these are pixel offsets to place (0,0)
            //when this isn't called (eg for an obstacle), the offset must still be done
            sh.x -= 100;
            sh.y -= 280;

            sh.confidence = double.Parse(line[0]);

            // need to update this once there are more shapes
            //does this ever happen? parseShapes() is only called for robots
            if (line[6] == "square")
            {
                //sh.shapetype = "boundary";
                //sh.id = 0;
            }
            else if (line[6] == "food")
            {
                //Console.WriteLine("DSLFKJS:DLFKJS:DLFKJS:DLFKJS SAW FOOD");
                //sh.shapetype = "robot";
                //sh.id = 8;
            }
            else //always
            {
                //Console.WriteLine("++++++++++++++Line 6:" + line[6]);
                sh.shapetype = "robot";
                try
                {
                    sh.id = int.Parse(line[6]);
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            if (sh.x > 1200)
            {
                Console.WriteLine("VISION: coordinate problem " + sh.id + " x " + sh.x + "\n");
                Thread.Sleep(10000);
            }
            if (sh.y > 600)
            {
                Console.WriteLine("VISION: coordinate problem " + sh.id + " y " + sh.y + "\n");
                Thread.Sleep(10000);
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
            //System.Console.WriteLine("orientation unfixed" + double.Parse(line[1]));
            //System.Console.WriteLine("orientation"+sh.orientation);
            //System.Console.WriteLine(sh.width);
            //System.Console.WriteLine(sh.height);

            
            return sh;
    
        }


    }
}
