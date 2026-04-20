using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class AIControllerWhite : AIController
{
    private Tank tank;
    private SteeringContext steeringContext;
    private BehaviorGraphAgent behaviorAgent;

    private BlackboardVariable<WhiteState> State;

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
        behaviorAgent.GetVariable<WhiteState>("WhiteState", out State);

        Vector3 CannonDirection = Vector3.zero;
        Vector3 TankDirection = Vector3.zero;

        switch (State.Value)
        {
            case WhiteState.None:
                Debug.Log("None!!");
                HandleReset();
                break;
                
            case WhiteState.Attack:
                Debug.Log("Attack The Barrels!!");
                HandleAttack(TankDirection);
                break;
                
            case WhiteState.Defend:                                 
                Debug.Log("Defend The Detonator!!");
                HandleDefend(TankDirection);                   
                break;
                
            case WhiteState.Rush:                                    
                Debug.Log("Found a Detonator!!");
                HandleRush(TankDirection);                                            
                break;                        
        }
    }


    void HandleReset()
    {
        tank.TurretRotation = 0.0f;
        tank.TankRotation = 0.0f;
        tank.ForwardMovement = 0.0f;
    }

    //Shoots at the nearest barrel
    //Moves toward the nearest barrel outside of firing range
    void HandleAttack(Vector3 TankDirection)
    {
        TankDirection = steeringContext.Solve(SteeringBehaviourType.BarrelSeek);

        Vector2 a = new Vector2(TankDirection.x, TankDirection.z);
        Vector3 cannonDirection = tank.transform.TransformDirection(Vector3.forward);
        Vector2 b = new Vector2(cannonDirection.x, cannonDirection.z);

        float degreesA = Mathf.Atan2(a.y, a.x) + Mathf.Rad2Deg;
        float degreesB = Mathf.Atan2(b.y, b.x) + Mathf.Rad2Deg;
        float difference = degreesB - degreesA;

        difference = difference % 360.0f;
        if (difference < 0.0f)
        {
            difference += 360.0f;
        }
        if (difference > 180.0f)
        {
            difference -= 360.0f;
        }

        float sign = Mathf.Sign(difference);
        float alignment = Vector2.Dot(a, b);

        if (alignment < 0.99f)
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
        tank.FireProjectile();
    }

    //Shoots at the nearest tank to the exposed white detonator
    //Moves toward the exposed white detonator
    //avoids driving over the exposed white detonator
    void HandleDefend(Vector3 TankDirection, Vector3 CannonDirection)
    {
        TankDirection = steeringContext.Solve(SteeringBehaviourType.Seeking | SteeringBehaviourType.DetonatorAvoidance);
        CannonDirection = steeringContext.Solve(SteeringBehaviourType.BarrelSeek);    
    }

    //moves towards the nearest exposed enemy detonator
    void HandleRush(Vector3 TankDirection)
    {
        TankDirection = steeringContext.Solve(SteeringBehaviourType.DetonatorSeek);

        Vector2 a = new Vector2(TankDirection.x, TankDirection.z);
        Vector3 direction = tank.transform.TransformDirection(Vector3.forward);
        Vector2 b = new Vector2(direction.x, direction.z);
        float degreesA = Mathf.Atan2(a.y, a.x) + Mathf.Rad2Deg;
        float degreesB = Mathf.Atan2(b.y, b.x) + Mathf.Rad2Deg;
        float difference = degreesB - degreesA;

        difference = difference % 360.0f;
        if (difference < 0.0f)
        {
            difference += 360.0f;
        }
        if (difference > 180.0f)
        {
            difference -= 360.0f;
        }

        float sign = Mathf.Sign(difference);
        float alignment = Vector2.Dot(a, b);
        if (alignment < 0.99f)
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
}