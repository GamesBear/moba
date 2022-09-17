using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapObject : MonoBehaviour {

    void Start()
    {
        MiniMap.Instance.AddIcon(this);

    }

    private void OnDestroy()
    {
        MiniMap.Instance.removeIcon(this);
    }
}
