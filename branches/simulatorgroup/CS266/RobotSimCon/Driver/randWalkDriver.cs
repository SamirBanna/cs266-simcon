//  Extend Driver.cs to randWalkDriver. Add this function there
// Given: list of robots and world objects, distance at which food can be sensed
// Output: Update each robot's sensor input to contain:
// 1. whether that robot is moving
// 2. whether that robot senses food
// 3. whether that robot faces an obstacle
// Assumes (DO CHECK THAT THESE ARE GIVEN!):
// 1. Robot CurrentInput is already initialized (TODO: check this)
// 2. speed is already read into list of robots via GetRobots (TODO: check this)
// 3. Assume PhysObject has ObjectType objectType field as enum'ed (TODO: put this in!)
// 4. Assume there is ObstacleObject that extends PhysObject (TODO: not currently there)
// 5. Robot already knows its location (TODO: check this)
// 6. assume orientation is in angle (not radians) and from [-180, 180] (TODO: is orientation given this way?)

public void UpdateLocalAgentsView(List<Robot> robots, List<PhysObject> worldObjects, 
		   double sensingDistance){
  double foodSensingAngle = 15; // angle at which it can sense food objects
                                // assumed symmetric for + or - direction
  double foodSensingDist = 3; // distance at which food items can be sensed
  double obstacleSensingAngle = 10; // angle at which it can sense obstacle objects
                                    // assumed symmetric for + or - direction
  double obstacleSensingDist = 2; // distance at which obstacles can be sensed


  // 1. Check if robots are moving
  foreach (Robot rob in robots){
    RandomWalkSensorInput input = rob.CurrentInput;
    
    if(rob.Speed > 0){// robot's moving
      input.isMoving = true;
    }else{// robot's not moving
      input.isMoving = false;
    }
  }
  
  // 2. Check if robots sense food
  foreach (Robot rob in robots){
    RandomWalkSensorInput input = rob.CurrentInput;
    input.senseFood = false; // reset to false each time
    
    foreach (PhysObject obj in worldObjects){
      if(obj.objectType == ObjectType.Food){// food object
	Food food = (Food)obj;
	// check if it is near the robot
	if(senseFood(foodSensingAngle, foodSensingDist, rob, food) == true){
	  input.senseFood = true;
	  break;
	}
      }
    }
  }

  // 3. Check if robots sense obstacle
  foreach (Robot rob in robots){
    RandomWalkSensorInput input = rob.CurrentInput;
    input.faceObstacle = false; // reset to false each time
    
    foreach (PhysObject obj in worldObjects){
      if(obj.objectType == ObjectType.Obstacle){// obstacle
	 Obstacle obs = (Obstacle)obj;
	// check if it is near the robot
	if(senseObstacle(obstacleSensingAngle, obstacleSensingDist, rob, obs) == true){
	  input.faceObstacle = true;
	  break;
	}
      }
    }
  }
}

// returns true if robot senses food in +/- angle within distance
bool senseFood(double maxangle, double maxdistance, Robot rob, Food food){
  Coordinates foodLocation = food.location;
  Coordinates robotLocation = rob.location;

  double distance = Math.Sqrt(
	  (foodLocation.x - robotLocation.x)*(foodLocation.x - robotLocation.x) + 
	  (foodLocation.y - robotLocation.y)*(foodLocation.y - robotLocation.y));  
  
  if(distance > maxdistance) return false;
  
  // get angle between (in radians)
  double radians = ATan2(foodLocation.y - robotLocation.y, 
		       foodLocation.x - robotLocation.x); 
  double angle = radians * (180/Math.PI); // from -180 to 180

   // assume orientation is defined from -180 to 180
  double angleDifference;
  double orientation = rob.orientation;
  
  if((angle >= 0 && orientation >= 0) || (angle <= 0 && orientation <= 0)){
    angleDifference = Math.Abs(angle - orientation);
  }else{//find angle difference by adding their degree distance from 0, then 
    // finding shorter arc if exists
    angleDifference = Math.Abs(angle) + Math.Abs(orientation);
    if(angleDifference > 360 - angleDifference){
      angleDifference = 360 - angleDifference;
    }
  }   
  if(angleDifference < maxangle){
    return true;    
  }
  
  return false;
}

// return true if robot senses obstacle in +/- angle within distance
// cannot implement until we have obstacle type...
bool senseObstacle(double angle, double distance, Robot rob, Obstacle obs){
  return false;
}


