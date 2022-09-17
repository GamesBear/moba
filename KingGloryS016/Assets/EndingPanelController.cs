using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingPanelController : MonoBehaviour
{
    public static EndingPanelController instance;

    private void Awake()
    {
        instance = this;
        //刚开始应该隐藏
        this.gameObject.SetActive(false);
    }

    public void Ending(int id)
    {
        //结算页面显示出来
        this.gameObject.SetActive(true);
        var  zi = id == BattleFieldManager.Instance.MyPlayerIndex ? "失败" : "胜利";

        transform.Find("Text").GetComponent<Text>().text = zi;

        foreach (var item in BattleFieldManager.Instance.playerList)
        {
            item.GetComponent<PlayerMove>().enabled = false;
        }

    }
    /// <summary>
    /// 当退出按钮按下
    /// </summary>
    public void Quit()
    {
        transform.parent.Find("MatchingPanel").gameObject.SetActive(true);
        this.gameObject.SetActive(false);

        //销毁角色
        foreach (var item in BattleFieldManager.Instance.playerList)
        {
            Destroy(item.gameObject);
            BattleFieldManager.Instance.playerList = new List<BodyModel>();
        }
    }


}
