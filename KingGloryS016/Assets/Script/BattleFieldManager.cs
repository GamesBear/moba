using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    public static BattleFieldManager Instance;
    public int MyPlayerIndex = -1;

    public GameObject Hero;
    public List<Transform> H1Pos;

    public List<BodyModel> playerList = new List<BodyModel>();
    public List<BodyModel> TowerList = new List<BodyModel>();
    public List<BodyModel> SoldierList = new List<BodyModel>();
    public PlayerMove Player1;
    public GameObject attFx;

    public Action OnInitCallbak;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void InitBattleField(int playerIndex)
    {
        //初始化场景
        MyPlayerIndex = playerIndex;
        //开始实例化
        Player1 = (Instantiate(Hero, H1Pos[MyPlayerIndex - 1].position, Quaternion.identity) as GameObject).GetComponent<PlayerMove>();
        Player1.model.id = MyPlayerIndex;
        Player1.model.group = MyPlayerIndex;
        Player1.model.isMe = true;

        playerList.Add(Player1.model);

        Camera.main.GetComponent<CameraFollow>().Init(Player1.gameObject);

        OnInitCallbak();
    }

    internal void AddPlayer(string allPlayer)
    {
        //解析
        var arr = allPlayer.Split(',');
        foreach (var item in arr)//遍历在场玩家的index
        {
            try//最后多了一个空格，所以转换成int会失败，不过这里忽略不计
            {
                //转换成int
                int i = Convert.ToInt16(item);

                if (i != MyPlayerIndex)
                {
                    print("不是自己：" + i);
                    PlayerMove p = (Instantiate(Hero, H1Pos[i - 1].position, Quaternion.identity) as GameObject).GetComponent<PlayerMove>();
                    p.model.id = i;
                    p.model.group = i;
                    playerList.Add(p.model);
                }
            }
            catch (Exception)
            {

            }

        }
    }

    internal void AddOnePlayer(int index)
    {
        //解析
        try//最后多了一个空格，所以转换成int会失败，不过这里忽略不计
        {
            if (index != MyPlayerIndex)
            {
                print("不是自己：" + index);
                PlayerMove p = (Instantiate(Hero, H1Pos[index - 1].position, Quaternion.identity) as GameObject).GetComponent<PlayerMove>();
                p.model.id = index;
                p.model.group = index;
                playerList.Add(p.model);
            }
        }
        catch (Exception)
        {

        }

    }

    internal void MovePlayer(short index, short x, short y, float Px, float Py, float Pz)
    {
        foreach (var item in playerList)
        {
            //拿到对应index的对象
            if (item.id == index)
            {
                var move = item.GetComponent<PlayerMove>();
                var v = new Vector3(move.DirX, move.DirY)*0.05f;
                //设置移动方向
                move.DirX = x;
                move.DirY = y;
                item.transform.position = new Vector3(Px+v.x, Py, Pz + v.y);
            }
        }
    }

    internal PlayerMove GetPlayerByID(int attIndex)
    {
        //通过id找到对象
        foreach (var item in playerList)
        {
            if (item.id == attIndex)
            {
                return item.GetComponent<PlayerMove>();
            }
        }
        return null;
    }
    internal Soldier GetSoldierByID(int attIndex)
    {
        //通过id找到对象
        foreach (var item in SoldierList)
        {
            if (item.id == attIndex)
            {
                return item.GetComponent<Soldier>();
            }
        }
        return null;
    }

    internal Tower GetTowerByID(int attIndex)
    {
        //通过id找到对象
        foreach (var item in TowerList)
        {
            if (item.id == attIndex)
            {
                return item.GetComponent<Tower>();
            }
        }
        return null;
    }


    internal void PlayAtt(int player, int target, int type)
    {

        print("PlayAtt:" + player + "type:" + type);
        var item = GetPlayerByID(player);

        if (type == (int)Common.AttackType.Normal)
        {
            //播放普通攻击动画
            item.GetComponent<PlayerAttack>().PlayAttack(target);
        }
        else if (type == (int)Common.AttackType.Skill1)
        {
            //播放普通攻击动画
            item.GetComponent<PlayerAttack>().PlaySkill(target);
        }
    }

    internal void PlayDestory(int index, int exp, int objectID)
    {
        if (Mathf.Round(index / 1000) == 2)
        {
            //防御塔
            print("xiaohhui:" + index);
            var item = GetTowerByID(index);
            item.PlayDestroy();
        }
        print("objectID:"+objectID);
        if (Mathf.Round(objectID / 1000) == 0)
        {
           var item = GetPlayerByID(objectID);
           item.model.ExpUp(exp);
        }

    }

    internal void Hurt(int index, int hp, int ObjectID)
    {
        //如果受击者是 炮塔
        if (Mathf.Round(index / 1000) == 2)
        {
            print(index + "：准备扣血");
            //拿到角色，扣血
            //扣血
            var item = GetTowerByID(index);
            item.HP -= hp;
            print(index + " 被攻击，剩余血量 " + item.HP);
            //播放特效fx
            Instantiate(attFx, item.transform.position + Vector3.up, Quaternion.identity);

            if (item.HP <= 0)
            {
                print("塔 打爆了");
                BattleFieldRequest.Instance.DestoryRequest(item.id, 100, ObjectID);
            }
            ShowBlood(item.gameObject, hp);
        }
        //如果受击者是 小兵
        else if (Mathf.Round(index / 1000) == 6)
        {
            print(index + "：准备扣血");
            //拿到角色，扣血
            //扣血
            var item = GetSoldierByID(index);
            item.HP -= hp;
            print(index + " 被攻击，剩余血量 " + item.HP);
            //播放特效fx
            Instantiate(attFx, item.transform.position + Vector3.up, Quaternion.identity);

            if (item.HP <= 0)
            {
                print("xiaobing 打爆了");
                //  BattleFieldRequest.Instance.DestoryRequest(item.id, 100, ObjectID);
                item.Dead();

            }
            ShowBlood(item.gameObject, hp);
        }
        else
        {
            //如果是炮塔
            //拿到角色，扣血
            //扣血
            var item = GetPlayerByID(index);
            item.model.HP -= hp;
            print(index + " 被攻击，剩余血量 " + item.model.HP);
            //播放特效fx
            Instantiate(attFx, item.transform.position + Vector3.up, Quaternion.identity);

            if (item.model.isMe && item.model.HP <= 0)
            {
                BattleFieldRequest.Instance.EndingRequest(item.model.id);
            }
            ShowBlood(item.gameObject, hp);
        }
    }

    private void ShowBlood(GameObject item,int hp)
    {
        //Build the information
        HUDTextInfo info7 = new HUDTextInfo(item.transform, string.Format("{1}{0}", hp, "-"));
        info7.Color = Color.red;
        info7.Size = UnityEngine.Random.Range(50, 120);
        info7.Speed = UnityEngine.Random.Range(0.2f, 1);
        info7.VerticalAceleration = UnityEngine.Random.Range(-2, 2f);
        info7.VerticalPositionOffset = 2f;
        info7.VerticalFactorScale = UnityEngine.Random.Range(1.2f, 10);
        info7.Side = (UnityEngine.Random.Range(0, 2) == 1) ? bl_Guidance.LeftDown : bl_Guidance.RightDown;
        info7.ExtraDelayTime = -1;
        info7.AnimationType = bl_HUDText.TextAnimationType.PingPong;
        info7.FadeSpeed = 200;
        info7.ExtraFloatSpeed = -11;
        //Send the information
        bl_UHTUtils.GetHUDText.NewText(info7);
    }

    public void OnAttBtn()
    {
        //调用P'layer'Move.OnAttBtn()
        Player1.GetComponent<PlayerAttack>().OnAttackBtn();
    }
    public void OnSkill1()
    {
        Player1.GetComponent<PlayerAttack>().OnSkill1();
    }
}
