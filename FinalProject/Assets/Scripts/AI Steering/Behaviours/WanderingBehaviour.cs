using System.Collections.Generic;
using UnityEngine;

public class WanderingBehaviour : SteeringBehaviour
{
    private bool didReachWanderLocation = true;
    private float timeoutTimer = 0.0f;

    public override SteeringBehaviourType GetSteeringBehaviourType()
    {
        return SteeringBehaviourType.Wandering;
    }

    public override float[] CalculateInterest(ref SteeringData steeringData, Vector3 position, List<Vector3> directions)
    {
        // Invoke the base class' implementation of CalculateInterest
        float[] interest = base.CalculateInterest(ref steeringData, position, directions);

        // Did we reach the wander location
        if (didReachWanderLocation)
        {
            didReachWanderLocation = false;

            float radians = 0;
            float distance = 0;
            Vector3 nextPosition = Vector3.zero;
            Collider[] colliders;
            do
            {
                radians = Random.Range(0.0f, 2.0f * Mathf.PI);
                distance = Random.Range(steeringData.Settings.wanderingMinDistance, steeringData.Settings.wanderingMaxDistance);
                nextPosition = position + new Vector3(Mathf.Cos(radians), 0.0f, Mathf.Sin(radians)) * distance;
                colliders = Physics.OverlapSphere(nextPosition, distance, steeringData.Settings.obstacleMask);
            }
            while (steeringData.Settings.wanderingBounds.Contains(nextPosition) == false || colliders.Length > 0);

            steeringData.WanderLocation = nextPosition;
            timeoutTimer = steeringData.Settings.wanderingTimeoutDuration;
        }

        // Have we reached the wander location?
        if (Vector3.Distance(position, steeringData.WanderLocation.Value) < steeringData.Settings.wanderingLocationThreshold)
        {
            didReachWanderLocation = true;
            steeringData.WanderLocation = null;
            return interest;
        }

        // Countdown the timeout timer, just in case we can't reach the wander location 
        if (timeoutTimer > 0.0f)
        {
            timeoutTimer -= Time.deltaTime;
            if (timeoutTimer < 0.0f)
            {
                timeoutTimer = 0.0f;
                didReachWanderLocation = true;
                return interest;
            }
        }

        // If we haven't reached the wander location, calculate the interest values
        Vector3 displacement = (steeringData.WanderLocation.Value - position);
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
