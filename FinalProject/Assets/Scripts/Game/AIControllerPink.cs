using Unity.Behavior;
using UnityEngine;

public class AIControllerPink : AIController
{
    private Tank tank;
    private SteeringContext steeringContext;
    private BehaviorGraphAgent behaviorAgent;

    //private BlackboardVariable<SteeringMode_Pink> steeringMode;

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

        //behaviorAgent.GetVariable<SteeringMode_Pink>("SteeringMode_Pink", out steeringMode);

        //if (steeringMode.Value == SteeringMode_Pink.None)
        //{
        //    tank.TurretRotation = 0.0f;
        //    tank.TankRotation = 0.0f;
        //    tank.ForwardMovement = 0.0f;
        //}
        //else
        //{
        //    Vector3 direction = Vector3.zero;

        //    switch (steeringMode.Value)
        //    {
        //        case SteeringMode_Pink.SeekingBarrel:
        //            direction = steeringContext.Solve(SteeringBehaviourType.ObstacleAvoidance | SteeringBehaviourType.Seeking);
        //            break;

        //    }
        //}
    }
}
