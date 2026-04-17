using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum SteeringBehaviourType
{
    None = 0,
    ObstacleAvoidance = 1,      // 0000 0001
    Wandering = 2,              // 0000 0010
    Seeking = 4,                // 0000 0100
    Fleeing = 8,                // 0000 1000
    BarrelSeek = 16,            // 0001 0000
    DetonatorSeek = 32,         // 0010 0000
    DetonatorAvoidance = 64,    // 0100 0000
    TankAvoidance = 128,        // 1000 0000
    // Add additional steering behaviour types here
}

public abstract class SteeringBehaviour
{
    private float[] interest = null;
    private float[] danger = null;

    public abstract SteeringBehaviourType GetSteeringBehaviourType();

    public virtual float[] CalculateInterest(ref SteeringData steeringData, Vector3 position, List<Vector3> directions)
    {
        // If the interest array is null, create it, otherwise clear the elements to zero
        if (interest == null)
        {
            interest = new float[directions.Count];
        }
        else
        {
            System.Array.Clear(interest, 0, interest.Length);
        }

        return interest;
    }

    public virtual float[] CalculateDanger(ref SteeringData steeringData, Vector3 position, List<Vector3> directions)
    {
        // If the danger array is null, create it, otherwise clear the elements to zero
        if (danger == null)
        {
            danger = new float[directions.Count];
        }
        else
        {
            System.Array.Clear(danger, 0, danger.Length);
        }

        return danger;
    }
}
