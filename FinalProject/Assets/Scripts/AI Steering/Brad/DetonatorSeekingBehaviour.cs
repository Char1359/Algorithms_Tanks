using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetonatorSeekingBehaviour : SteeringBehaviour
{
    public override SteeringBehaviourType GetSteeringBehaviourType()
    {
        return SteeringBehaviourType.DetonatorSeek;
    }

    public override float[] CalculateInterest(ref SteeringData steeringData, Vector3 position, List<Vector3> directions)
    {
        // Invoke the base class' implementation of CalculateInterest
        float[] interest = base.CalculateInterest(ref steeringData, position, directions);

        //Get closest detonator
        steeringData.CurrentTarget = steeringData.GetClosestDetonator(position).transform;

        for(int i = 0; i < steeringData.Detonators.Count; i++)
        {
            if (steeringData.Detonators[i].gameObject.GetComponent<Detonator>().isTriggered == false)
            {
                steeringData.CurrentTarget = steeringData.Detonators[i].transform;
            }
        }

        Vector3 displacement = (steeringData.CurrentTarget.position - position);
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
