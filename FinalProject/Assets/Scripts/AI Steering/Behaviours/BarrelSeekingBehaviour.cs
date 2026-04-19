using System.Collections.Generic;
using UnityEngine;

public class BarrelSeekingBehaviour : SteeringBehaviour
{
	public override SteeringBehaviourType GetSteeringBehaviourType()
	{
		return SteeringBehaviourType.BarrelSeek;
	}

	public override float[] CalculateInterest(ref SteeringData steeringData, Vector3 position, List<Vector3> directions)
	{
		// Invoke the base class' implementation of CalculateInterest
		float[] interest = base.CalculateInterest(ref steeringData, position, directions);

		// Get the closest barrel
        steeringData.CurrentBarrel = steeringData.GetClosestBarrel(position).transform;
		if (steeringData.CurrentBarrel == null)
		{
			return interest;
		}

        // If we haven't reached the target, calculate the interest values
        Vector3 displacement = (steeringData.CurrentBarrel.position - position);
		Vector3 direction = displacement.normalized;
		float result = 0.0f;

		for (int i = 0; i < directions.Count; i++)
		{
			result = Vector3.Dot(direction, directions[i]);
			if (result > 0.0f)
			{
				if (result > interest[i])
				{
					interest[i] = result;
				}
			}
		}

		return interest;
	}
}
