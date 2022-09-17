using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region 攻击
    public GameObject Skill1Effect;
    public Transform Skill1Position;
    [HideInInspector]
    public int attackIndex;
    public GameObject Arrow;
    public GameObject HandPosition;
    [HideInInspector]
    //当前是否在攻击
    public bool isAttcking = false;
    void Start()
    {
        AddSkillFxListener();
    }
    void LateUpdate()
    {
        SetSkillFXPosition();
    }

    //c# 属性
    public PlayerModel model
    {
        get
        {
            return GetComponent<PlayerModel>();
        }
    }

    private void AddSkillFxListener()
    {
        //添加技能侦听
        var tm = GetComponentInChildren<RFX4_TransformMotion>(true);
        //添加事件侦听
        if (tm != null) tm.CollisionEnter += Tm_CollisionEnter;
    }
    private void SetSkillFXPosition()
    {
        //设置技能位置
        if (Skill1Effect != null && Skill1Position != null)
        {
            Skill1Effect.transform.position = Skill1Position.position;
        }
    }
    private void Tm_CollisionEnter(object sender, RFX4_TransformMotion.RFX4_CollisionInfo e)
    {
        if (e.Hit.transform.tag == "Player")
        {
            //发送请求，伤害
            Debug.Log(e.Hit.transform.name);
            BattleFieldRequest.Instance.HurtRequest(e.Hit.transform.GetComponent<PlayerModel>().id, 100, model.id);
        }
    }
    public void OnAttackBtn()
    {
        if (GetTarget(GetManager().playerList))
        {
            return;
        }
        if (GetTarget(GetManager().TowerList))
        {
            return;
        }
        if (GetTarget(GetManager().SoldierList))
        {
            return;
        }
    }

    private bool GetTarget(List<BodyModel> list)
    {
        //获取到攻击对象，如果有，发送攻击指令
        foreach (var item in list)
        {
            if (item.isDead == false && item.group != model.group)
            {
                var dis = Vector3.Distance(item.transform.position, this.transform.position);
                if (dis < 7f)
                {
                    attackIndex = item.id;
                    //发送攻击请求
                    BattleFieldRequest.Instance.AttackRequest(model.id, attackIndex, Common.AttackType.Normal);
                    GetComponent<Transform>().LookAt(item.GetComponent<Transform>().position);
                    return true;
                }
            }
        }
        return false;
    }

    public void OnAttackAnimation()
    {
        //弓箭实例化
        var obj = Instantiate(Arrow, HandPosition.transform.position, transform.rotation);
        var arrow = obj.GetComponent<Arrow>();
        arrow.Owner = model.id;


        if (Mathf.Round(attackIndex / 1000) == 2)
        {
            arrow.target = GetManager().GetTowerByID(attackIndex);
        }
        else if(Mathf.Round(attackIndex / 1000) == 6)
        {
            arrow.target = GetManager().GetSoldierByID(attackIndex);
        }else
        {
            arrow.target = GetManager().GetPlayerByID(attackIndex).model;
        }


        isAttcking = false;

    }
    public void OnSkill1Anim()
    {
        if (Skill1Effect == null) return;
        Skill1Effect.SetActive(true);
        print("准备发射技能");
        isAttcking = false;
    }
    internal void PlayAttack(int target)
    {
        //切换动画状态机
        GetComponent<Animator>().SetTrigger("attack");
        isAttcking = true;
        attackIndex = target;
    }
    internal void OnSkill1()
    {
        //获取到攻击对象，如果有，发送攻击指令
        foreach (var item in GetManager().playerList)
        {
            if (item != this)
            {
                var dis = Vector3.Distance(item.transform.position, this.transform.position);
                if (dis < 7f)
                {
                    attackIndex = item.GetComponent<PlayerModel>().id;
                    //发送攻击请求
                    BattleFieldRequest.Instance.AttackRequest(model.id, attackIndex, Common.AttackType.Skill1);

                    GetComponent<Transform>().LookAt(item.GetComponent<Transform>().position);
                }
            }
        }
    }
    internal void PlaySkill(int target)
    {
        var item = GetManager().GetPlayerByID(target);
        //切换动画状态机
        GetComponent<Animator>().SetTrigger("skill1");
        isAttcking = true;
        attackIndex = target;

        Skill1Effect.GetComponent<Transform>().LookAt(item.transform.position + Vector3.up);
    }

    #endregion

    public BattleFieldManager GetManager()
    {
        return BattleFieldManager.Instance;
    }
}
