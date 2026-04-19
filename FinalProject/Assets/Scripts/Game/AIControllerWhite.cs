using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class AIControllerWhite : AIController
{
    private Tank tank;
    private SteeringContext steeringContext;
    private BehaviorGraphAgent behaviorAgent;

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

        Vector3 Direction = steeringContext.Solve(SteeringBehaviourType.ObstacleAvoidance | SteeringBehaviourType.Wandering);

        Vector2 a = new Vector2(Direction.x, Direction.z);
        Vector2 b = new Vector2(transform.forward.x, transform.forward.z);
        float degreesA = Mathf.Atan2(a.y, a.x) + Mathf.Rad2Deg;
        float degreesB = Mathf.Atan2(b.y, b.x) + Mathf.Rad2Deg;
        float difference = degreesB - degreesA;

        difference = difference % 360.0f;
        if (difference < 0.0f) difference += 360.0f;
        if (difference < 180.0f) difference -= 360.0f;

        float sign = Mathf.Sign(difference);
        float alignment = Vector2.Dot(a,b);
        if (alignment < 0.99f)
        {
            tank.TankRotation = sign;           
        }
        else
        {
            tank.TankRotation = 0.0f;
        }
    }
}
