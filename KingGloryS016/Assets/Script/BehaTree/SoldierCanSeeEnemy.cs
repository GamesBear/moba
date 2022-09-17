using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class SoldierCanSeeEnemy : Conditional
{
    public Transform[] targets;//判断是否在视野内的目标
    public float fieldOfViewAngle = 90;
    //public float viewDistance = 7;
    public SharedFloat sharedViewDistance;

    public SharedTransform target;

    public override TaskStatus OnUpdate()
    {
        if (GetSoldier().GetCurTarget() == null) return TaskStatus.Failure;
        return TaskStatus.Success;
    }


    Soldier GetSoldier()
    {
        return GetComponent<Soldier>();
    }
}
