using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckDetonatorsCondition", story: "[Self] checks if any [D1] , [D2] or [D3] are set", category: "Conditions", id: "7c72526e51479950b4158fc93d903eea")]
public partial class CheckDetonatorsCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> D1;
    [SerializeReference] public BlackboardVariable<GameObject> D2;
    [SerializeReference] public BlackboardVariable<GameObject> D3;

    public override bool IsTrue()
    {
        if (D1.Value != null)
        {
            return true;
        }
        if (D2.Value != null)
        {
            return true;
        }
        if (D3.Value != null)
        {
            return true;
        }
        return false;
    }
}
