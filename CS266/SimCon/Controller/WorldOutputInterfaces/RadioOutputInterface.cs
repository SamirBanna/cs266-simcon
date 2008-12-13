using System;
using System.Collections.Generic;

using System.Text;
using System.IO.Ports;
using System.Threading;

namespace CS266.SimCon.Controller.WorldOutputInterfaces
{
    public class RadioOutputInterface : WorldOutputInterface
    {
        //Translate the RobotActions into real world robot movements serial commands
        public override void DoActions(PhysicalRobotAction action)
        {
            if (action.ActionType == PhysicalActionType.MoveForward)
                moveDistance(action.RobotId, (int) action.ActionValue);
            else if (action.ActionType == PhysicalActionType.MoveBackward)
                moveDistance(action.RobotId, -1*(int)action.ActionValue);
            else if (action.ActionType == PhysicalActionType.Turn)
                rotateDegree(action.RobotId, (int)action.ActionValue);
            else if (action.ActionType == PhysicalActionType.SetSpeed)
                moveDistance(action.RobotId, (int)action.ActionValue);

             
        }
        
        SerialPort serialPort;

        public RadioOutputInterface()
        {
            setupSerialPort();
        }

        

        internal void setupSerialPort()
        {
            serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.Two);
            serialPort.Open();
        }

        internal void sendCommand(String cmd)
        {
            cmd += '\n';
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte [] cmdBytes = encoding.GetBytes(cmd);
            serialPort.Write(cmdBytes, 0, cmdBytes.Length);
            Console.WriteLine("Sending command to robot " + cmd);
            Thread.Sleep(100);
        }

        internal void wheelCountReset(int id)
        {
            sendCommand("*" + id.ToString());
            sendCommand("G,0,0");
            
        }

        internal void setSpeed(int id, int speed, int accel)
        {
            sendCommand("*" + id.ToString());
            sendCommand("J," + speed.ToString() + "," + accel.ToString() + "," + speed.ToString() + "," + accel.ToString());
        }

        internal void rotateDegree(int id, int degree)
        {
            int degree1 = degree * 6;
            int degree2 = -degree1;

            wheelCountReset(id);
            sendCommand("*" + id.ToString());
            sendCommand("C," + degree1.ToString() + "," + degree2.ToString());
        }

        public  void testSpin(int id, int degree)
        {
            int degree1 = degree * 6;
            int degree2 = -degree1;

            wheelCountReset(id);
            sendCommand("*" + id.ToString());
            sendCommand("C," + degree1.ToString() + "," + degree2.ToString());
        }

        internal void moveDistance(int id, int dist)
        {
            dist = (int)Math.Floor(dist * 12.5);
            wheelCountReset(id);
            sendCommand("*" + id.ToString());
            sendCommand("C," + dist.ToString() + "," + dist.ToString());
        }

        internal void stop(int id)
        {
            sendCommand("*" + id.ToString());
            sendCommand("D,0,0");
        }

        internal void restart(int id)
        {
            sendCommand("*" + id);
            sendCommand("restart");
        }
    }
}
