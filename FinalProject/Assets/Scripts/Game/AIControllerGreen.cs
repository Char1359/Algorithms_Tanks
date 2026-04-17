using Unity.Behavior;
using UnityEngine;

public class AIControllerGreen : AIController
{
    private Tank tank;
    private SteeringContext steeringContext;
    private BehaviorGraphAgent behaviorAgent;

    private BlackboardVariable<GreenState> State;

    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Tank>();
        steeringContext = GetComponent<SteeringContext>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
    }

    private void Update()
    {
        steeringContext.Detect(DetectorType.Obstacle | DetectorType.Barrel | DetectorType.Tank | DetectorType.Detonator);

        behaviorAgent.GetVariable<GreenState>("GreenState", out State);

        if (State.Value == GreenState.None)
        {
            tank.TurretRotation = 0;
            tank.ForwardMovement = 0;
            tank.TankRotation = 0;
        }
        else
        {

            Vector3 turretDirection = Vector3.zero;
            Vector3 tankDirection = Vector3.zero;

            switch (State.Value)
            {
                case GreenState.Flee:
                    tankDirection = steeringContext.Solve(SteeringBehaviourType.Fleeing);
                    break;

                case GreenState.Wander:
                    tankDirection = steeringContext.Solve(SteeringBehaviourType.Wandering);
                    break;

                case GreenState.Seek:
                    tankDirection = steeringContext.Solve(SteeringBehaviourType.DetonatorSeek);
                    break;
            }

        }
    }
}
