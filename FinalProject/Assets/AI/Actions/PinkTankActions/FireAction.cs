using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Fire", story: "[Agent] Fires", category: "Action", id: "c58ac707d8c70977c8e1fad97cf510ef")]
public partial class FireAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    protected override Status OnStart()
    {
        if (Agent != null)
        {
            Tank pinkTank = Agent.Value.GetComponent<Tank>();
            pinkTank.FireProjectile();

            return Status.Success;
        }
        return Status.Failure;
    }
}

