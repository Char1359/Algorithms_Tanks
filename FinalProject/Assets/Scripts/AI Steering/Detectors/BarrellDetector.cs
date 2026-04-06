using UnityEngine;

public class BarrelDetector : Detector
{
    public override DetectorType GetDetectorType()
    {
        return DetectorType.Barrel;
    }

    public override void Detect(ref SteeringData steeringData, Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, steeringData.Settings.barrelDetectionRadius, steeringData.Settings.barrelMask);
        steeringData.Barrels.AddRange(colliders);
    }
}
