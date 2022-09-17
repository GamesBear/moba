using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class SolderSeekCurTarget : Action
{
    private float AttCD;

    public override TaskStatus OnUpdate()
    {
        var target = GetSoldier().GetCurTarget();
        if (target != null)
        {
            Debug.LogError("攻击当前目标：" + target.name);

            var dis = Vector3.Distance(transform.position, target.transform.position);
            if (dis < 3)
            {
                if (Time.time > AttCD)
                {
                    AttCD = Time.time + 2;
                    GetComponent<NavMeshAgent>().isStopped = true;
                    if (BattleFieldManager.Instance.Player1.model.group == GetSoldier().group)
                    {
                        BattleFieldRequest.Instance.HurtRequest(target.GetComponent<BodyModel>().id, 100, GetSoldier().GetComponent<BodyModel>().id);
                    }
                }
            }
            else
            {
                GetComponent<NavMeshAgent>().isStopped = false;
                GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
            }

            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }

    Soldier GetSoldier()
    {
        return GetComponent<Soldier>();
    }
}
