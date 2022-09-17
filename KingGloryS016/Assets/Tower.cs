using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : BodyModel {


    private List<GameObject> targets = new List<GameObject>();
    public GameObject fireFB;
    private GameObject fire;

    private GameObject guanghuan;
    private GameObject TowerDeathB;
    private LineRenderer Line;

    public GameObject Tower3DModel;
    private int attVal = 200;

    void Start () {
        //注册自己
        BattleFieldManager.Instance.TowerList.Add(this);

        Line = transform.Find("Line").gameObject.GetComponent<LineRenderer>();
        guanghuan = transform.Find("guangquan_jianta").gameObject;
        TowerDeathB = transform.Find("TowerDeathB").gameObject;

        guanghuan.SetActive(false);
    }
	
	void Update () {
        if (GetTarget() != null)
        {
            Line.gameObject.SetActive(true);
            Line.SetPosition(0, transform.position + new Vector3(0, 5.6f, 0));
            Line.SetPosition(1, GetTarget().transform.position + new Vector3(0, 1.5f, 0));
        }
        else
        {
            Line.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BodyModel>() == null)
        {
            return;
        }

        //判断进来的人，是不是敌人
        if (isDead|| group == other.GetComponent<BodyModel>().group)
        {
            return;
        }
        targets.Add(other.gameObject);
        //只有添加第一个对象的时候，才攻击
        if (targets.Count==1)
        {
            Attack();
        }

        //显示攻击范围,只有player1才显示
        if (other.GetComponent<PlayerModel>() != null&& GetTarget() == BattleFieldManager.Instance.Player1.gameObject)
        {
            guanghuan.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targets.IndexOf(other.gameObject)>=-1)
        {
            targets.Remove(other.gameObject);
        }

        if (other.gameObject == BattleFieldManager.Instance.Player1.gameObject)
        {
            guanghuan.SetActive(false);
        }
    }

    private void Attack()
    {
        if (isDead) { return; }
        //创建火球
        fire = Instantiate(fireFB, transform.position + new Vector3(0, 5.6f, 0), Quaternion.identity);
        fire.GetComponent<FireBallMove>().target = this.GetTarget();
        fire.GetComponent<FireBallMove>().TouchCallback = OnTouchCallback;
    }

    private void OnTouchCallback(GameObject obj)
    {
        //再进行攻击
        if (GetTarget() != null)
        {
            if (GetTarget().GetComponent<BodyModel>().HP > attVal)
            {
                Attack();
            }else if (targets.Count>=2)
            {
                //如果这次能够打死他，直接从目标中移除，打下一个
                targets.Remove(targets[0]);
                Attack();
            }//如果打死，并且没有下一个目标了，不攻击
        }

        //判断被打中的人，是不是自己
        if (BattleFieldManager.Instance.Player1.model.group
            != obj.GetComponent<BodyModel>().group)
        {
            return;
        }
        print("喷到了，要做一些什么呢");
        BattleFieldRequest.Instance.HurtRequest(obj.GetComponent<BodyModel>().id, attVal, id);

    }

    public override void SendHurtRequest(int hurtValue, int ObjectID)
    {
        BattleFieldRequest.Instance.HurtRequest(id, hurtValue, ObjectID);
    }

    internal void PlayDestroy()
    {
        print("tower boom");

        TowerDeathB.GetComponent<ParticleSystem>().Play();
        Tower3DModel.transform.Translate(Vector3.up * 3);

        enabled = false;
        guanghuan.SetActive(false);
        Line.gameObject.SetActive(false);

        isDead = true;
    }
    GameObject GetTarget()
    {
        if (targets.Count == 0)
            return null;
        if (targets[0] == null || targets[0].GetComponent<BodyModel>().isDead == true)
        {
            targets.Remove(targets[0]);
            if (targets.Count == 0)
                return null;
        }
        return targets[0];

    }
}
