using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SteeringContext : MonoBehaviour
{
    public SteeringSettings settings;
    public DirectionsHelper.Count numberOfDirections = DirectionsHelper.Count.Eight;

    private SteeringData steeringData;
    private SteeringMap steeringMap;
    private Dictionary<DetectorType, Detector> detectors;
    private Dictionary<SteeringBehaviourType, SteeringBehaviour> behaviours;
    private Vector3 preferredDirection;
    public DirectionsHelper.Count NumberOfDirections { get { return numberOfDirections; } }

    public int DirectionsCount { get { return (int)numberOfDirections; } }

    public List<Vector3> Directions
    {
        get
        {
            List<Vector3> directions = DirectionsHelper.EightDirections;

            if (numberOfDirections == DirectionsHelper.Count.Twelve)
            {
                directions = DirectionsHelper.TwelveDirections;
            }
            else if (numberOfDirections == DirectionsHelper.Count.Sixteen)
            {
                directions = DirectionsHelper.SixteenDirections;
            }

            return directions;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        steeringData = new SteeringData(settings);
        steeringMap = new SteeringMap(DirectionsCount);

        detectors = new Dictionary<DetectorType, Detector>();
        detectors.Add(DetectorType.Obstacle, new ObstacleDetector());
        detectors.Add(DetectorType.Target, new TargetDetector());
        detectors.Add(DetectorType.Barrel, new BarrelDetector());
        detectors.Add(DetectorType.Tank, new TankDetector());
        detectors.Add(DetectorType.Detonator, new DetonatorDetector());

        behaviours = new Dictionary<SteeringBehaviourType, SteeringBehaviour>();
        behaviours.Add(SteeringBehaviourType.ObstacleAvoidance, new ObstacleAvoidanceBehaviour());
        behaviours.Add(SteeringBehaviourType.Wandering, new WanderingBehaviour());
        behaviours.Add(SteeringBehaviourType.Seeking, new SeekingBehaviour());
        behaviours.Add(SteeringBehaviourType.Fleeing, new FleeingBehaviour());
        behaviours.Add(SteeringBehaviourType.BarrelSeek, new BarrelSeekingBehaviour());
        behaviours.Add(SteeringBehaviourType.DetonatorSeek, new DetonatorSeekingBehaviour());
        behaviours.Add(SteeringBehaviourType.DetonatorAvoidance, new  DetonatorAvoidanceBehaviour());
        behaviours.Add(SteeringBehaviourType.TankAvoidance, new TankAvoidanceBehaviour());
    }

    public void Detect(DetectorType detectorType)
    {
        // Reset the steering data
        steeringData.Reset();

        // Get the position and add an offset of 2 on the y-axis
        Vector3 position = transform.position + settings.positionOffset;

        // If the obstacle flag is set, execute the obstacle detector
        if (detectorType.HasFlag(DetectorType.Obstacle))
        {
            detectors[DetectorType.Obstacle].Detect(ref steeringData, position);
        }

        // If the target flag is set, execute the target detector
        if (detectorType.HasFlag(DetectorType.Target))
        {
            detectors[DetectorType.Target].Detect(ref steeringData, position);
        }

        // If the target flag is set, execute the barrel detector
        if (detectorType.HasFlag(DetectorType.Barrel))
        {
            detectors[DetectorType.Barrel].Detect(ref steeringData, position);
        }

        // If the target flag is set, execute the tank detector
        if (detectorType.HasFlag(DetectorType.Tank))
        {
            detectors[DetectorType.Tank].Detect(ref steeringData, position);
        }

        // If the target flag is set, execute the detonator detector
        if (detectorType.HasFlag(DetectorType.Detonator))
        {
            detectors[DetectorType.Detonator].Detect(ref steeringData, position);
        }
    }

    public Vector3 Solve(SteeringBehaviourType steeringBehaviourType)
    {
        // Reset the steering map
        steeringMap.Reset();

        // If the obstacle avoidance flag is set, execute the obstacle avoidance behaviour
        if (steeringBehaviourType.HasFlag(SteeringBehaviourType.ObstacleAvoidance))
        {
            ExecuteBehaviour(behaviours[SteeringBehaviourType.ObstacleAvoidance]);
        }

        // If the wandering flag is set, execute the wandering behaviour
        if (steeringBehaviourType.HasFlag(SteeringBehaviourType.Wandering))
        {
            ExecuteBehaviour(behaviours[SteeringBehaviourType.Wandering]);
        }

        // If the seeking flag is set, execute the seeking behaviour
        if (steeringBehaviourType.HasFlag(SteeringBehaviourType.Seeking))
        {
            ExecuteBehaviour(behaviours[SteeringBehaviourType.Seeking]);
        }

        // If the fleeing flag is set, execute the fleeing behaviour
        if (steeringBehaviourType.HasFlag(SteeringBehaviourType.Fleeing))
        {
            ExecuteBehaviour(behaviours[SteeringBehaviourType.Fleeing]);
        }

        // If the seeking Barrel flag is set, execute the seeking behaviour
        if (steeringBehaviourType.HasFlag(SteeringBehaviourType.BarrelSeek))
        {
            ExecuteBehaviour(behaviours[SteeringBehaviourType.BarrelSeek]);
        }

        if (steeringBehaviourType.HasFlag(SteeringBehaviourType.DetonatorSeek))
        {
            ExecuteBehaviour(behaviours[SteeringBehaviourType.DetonatorSeek]);
        }

        if (steeringBehaviourType.HasFlag(SteeringBehaviourType.DetonatorAvoidance))
        {
            ExecuteBehaviour(behaviours[SteeringBehaviourType.DetonatorAvoidance]);
        }

        if (steeringBehaviourType.HasFlag(SteeringBehaviourType.TankAvoidance))
        {
            ExecuteBehaviour(behaviours[SteeringBehaviourType.TankAvoidance]);
        }

        // Combine the interst and danger steering maps
        steeringMap.Solve();

        // Take the results steering map and get the average direction
        Vector3 direction = Vector3.zero;
        for (int i = 0; i < DirectionsCount; i++)
        {
            direction += Directions[i] * steeringMap.Result[i];
        }

        // Normalized the average direction vector to get the preferred direction and then return it
        preferredDirection = direction.normalized;
        return preferredDirection;
    }

    private void ExecuteBehaviour(SteeringBehaviour steeringBehaviour)
    {
        // Get the position and add an offset of 2 on the y-axis
        Vector3 position = transform.position + settings.positionOffset;

        // Calculate the interest and danger steering maps from the steering behaviour
        float[] interest = steeringBehaviour.CalculateInterest(ref steeringData, position, Directions);
        float[] danger = steeringBehaviour.CalculateDanger(ref steeringData, position, Directions);

        // Apply the interest and danger steering maps from this steering behaviour to the master steering map
        steeringMap.Apply(interest, danger);
    }

    private void OnDrawGizmos()
    {
        if (settings != null)
        {
            Vector3 position = transform.position + settings.positionOffset;

            // Draw obstacle detection sphere
            if (settings.gizmosDrawObstacleDetectorRadius)
            {
                Gizmos.color = settings.gizmosDangerColor;
                Gizmos.DrawWireSphere(position, settings.obstacleDetectionRadius);
            }

            // Draw target detection sphere
            if (settings.gizmosDrawTargetDetectorRadius)
            { 
                Gizmos.color = settings.gizmosInterstColor;
                Gizmos.DrawWireSphere(position, settings.targetDetectionRadius);
            }

            // Draw barrel detection sphere
            if (settings.gizmosDrawBarrelDetectorRadius)
            {
                Gizmos.color = settings.gizmosDetectedBarrelColor;
                Gizmos.DrawWireSphere(position, settings.barrelDetectionRadius);
            }

            // Draw tank detection sphere
            if (settings.gizmosDrawTankDetectorRadius)
            {
                Gizmos.color = settings.gizmosDetectedTankColor;
                Gizmos.DrawWireSphere(position, settings.tankDetectionRadius);
            }

            // Draw detonator detection sphere
            if (settings.gizmosDrawDetonatorDetectorRadius)
            {
                Gizmos.color = settings.gizmosDetectedDetonatorColor;
                Gizmos.DrawWireSphere(position, settings.detonatorDetectionRadius);
            }

            // Draw the wandering bounds
            if (settings.gizmosDrawWanderingBounds)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(settings.wanderingBounds.center, settings.wanderingBounds.size);
            }

            if (Application.isPlaying)
            {
                // Draw the interest rays
                if (settings.gizmosDrawInterestRays)
                {
                    Gizmos.color = settings.gizmosDangerColor;
                    for (int i = 0; i < DirectionsCount; i++)
                    {
                        Gizmos.DrawRay(position, Directions[i] * steeringMap.danger[i] * settings.gizmosDangerRayMagnitude);
                    }
                }

                // Draw the danger rays
                if (settings.gizmosDrawDangerRays)
                {
                    Gizmos.color = settings.gizmosInterstColor;
                    for (int i = 0; i < DirectionsCount; i++)
                    {
                        Gizmos.DrawRay(position, Directions[i] * steeringMap.interest[i] * settings.gizmosInterestRayMagnitude);
                    }
                }

                // Draw the wandering location
                if (settings.gizmosDrawWanderingLocation && steeringData.WanderLocation.HasValue)
                {
                    Gizmos.color = settings.gizmosWanderingLocationColor;
                    Gizmos.DrawSphere(steeringData.WanderLocation.Value, 1.25f);
                }

                // Draw the current target
                if (settings.gizmosDrawCurrentTarget && steeringData.CurrentTarget != null)
                {
                    Gizmos.color = settings.gizmosCurrentTargetColor;
                    Gizmos.DrawSphere(steeringData.CurrentTarget.position, 1.25f);
                }

                // Draw the detected targets
                if (settings.gizmosDrawDetectedTargets)
                {
                    Gizmos.color = settings.gizmosDetectedTargetsColor;
                    foreach (Collider target in steeringData.Targets)
                    {
                        Gizmos.DrawSphere(target.transform.position, 1.0f);
                    }
                }

                // Draw the detected obstacles
                if (settings.gizmosDrawDetectedObstacles)
                {
                    Gizmos.color = settings.gizmosDetectedObstaclesColor;
                    foreach (Collider obstacle in steeringData.Obstacles)
                    {
                        Gizmos.DrawSphere(obstacle.transform.position, 1.0f);
                    }
                }

                // Draw the detected barrels
                if (settings.gizmosDrawDetectedBarrels)
                {
                    Gizmos.color = settings.gizmosDetectedBarrelColor;
                    foreach (Collider barrel in steeringData.Barrels)
                    {
                        Gizmos.DrawSphere(barrel.transform.position + new Vector3(0.0f, 5.0f, 0.0f), 1.0f);
                    }
                }

                // Draw the detected Tanks
                if (settings.gizmosDrawDetectedTanks)
                {
                    Gizmos.color = settings.gizmosDetectedTankColor;
                    foreach (Collider tank in steeringData.Tanks)
                    {
                        Gizmos.DrawSphere(tank.transform.position + new Vector3(0.0f, 5.0f, 0.0f), 2.5f);
                    }
                }

                // Draw the detected Detonators
                if (settings.gizmosDrawDetectedDetonators)
                {
                    Gizmos.color = settings.gizmosDetectedDetonatorColor;
                    foreach (Collider detonator in steeringData.Detonators)
                    {
                        Gizmos.DrawSphere(detonator.transform.position + new Vector3(0.0f, 5.0f, 0.0f), 1.0f);
                    }
                }

                // Draw the preferred direction ray
                if (settings.gizmosDrawPreferredDirectionRay)
                {
                    Gizmos.color = settings.gizmosPreferredDirectionColor;
                    Gizmos.DrawRay(position, preferredDirection * settings.gizmosPreferredDirectionRayMagnitude);
                }
            }
            else
            {
                // Draw the default directions
                Gizmos.color = settings.gizmosDefaultColor;
                DirectionsHelper.DrawGizmo(position, settings.gizmosDefaultRayMagnitude, numberOfDirections);
            }
        }
    }
}
