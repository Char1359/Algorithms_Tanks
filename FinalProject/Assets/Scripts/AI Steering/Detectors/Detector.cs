using System;
using UnityEngine;


[Flags]
public enum DetectorType
{
    None = 0,
    Obstacle = 1,   // 0000 0001
    Target = 2,     // 0000 0010
    Barrel = 4,     // 0000 0100
    Tank = 8,       // 0000 1000
    Detonator = 16, // 0001 0000

    // Add additional detector types here

    All = Obstacle | Target | Barrel
}

public abstract class Detector
{
    public abstract DetectorType GetDetectorType();

    public abstract void Detect(ref SteeringData steeringData, Vector3 position);
}
