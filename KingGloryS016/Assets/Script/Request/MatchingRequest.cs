using System.Collections;
using System.Collections.Generic;
using Common;
using ExitGames.Client.Photon;
using UnityEngine;

public class MatchingRequest : Request
{
    public static MatchingRequest Instance;
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

    public override void OnEvent(EventData data)
    {
        print("match return");
        //解析数据
        ParaCode type = (ParaCode)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.ParaType);
        // Debug.Log("收到服务器:" + type);
        if (type == ParaCode.Matching_confirm)
        {
            int playerindex = (int)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.Matching_confirm);
            // Debug.Log("收到服务器:" + allPlayer);

            //初始化角色
            BattleFieldManager.Instance.InitBattleField(playerindex);
            BattleFieldManager.Instance.AddOnePlayer(playerindex == 1 ? 2 : 1);

            //隐藏界面
            gameObject.SetActive(false);
        }
    }

    public override void OnOprationRespionse(OperationResponse operationResponse)
    {
        //解析数据
        if (operationResponse.ReturnCode == (byte)ReturnCode.Success)
        {

            Debug.Log("MatchingStartRequest");
            //进入等待界面
        }
        else
        {
            Debug.Log("请求失败");
        }
    }

    internal void MatchingStartRequest()
    {
        //构造参数
        var data = new Dictionary<byte, object>();
        //构造参数
        data.Add((byte)ParaCode.ParaType, ParaCode.Matching_Start);

        //发送
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

}
