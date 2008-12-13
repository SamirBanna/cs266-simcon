// Extend sensorInput to RandomWalkSensorInput
// Filename: RandomWalkSensorInput.cs
using System;
using System.Collections.Generic;

using System.Text;

namespace CS266.SimCon.Controller
{
    public class RandomWalkSensorInput : SensorInput
    {
        public bool isMoving;
        public bool senseFood;
        public bool faceObstacle;

        public RandomWalkSensorInput(){}
        public RandomWalkSensorInput(bool isMoving, bool senseFood, bool faceObstacle)
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

