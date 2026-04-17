using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;

public class AIControllerBlue : AIController
{
    private Tank tank;
    private SteeringContext steeringContext;
    private BehaviorGraphAgent behaviorAgent;

    private BlackboardVariable<State_Blue> State;
    private BlackboardVariable<bool> blueDetonatorExposed;

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

        behaviorAgent.GetVariable<bool>("BlueDetonator-IsSpawned", out blueDetonatorExposed);
        behaviorAgent.GetVariable<State_Blue>("State_Blue", out State);
        Debug.Log(State.Value);

        Vector3 turretDirection = Vector3.zero;
        Vector3 tankDirection = Vector3.zero;

        switch (State.Value)
        {
            case State_Blue.TargetBarrel:
                {
                    if (blueDetonatorExposed == true)
                    {
                        turretDirection = steeringContext.Solve(SteeringBehaviourType.BarrelSeek);
                        tankDirection = steeringContext.Solve(SteeringBehaviourType.DetonatorAvoidance);
                    }
                    else
                    {
                        turretDirection = steeringContext.Solve(SteeringBehaviourType.BarrelSeek);
                    }
                    HandleBarrelSeek(turretDirection, tankDirection);
                    break;
                }
            case State_Blue.TargetDetonator:
                if (blueDetonatorExposed == true)
                {
                    tankDirection = steeringContext.Solve(SteeringBehaviourType.DetonatorSeek | SteeringBehaviourType.DetonatorAvoidance);
                }
                else
                {
                    tankDirection = steeringContext.Solve(SteeringBehaviourType.DetonatorSeek);
                }
                HandleDetonatorSeek(tankDirection);
                break;
            case State_Blue.TargetGreen:
                if (blueDetonatorExposed == true)
                {
                    tankDirection = steeringContext.Solve(SteeringBehaviourType.DetonatorSeek | SteeringBehaviourType.DetonatorAvoidance);
                }
                else
                {
                    tankDirection = steeringContext.Solve(SteeringBehaviourType.DetonatorSeek);
                }
                break;
        }
    }

    void HandleDetonatorSeek(Vector3 direction)
    {
        Vector2 a = new Vector2(direction.x, direction.z);

        Vector3 tankDir = tank.transform.TransformDirection(Vector3.forward);

        Vector2 b = new Vector2(tankDir.x, tankDir.z);
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
        if (alignment < 0.98f)
        {
            tank.TurretRotation = 0.0f;
            tank.TankRotation = sign;
            tank.ForwardMovement = 0.0f;
        }
        else
        {
            tank.TurretRotation = 0.0f;
            tank.TankRotation = 0.0f;
            tank.ForwardMovement = 1.0f;
        }
    }


    void HandleBarrelSeek(Vector3 turDirection, Vector3 tankDirection)
    {
        Vector2 a = new Vector2(turDirection.x, turDirection.z);

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
            tank.TankRotation = 0.0f;
            tank.ForwardMovement = 0.0f;
        }
        else
        {
            tank.TurretRotation = 0.0f;
            tank.TankRotation = 0.0f;
            tank.ForwardMovement = 0.5f;

            tank.FireProjectile();
            
            //Turret aligned, fire projectile
        }
    }

    
}

