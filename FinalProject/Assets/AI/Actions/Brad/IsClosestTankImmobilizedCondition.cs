using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsClosestTankImmobilized", story: "[Self] checks if [T1] , [T2] or [T3] are closest to [detonator] and immobilized", category: "Conditions", id: "e6584efd9ed97720772ac5409d8f9f76")]
public partial class IsClosestTankImmobilizedCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> T1;
    [SerializeReference] public BlackboardVariable<GameObject> T2;
    [SerializeReference] public BlackboardVariable<GameObject> T3;
    [SerializeReference] public BlackboardVariable<GameObject> Detonator;

    public override bool IsTrue()
    {
        if (Detonator == null || Detonator.Value == null)
        {
            return false;
        }
        Vector3 detonatorPos = Detonator.Value.transform.position;
        float closestDistance = float.MaxValue; //Stack Overflow (Really cool value setter)
        Tank closestTank = null;

        void DistanceCheck(BlackboardVariable<GameObject> tank)
        {
            if (tank == null || tank.Value ==  null)
            {
                return;
            }

            Tank temp = tank.Value.GetComponent<Tank>();
            if (temp == null)
            {
                return;
            }

            float dist = (temp.transform.position - detonatorPos).sqrMagnitude;

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTank = temp;
            }
        }

        DistanceCheck(T1);
        DistanceCheck(T2);
        DistanceCheck(T3);

        return closestTank != null && closestTank.IsImmobilized && closestTank.IsExploded == false;
    }
}
