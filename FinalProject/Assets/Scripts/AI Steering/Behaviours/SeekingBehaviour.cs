using System.Collections.Generic;
using UnityEngine;

public class SeekingBehaviour : SteeringBehaviour
{
    private bool didReachLastTarget = true;
    private Vector3 lastKnownLocation;

    public override SteeringBehaviourType GetSteeringBehaviourType()
    {
        return SteeringBehaviourType.Seeking;
    }

    public override float[] CalculateInterest(ref SteeringData steeringData, Vector3 position, List<Vector3> directions)
    {
        // Invoke the base class' implementation of CalculateInterest
        float[] interest = base.CalculateInterest(ref steeringData, position, directions);

        // Did we reach the last target
        if (didReachLastTarget)
        {
            // If there isn't a target stop seeking
            if (steeringData.Targets.Count < 1)
            {
                steeringData.CurrentTarget = null;
                return interest;
            }
            // Otherwise set a new target
            else
            {
                didReachLastTarget = false;
                steeringData.CurrentTarget = steeringData.GetClosestTarget(position).transform;
            }
        }

        // Keep track of the last known location
        if (steeringData.CurrentTarget != null)
        {
            lastKnownLocation = steeringData.CurrentTarget.position;
        }

        // Have we reached the target?
        if (Vector3.Distance(position, lastKnownLocation) < steeringData.Settings.seekingTargetThreshold)
        {
            didReachLastTarget = true;
            steeringData.CurrentTarget = null;
            return interest;
        }

        // If we haven't reached the target, calculate the interest values
        Vector3 displacement = (lastKnownLocation - position);
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
