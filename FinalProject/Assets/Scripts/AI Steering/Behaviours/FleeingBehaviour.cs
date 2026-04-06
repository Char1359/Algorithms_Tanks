using System.Collections.Generic;
using UnityEngine;

public class FleeingBehaviour : SteeringBehaviour
{
    public override SteeringBehaviourType GetSteeringBehaviourType()
    {
        return SteeringBehaviourType.Fleeing;
    }

    public override float[] CalculateInterest(ref SteeringData steeringData, Vector3 position, List<Vector3> directions)
    {
        // Invoke the base class' implementation of CalculateInterest
        float[] interest = base.CalculateInterest(ref steeringData, position, directions);

        Vector3 displacement = (steeringData.CurrentTarget.transform.position - position);
        Vector3 direction = displacement.normalized * -1.0f;
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
