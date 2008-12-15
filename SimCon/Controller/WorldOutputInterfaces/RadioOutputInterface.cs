using System;

using System.IO.Ports;
using System.Threading;

namespace CS266.SimCon.Controller.WorldOutputInterfaces
{
    /// <summary>
    /// A world output interface that translates robot actions into commands that are sent to robots via the wireless base station,
    /// through a serial port
    /// </summary>
    public class RadioOutputInterface : WorldOutputInterface
    {
        /// <summary>
        /// Translate the RobotActions into real world robot movements serial commands 
        /// </summary>
        /// <param name="action"></param>
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

        /// <summary>
        /// Converts the command tring to ASCII and writes it to the serial port.
        /// </summary>
        /// <param name="cmd">A string representation of the command to be sent.</param>
        internal void sendCommand(String cmd)
        {
            cmd += '\n';
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte [] cmdBytes = encoding.GetBytes(cmd);
            serialPort.Write(cmdBytes, 0, cmdBytes.Length);
            Console.WriteLine("Sending command to robot " + cmd);
            Thread.Sleep(100);
        }

        /// <summary>
        /// Sends a command to reset the wheel count. This is necessary before every motion command.
        /// </summary>
        /// <param name="id"></param>
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

        internal void rotateOneWheel(int id, int degree)
        {
            int degree1 = degree * 6;
            int degree2 = -degree1;

            wheelCountReset(id);
            sendCommand("*" + id.ToString());
            sendCommand("C," + "0" + "," + degree2.ToString());
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
