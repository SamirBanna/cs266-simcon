using System;
using System.Collections.Generic;

using System.Text;
using CS266.SimCon.Controller.WorldOutputInterfaces;

namespace CS266.SimCon.Controller.Driver
{
    public class RobotTest
    {
        public static void Run()
        {
            RadioOutputInterface radio = new RadioOutputInterface();
            radio.setupSerialPort();

        }
    }
}
