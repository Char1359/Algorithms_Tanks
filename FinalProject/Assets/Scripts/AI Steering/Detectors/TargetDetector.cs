using UnityEngine;

public class TargetDetector : Detector
{
    public override DetectorType GetDetectorType()
    {
        return DetectorType.Target;
    }

    public override void Detect(ref SteeringData steeringData, Vector3 position)
    {
        // Local variables
        Vector3 displacement;
        Vector3 direction;
        RaycastHit raycastHit;
        bool success = false;

        // Get all the targets within the detection radius
        Collider[] targetColliders = Physics.OverlapSphere(position, steeringData.Settings.targetDetectionRadius, steeringData.Settings.targetMask);

        // Loop through the target colliders
        foreach (Collider collider in targetColliders)
        {
            // Calculate the displacement and the direction vectors
            displacement = collider.transform.position - position;
            direction = displacement.normalized;

            // Perform a Raycast to make sure an obstacle isn't blocking the target
            // If an obstacle is in fact blocking the target, then the enemy can't see it and we won't consider it
            success = Physics.Raycast(position, direction, out raycastHit, steeringData.Settings.targetDetectionRadius, steeringData.Settings.targetMask | steeringData.Settings.obstacleMask);

            // Ensure that the collider is on the Target collision layer
            if (success && raycastHit.collider != null && steeringData.IsTargetBitEnabled(raycastHit.collider.gameObject.layer))
            {
                if (steeringData.Settings.gizmosDrawTargetDetectorDebugRay)
                {
                    Debug.DrawRay(position, direction * steeringData.Settings.targetDetectionRadius, steeringData.Settings.gizmosDetectedTargetsColor);
                }

                steeringData.Targets.Add(collider);
            }
        }
    }
}
