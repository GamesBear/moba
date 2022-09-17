using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : BodyModel
{
    public List<GameObject> FinalTargets;
    public List<GameObject> CurTargets;

    void Start()
    {
        BattleFieldManager.Instance.SoldierList.Add(this);
        HP = 300;
    }

    private void OnDestroy()
    {
        BattleFieldManager.Instance.SoldierList.Remove(this);
    }

    internal GameObject GetCurTarget()
    {
        if (CurTargets.Count == 0)
            return null;
        if (CurTargets[0]==null|| CurTargets[0].GetComponent<BodyModel>().isDead == true)
        {
            CurTargets.Remove(CurTargets[0]);
        }
        if (CurTargets.Count == 0)
            return null;
        return CurTargets[0];
    }

    public void Init(List<GameObject> target,int group,int id)
    {
        this.FinalTargets = target;
        this.group = group;
        this.id = id;
    }

    internal GameObject FinalTarget()
    {
        //如果最近的目标死亡，那么从目标中移除
        if (FinalTargets[0].GetComponent<BodyModel>().isDead==true)
        {
            FinalTargets.Remove(FinalTargets[0]);
        }
        return FinalTargets[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        var body = other.GetComponent<BodyModel>();
        //是否已经死亡，是否是对面的组
        if (body.isDead == false && body.group != group)
        {
            print("添加cur目标：" + body.group + ":" + group + ":" + body.gameObject.name);
            CurTargets.Add(body.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var body = other.GetComponent<BodyModel>();
        if (body.isDead == false && body.group != group)
        {
            print("退出cur目标：" + body.group + ":" + group + ":" + body.gameObject.name);
        }
    }

    internal void Dead()
    {
        isDead = true;
        Destroy(gameObject);
    }
    public override void SendHurtRequest(int hurtValue, int ObjectID)
    {
        BattleFieldRequest.Instance.HurtRequest(id, hurtValue, ObjectID);
    }
}
