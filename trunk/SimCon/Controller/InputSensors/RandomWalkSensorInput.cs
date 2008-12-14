// Extend InputSensor to RandomWalkInputSensor
// Filename: RandomWalkInputSensor.cs
using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller.InputSensors
{
    public class RandomWalkInputSensor : InputSensor
    {
        public bool isMoving;
        public bool senseFood;
        public bool faceObstacle;

        public RandomWalkInputSensor(){}
        public RandomWalkInputSensor(bool isMoving, bool senseFood, bool faceObstacle)
        {
            this.isMoving = isMoving;
	    this.senseFood = senseFood;
	    this.faceObstacle = faceObstacle;
	    }
        public override void UpdateSensor()
        {
            throw new NotImplementedException();
        }
    }
}

