using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BodyModel {

    [HideInInspector]
    public bool isMe = false;
    internal void ExpUp(int exp)
    {
        print("加经验");
        transform.Find("LvFx").GetComponent<ParticleSystem>().Play();
    }
    public override void SendHurtRequest(int hurtValue, int ObjectID)
    {
        BattleFieldRequest.Instance.HurtRequest(id, hurtValue, ObjectID);
    }
}
