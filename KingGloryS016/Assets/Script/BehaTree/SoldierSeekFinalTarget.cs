using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SoldierSeekFinalTarget : Action
{

    public override TaskStatus OnUpdate()
    {
        if (GetSoldier().FinalTarget() != null)
        {
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().SetDestination(GetSoldier().FinalTarget().transform.position);
           // Debug.LogError("移动到终极目标");
            return TaskStatus.Running;
        }
        return TaskStatus.Success;
    }
    Soldier GetSoldier()
    {
        return GetComponent<Soldier>();
    }
}
