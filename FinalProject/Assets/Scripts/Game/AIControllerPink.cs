using Unity.Behavior;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AIControllerPink : AIController
{
    private Tank tank;
    private SteeringContext steeringContext;
    private BehaviorGraphAgent behaviorAgent;

    private BlackboardVariable<PinkState> State;

    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Tank>();
        steeringContext = GetComponent<SteeringContext>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();

        behaviorAgent.SetVariableValue<float>("TargetDetectionRadius", steeringContext.settings.targetDetectionRadius);
    }

    private void Update()
    {
        steeringContext.Detect(DetectorType.Obstacle | DetectorType.Barrel | DetectorType.Tank | DetectorType.Detonator);

        behaviorAgent.GetVariable<PinkState>("PinkState", out State);

        if (State.Value == PinkState.None)
        {
            tank.TurretRotation = 0.0f;
            tank.TankRotation = 0.0f;
            tank.ForwardMovement = 0.0f;
        }
        else
        {
            Vector3 direction = Vector3.zero;

            switch (State.Value)
            {
                case PinkState.BarrelSeek:
                    {
                        direction = steeringContext.Solve(SteeringBehaviourType.BarrelSeek);
                        Debug.Log(direction);
                        HandleBarrelSeek(direction);
                    }
                    
                    break;
                case PinkState.Defence:
                    direction = steeringContext.Solve(SteeringBehaviourType.ObstacleAvoidance | SteeringBehaviourType.Fleeing);
                    break;
                case PinkState.OffenceBlue:
                    direction = steeringContext.Solve(SteeringBehaviourType.ObstacleAvoidance | SteeringBehaviourType.Seeking);
                    break;
                case PinkState.OffenceGreen:
                    direction = steeringContext.Solve(SteeringBehaviourType.ObstacleAvoidance | SteeringBehaviourType.Seeking);
                    break;
                case PinkState.OffenceWhite:
                    direction = steeringContext.Solve(SteeringBehaviourType.ObstacleAvoidance | SteeringBehaviourType.Seeking);
                    break;
            }


            
        }
    }

    void HandleBarrelSeek(Vector3 direction)
    {
        Vector2 a = new Vector2(direction.x, direction.z);

        Vector3 barrelDir = tank.projectileSpawnTransform.TransformDirection(Vector3.forward);

        Vector2 b = new Vector2(barrelDir.x, barrelDir.z);
        float degreesA = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
        float degreesB = Mathf.Atan2(b.y, b.x) * Mathf.Rad2Deg;
        float difference = degreesB - degreesA;

        // Normalize to the range (0, 360)
        difference = difference % 360.0f;
        if (difference < 0.0f)
        {
            difference += 360.0f;
        }

        // Normalize to the range (-180, 180)
        if (difference > 180.0f)
        {
            difference -= 360.0f;
        }

        // Move and rotate the movement agent based on the alignment
        float sign = Mathf.Sign(difference);
        float alignment = Vector2.Dot(a, b);
        if (alignment < 0.9999f)
        {
            tank.TurretRotation = sign;
            tank.TankRotation = sign;
            tank.ForwardMovement = 0.0f;
        }
        else
        {
            tank.TurretRotation = 0.0f;
            tank.TankRotation = 0.0f;
            tank.ForwardMovement = 1.0f;

            tank.FireProjectile();
            //Turret aligned, fire projectile

        }

        //Debug.Log("allignment: " + alignment + ", sign: " + sign + ", difference: " + difference + ", degreesA: " + degreesA + ", degreesb: " + degreesB);
    }
}
