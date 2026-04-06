using UnityEngine;

public class DetonatorDetector : Detector
{
    public override DetectorType GetDetectorType()
    {
        return DetectorType.Detonator;
    }

    public override void Detect(ref SteeringData steeringData, Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, steeringData.Settings.detonatorDetectionRadius, steeringData.Settings.detonatorMask);
        steeringData.Detonators.AddRange(colliders);
    }
}
