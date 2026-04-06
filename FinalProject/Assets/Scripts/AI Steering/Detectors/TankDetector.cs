using UnityEngine;

public class TankDetector : Detector
{
    public override DetectorType GetDetectorType()
    {
        return DetectorType.Tank;
    }

    public override void Detect(ref SteeringData steeringData, Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, steeringData.Settings.tankDetectionRadius, steeringData.Settings.tankMask);
        steeringData.Tanks.AddRange(colliders);
    }
}
