using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
 {
    #region 生命周期
    void Update () {
        SendAxisToServer();
        Move(DirX, DirY);
    }
    public BattleFieldManager GetManager()
    {
        return BattleFieldManager.Instance;
    }
    #endregion
    #region 移动
    internal short DirX;
    internal short DirY;
    private float rotationSpeed=10;
    private void SendAxisToServer()
    {
        if (GetComponent<PlayerModel>().isMe)
        {
            BattleFieldRequest.Instance.MoveRequest(
                (int)Math.Round(ETCInput.GetAxis("Horizontal")), 
                (int)Math.Round(ETCInput.GetAxis("Vertical")),
                transform.position);
        }
    }
    public void Move(int x,int y)
    {
        var speed = new Vector3(x, 0, y);
        //GetComponent<Transform>().LookAt(GetComponent<Transform>().position + speed);
        if (GetComponent<PlayerAttack>().isAttcking==true)
        {
            return;
        }
        //控制 nav agent移动
        GetComponent<NavMeshAgent>().velocity = new Vector3(x, 0, y) *3f;
        //切换动画状态机
        GetComponent<Animator>().SetFloat("speed", speed.magnitude);

        //速度小于0的时候，就不旋转。
        if (speed.magnitude==0)
        {
            return;
        }
        //旋转
        Quaternion targetRotion;
        targetRotion = Quaternion.LookRotation(GetComponent<Transform>().position + speed - transform.position);
        if (targetRotion != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotion, rotationSpeed * Time.deltaTime);
        }

    }
    #endregion
    //c# 属性
    public PlayerModel model
    {
        get
        {
            return GetComponent<PlayerModel>();
        }
    }









}
