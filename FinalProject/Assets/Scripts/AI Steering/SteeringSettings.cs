using UnityEngine;

[CreateAssetMenu(fileName = "SteeringSettings", menuName = "Scriptable Objects/SteeringSettings")]
public class SteeringSettings : ScriptableObject
{
    [Header("Layer Mask settings")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask barrelMask;
    public LayerMask tankMask;
    public LayerMask detonatorMask;

    [Header("Detection settings")]
    public float targetDetectionRadius = 15.0f;
    public float obstacleDetectionRadius = 10.0f;
    public float barrelDetectionRadius = 200.0f;
    public float tankDetectionRadius = 200.0f;
    public float detonatorDetectionRadius = 200.0f;

    [Header("Obstacle avoidance behaviour settings")]
    public float obstacleAvoidanceRadius = 2.5f;

    [Header("Wandering behaviour settings")]
    public float wanderingMinDistance = 6.0f;
    public float wanderingMaxDistance = 18.0f;
    public float wanderingTimeoutDuration = 5.0f;
    public float wanderingLocationThreshold = 3.5f;
    public Bounds wanderingBounds = new Bounds(Vector3.zero, new Vector3(45.0f, 1.0f, 55.0f));

    [Header("Seeking behaviour settings")]
    public float seekingTargetThreshold = 3.5f;

    [Header("Position settings")]
    public Vector3 positionOffset = new Vector3(0.0f, 2.0f, 0.0f);

    [Header("Gizmo settings")]
    public bool gizmosDrawTargetDetectorDebugRay = true;
    public bool gizmosDrawWanderingBounds = true;
    public bool gizmosDrawWanderingLocation = true;
    public bool gizmosDrawCurrentTarget = true;
    public bool gizmosDrawDetectedTargets = true;
    public bool gizmosDrawDetectedObstacles = true;
    public bool gizmosDrawDetectedBarrels = true;
    public bool gizmosDrawDetectedTanks = true;
    public bool gizmosDrawDetectedDetonators = true;
    public bool gizmosDrawTargetDetectorRadius = true;
    public bool gizmosDrawObstacleDetectorRadius = true;
    public bool gizmosDrawBarrelDetectorRadius = true;
    public bool gizmosDrawTankDetectorRadius = true;
    public bool gizmosDrawDetonatorDetectorRadius = true;
    public bool gizmosDrawInterestRays = true;
    public bool gizmosDrawDangerRays = true;
    public bool gizmosDrawPreferredDirectionRay = true;

    public float gizmosDangerRayMagnitude = 8.0f;
    public float gizmosInterestRayMagnitude = 8.0f;
    public float gizmosPreferredDirectionRayMagnitude = 8.0f;
    public float gizmosDefaultRayMagnitude = 4.0f;

    public Color gizmosDangerColor = Color.red;
    public Color gizmosInterstColor = Color.blue;
    public Color gizmosPreferredDirectionColor = Color.yellow;
    public Color gizmosDefaultColor = Color.green;
    public Color gizmosWanderingLocationColor = Color.magenta;
    public Color gizmosCurrentTargetColor = Color.turquoise;
    public Color gizmosDetectedTargetsColor = Color.purple;
    public Color gizmosDetectedObstaclesColor = Color.crimson;
    public Color gizmosDetectedBarrelColor = Color.beige;
    public Color gizmosDetectedTankColor = Color.brown;
    public Color gizmosDetectedDetonatorColor = Color.darkSlateGray;
}
