using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Spawned And Alive", story: "Checks if [detonator] is spawned and [tank] is alive returns true if detonator spawned and tank is not dead", category: "Variable Conditions", id: "369b539a80b9d2ef9cc63c1459aace69")]
public partial class SpawnedAndAliveCondition : Condition
{
    [Comparison(comparisonType: ComparisonType.Boolean)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> Detonator;
    [Comparison(comparisonType: ComparisonType.Boolean)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> Tank;

    public override bool IsTrue()
    {
        //if(Detonator.Value == true && Tank.Value == false)
        //{
            return true;
        //}
        //return false;
    }

}
