using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace CS266.SimCon.Controller.WorldOutputInterfaces
{
    public class RadioOutputInterface : WorldOutputInterface
    {
        SerialPort serialPort;

        void setupSerialPort()
        {
            serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.Two);
        }

        void sendCommand(String cmd)
        {
            cmd += '\n';
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte [] cmdBytes = encoding.GetBytes(cmd);
            serialPort.Write(cmdBytes, 0, cmdBytes.Length);
        }

        void wheelCountReset(int id)
        {
            sendCommand("*" + id.ToString());
        }

        void setSpeed(int id, int speed, int accel)
        {
            sendCommand("*" + id.ToString());
            sendCommand("J," + speed.ToString() + "," + accel.ToString() + "," + speed.ToString() + "," + accel.ToString());
        }

        void rotateDegree(int id, int degree)
        {
            int degree1 = degree * 6;
            int degree2 = -degree1;

            sendCommand("*" + id.ToString());
            wheelCountReset(id);
            sendCommand("C," + degree1.ToString() + "," + degree2.ToString());
        }

        void moveDistance(int id, int dist)
        {
            dist = (int)Math.Floor(dist * 12.5);
            sendCommand("*" + id.ToString());
            wheelCountReset(id);
            sendCommand("C," + dist.ToString() + "," + dist.ToString());
        }

        void stop(int id)
        {
            sendCommand("*" + id.ToString());
            sendCommand("D,0,0");
        }

        void restart(int id)
        {
            sendCommand("*" + id);
            sendCommand("restart");
        }
    }
}
