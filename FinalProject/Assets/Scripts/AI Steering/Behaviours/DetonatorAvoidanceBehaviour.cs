using System.Collections.Generic;
using Unity.AppUI.Redux;
using UnityEngine;

public class DetonatorAvoidanceBehaviour : SteeringBehaviour
{
    public override SteeringBehaviourType GetSteeringBehaviourType()
    {
        return SteeringBehaviourType.DetonatorAvoidance;
    }

    public override float[] CalculateDanger(ref SteeringData steeringData, Vector3 position, List<Vector3> directions)
    {
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

        GameObject myDet = GameObject.FindWithTag("Detonator-Blue");

        //calculate displacement from detonator 
        closestPoint = myDet.transform.position;
        displacement = closestPoint - position;

        //calculate distance and use for weight
        distance = displacement.magnitude;
        weight = (distance <= steeringData.Settings.obstacleAvoidanceRadius) ? 1.0f : (radius - distance) / radius;

        //calculate obstacle direction
        direction = displacement.normalized;

        //add parameters to danger array
        for (int i = 0; i < directions.Count; i++)
        {
            alignment = Vector3.Dot(direction, directions[i]);
            weightedAlignment = alignment * weight;

            //override value only if its higher than current one stored in danger array
            if (weightedAlignment > danger[i])
            {
                danger[i] = weightedAlignment;  
            }
        }
        return danger;
    }


}
