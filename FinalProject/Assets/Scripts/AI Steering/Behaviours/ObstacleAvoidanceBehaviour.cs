using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    public override SteeringBehaviourType GetSteeringBehaviourType()
    {
        return SteeringBehaviourType.ObstacleAvoidance;
    }

    public override float[] CalculateDanger(ref SteeringData steeringData, Vector3 position, List<Vector3> directions)
    {
        // Invoke the base class' implementation of CalculateDanger
        float[] danger = base.CalculateDanger(ref steeringData, position, directions);

        // Local variables
        Vector3 closestPoint = Vector3.zero;
        Vector3 displacement = Vector3.zero;
        Vector3 direction = Vector3.zero;
        float radius = steeringData.Settings.obstacleDetectionRadius;
        float distance = 0.0f;
        float weight = 0.0f;
        float weightedAlignment = 0.0f;
        float alignment = 0.0f;

        // Loop through each obstacle in the steering data
        foreach (Collider collider in steeringData.Obstacles)
        {
            // Calculate the displacement from the closest point on the obstacle collider
            closestPoint = collider.ClosestPoint(position);
            displacement = closestPoint - position;

            // Calculate the distance and then use it to determine the weight
            distance = displacement.magnitude;
            weight = (distance <= steeringData.Settings.obstacleAvoidanceRadius) ? 1.0f : (radius - distance) / radius;

            // Calculate the obstacle direction
            direction = displacement.normalized;

            //Add obstacle parameters to the danger array
            for (int i = 0; i < directions.Count; i++)
            {
                alignment = Vector3.Dot(direction, directions[i]);
                weightedAlignment = alignment * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (weightedAlignment > danger[i])
                {
                    danger[i] = weightedAlignment;
                }
            }
        }

        return danger;
    }
}
