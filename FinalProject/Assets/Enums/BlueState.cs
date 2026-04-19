using System;
using Unity.Behavior;

[BlackboardEnum]
public enum State_Blue
{
	TargetBarrel,
	TargetDetonator,
	ProtectDetonator,
}

//Game Over 
//protect my detonator - check closest tank to my detonator, shoot it. graph checks if immobilized
//wander - cant see any barrels, move to opposite side
