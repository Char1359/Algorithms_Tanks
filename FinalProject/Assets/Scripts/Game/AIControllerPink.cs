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
            Vector3 turretDirection = Vector3.zero;
            Vector3 tankDirection = Vector3.zero;

            switch (State.Value)
            {
                case PinkState.BarrelSeek:
                    {
                        turretDirection = steeringContext.Solve(SteeringBehaviourType.BarrelSeek);
                        //Debug.Log(direction);
                        HandleBarrelSeek(turretDirection);
                    }
                    break;
                    
                case PinkState.Offence:
                    turretDirection = steeringContext.Solve(SteeringBehaviourType.BarrelSeek);
                    tankDirection = steeringContext.Solve(SteeringBehaviourType.DetonatorSeek | SteeringBehaviourType.DetonatorAvoidancePink);
                    HandleDetonatorSeek(turretDirection, tankDirection);
                    break;
                case PinkState.D_BarrelSeek:
                    turretDirection = steeringContext.Solve(SteeringBehaviourType.BarrelSeek);
                    //Debug.Log(direction);
                    HandleBarrelSeek(turretDirection);
                    break;
                case PinkState.D_Offence:
                    turretDirection = steeringContext.Solve(SteeringBehaviourType.BarrelSeek);
                    tankDirection = steeringContext.Solve(SteeringBehaviourType.DetonatorSeek | SteeringBehaviourType.DetonatorAvoidancePink);
                    HandleDetonatorSeek(turretDirection, tankDirection);
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
    void HandleDetonatorSeek( Vector3 turretDirection, Vector3 tankDirection)
    {
        Vector2 a = new Vector2(tankDirection.x, tankDirection.z);

        Vector3 tankDir = tank.transform.TransformDirection(Vector3.forward);

        Vector2 b = new Vector2(tankDir.x, tankDir.z);
        float degreesA = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
        float degreesB = Mathf.Atan2(b.y, b.x) * Mathf.Rad2Deg;
        float TankDifference = degreesB - degreesA;

        // Normalize to the range (0, 360)
        TankDifference = TankDifference % 360.0f;
        if (TankDifference < 0.0f)
        {
            TankDifference += 360.0f;
        }

        // Normalize to the range (-180, 180)
        if (TankDifference > 180.0f)
        {
            TankDifference -= 360.0f;
        }

        // Move and rotate the movement agent based on the alignment
        float TankSign = Mathf.Sign(TankDifference);
        float TankAlignment = Vector2.Dot(a, b);
        if (TankAlignment < 0.98f)
        {
            tank.TankRotation = TankSign;
            tank.ForwardMovement = 0.5f;
        }
        else
        {
            tank.TankRotation = 0.0f;
            tank.ForwardMovement = 1.0f;
        }

        Vector2 y = new Vector2(turretDirection.x, turretDirection.z);

        Vector3 barrelDir = tank.projectileSpawnTransform.TransformDirection(Vector3.forward);

        Vector2 z = new Vector2(barrelDir.x, barrelDir.z);
        float degreesY = Mathf.Atan2(y.y, y.x) * Mathf.Rad2Deg;
        float degreesZ = Mathf.Atan2(z.y, z.x) * Mathf.Rad2Deg;
        float TurretDifference = degreesB - degreesA;

        // Normalize to the range (0, 360)
        TurretDifference = TurretDifference % 360.0f;
        if (TurretDifference < 0.0f)
        {
            TurretDifference += 360.0f;
        }

        // Normalize to the range (-180, 180)
        if (TurretDifference > 180.0f)
        {
            TurretDifference -= 360.0f;
        }

        // Move and rotate the movement agent based on the alignment
        float TurretSign = Mathf.Sign(TurretDifference);
        float TurretAlignment = Vector2.Dot(y, z);
        if (TurretAlignment < 0.99f)
        {
            tank.TurretRotation = TurretSign;
        }
        else
        {
            tank.TurretRotation = 0.0f;

            tank.FireProjectile();
            //Turret aligned, fire projectile

        }
    }
}
