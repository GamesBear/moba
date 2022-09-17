using Common;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Request: MonoBehaviour
{
    public OperationCode OpCode;
    public abstract void DefaultRequest();
    //抽象方法就是只声明，具体的实现有子类完成

    public void Awake()
    {
        PhotonEngine.Instance.AddRequest(this);
    }

    public void OnDestroy()
    {
        PhotonEngine.Instance.RemoveRequest(this);
    }

    public abstract void OnOprationRespionse(OperationResponse operationResponse);
    public abstract void OnEvent(EventData data);
}
