﻿using System;
using System.Collections.Generic;
using System.Text;
using CS266.SimCon.Controller;

namespace CS266.SimCon.Controller.InputSensors
{
    /**
     * 
     *  This class will be used to create Sensors based on their string name
     *  
     *  Whenever a new InputSensor is written, it should be added here into the switch statement so that
     *  when the sensors are initialized by name, they can be found
     *  
     *  Assembly.GetExecutingAssembly could have been used to create the objects based on class name
     *  but was decided against. (Security issues, and to avoid other user errors)
     * 
     * */
    public class SensorList
    {

        public static InputSensor makeSensor(String name)
        {
            InputSensor sensor = null;

            switch (name)
            {

                case "ProximitySensor":
                    sensor = new ProximitySensor();
                    break;

                case "FoodSensor":
                    sensor = new FoodSensor();
                    break;

                case "ObstacleSensor":
                    sensor = new ObstacleSensor();
                    break;

                case "RobotSensor":
                    sensor = new RobotSensor();
                    break;

                case "SpeedSensor":
                    sensor = new SpeedSensor();
                    break;

                case "DFSSensor":
                    sensor = new DFSSensor();
                    break;

                case "GridSensor":
                    sensor = new GridSensor();
                    break;

                case "BoundarySensor":
                    sensor = new BoundarySensor();
                    break;
                
                case "LocalGridSensor":
                    sensor = new LocalGridSensor();
                    break;

                default:
                    throw new System.Exception("No sensor created.");
            }

            return sensor;
    
        }
    }

    
}
