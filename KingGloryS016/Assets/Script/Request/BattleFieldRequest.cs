using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Common;
using System;

public class BattleFieldRequest : Request {

    public static BattleFieldRequest Instance;
    private int curX;
    private int curY;

    private void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public override void DefaultRequest()
    {
       
    }

    public override void OnOprationRespionse(OperationResponse operationResponse)
    {
        //解析数据
        if (operationResponse.ReturnCode == (byte)ReturnCode.Success)
        {
            int playerIndex = (int)DicTool.GetValue<byte, object>(operationResponse.Parameters, (byte)ParaCode.BF_Join);
            Debug.Log("playerIndex:" + playerIndex);
            BattleFieldManager.Instance.InitBattleField(playerIndex);
        }
        else
        {
            Debug.Log("请求失败");
        }
    }

    internal void MoveRequest(int posX, int posY, Vector3 pos)
    {
        //在更新操作的时候，才发送请求
        if (curX == posX && curY == posY) return;
        curX = posX;
        curY = posY;

        //构造参数
        var data = new Dictionary<byte, object>();

        data.Add((byte)ParaCode.ParaType, ParaCode.BF_Move);
        data.Add((byte)ParaCode.BF_Move, BattleFieldManager.Instance.MyPlayerIndex.ToString() + "," + posX + "," + posY + "," + pos.x + "," + pos.y + "," + pos.z);
        //发送
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    internal void AttackRequest(int player1, int target, Common.AttackType type)
    {
        var p = player1 + "," + target+"," + (int)type;
        //构造参数
        var data = new Dictionary<byte, object>();
        //构造参数
        data.Add((byte)ParaCode.ParaType, ParaCode.BF_Att);
        data.Add((byte)ParaCode.BF_Att, p);

        //发送
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }
    internal void HurtRequest(int player1, int hp, int ObjectID)
    {
        var p = player1 + "," + hp + "," + ObjectID;
        //构造参数
        var data = new Dictionary<byte, object>();
        //构造参数
        data.Add((byte)ParaCode.ParaType, ParaCode.BF_Hurt);
        data.Add((byte)ParaCode.BF_Hurt, p);

        //发送
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void JoinRequest()
    {
        var data = new Dictionary<byte, object>();
        //key 是 本次请求类型 ，value 是 加入战场
        data.Add((byte)ParaCode.ParaType, ParaCode.BF_Join);

        PhotonEngine.peer.OpCustom((byte)OpCode,data,true);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerIndex">死亡者的参数</param>
    internal void EndingRequest(int playerIndex)
    {
        //构造参数
        var data = new Dictionary<byte, object>();
        //构造参数
        data.Add((byte)ParaCode.ParaType, ParaCode.BF_Ending);
        data.Add((byte)ParaCode.BF_Ending, playerIndex);

        //发送
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    internal void DestoryRequest(int towerIndex, int exp, int target)
    {
        var p = towerIndex + "," + exp + "," + target;
        //构造参数
        var data = new Dictionary<byte, object>();
        //构造参数
        data.Add((byte)ParaCode.ParaType, ParaCode.BF_Destory);
        data.Add((byte)ParaCode.BF_Destory, p);

        //发送
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }
    public override void OnEvent(EventData data)
    {
        //解析数据

        ParaCode type = (ParaCode)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.ParaType);
       // Debug.Log("收到服务器:" + type);
        if (type == ParaCode.BF_Join)
        {
            string allPlayer = (string)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.BF_Join);
           // Debug.Log("收到服务器:" + allPlayer);
            BattleFieldManager.Instance.AddPlayer(allPlayer);
        }
        else if (type == ParaCode.BF_Move)
        {
            string para = (string)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.BF_Move);
            //   Debug.Log("收到服务器BF_Move:" + para);

            var list = para.Split(',');
            BattleFieldManager.Instance.MovePlayer(Convert.ToInt16(list[0]), Convert.ToInt16(list[1]), Convert.ToInt16(list[2]), float.Parse(list[3]), float.Parse(list[4]), float.Parse(list[5]));

        }
        else if (type == ParaCode.BF_Att)
        {
            string para = (string)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.BF_Att);
            var list = para.Split(',');

            Debug.Log("收到服务器BF_Att:" + para);

            BattleFieldManager.Instance.PlayAtt(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]));
        }
        else if (type == ParaCode.BF_Hurt)
        {
            string para = (string)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.BF_Hurt);
            var list = para.Split(',');

            Debug.Log("收到服务器BF_Hurt:" + para);

            BattleFieldManager.Instance.Hurt(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]));
        }
        else if (type == ParaCode.BF_Ending)
        {
            int index = (int)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.BF_Ending);

            Debug.Log("收到服务器BF_Ending:" + index);

            EndingPanelController.instance.Ending(index);
        }
        else if (type == ParaCode.BF_Destory)
        {
            string index = (string)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.BF_Destory);
            var list = index.Split(',');
            Debug.Log("收到服务器BF_Destory:" + index);

            BattleFieldManager.Instance.PlayDestory(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]));
        }

    }

}
