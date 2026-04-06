using UnityEngine;

public class ObstacleDetector : Detector
{
    public override DetectorType GetDetectorType()
    {
        return DetectorType.Obstacle;
    }

    public override void Detect(ref SteeringData steeringData, Vector3 position)
    {
        // Get all the obstacles within the detection radius and add the colliders to the obstacles list
        Collider[] colliders = Physics.OverlapSphere(position, steeringData.Settings.obstacleDetectionRadius, steeringData.Settings.obstacleMask);
        steeringData.Obstacles.AddRange(colliders);
    }
}
