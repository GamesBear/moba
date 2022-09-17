using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private GameObject target;
    private Vector3 offset = new Vector3(0.147f, 23.252f, -7.567f);

    void Start () {
        
	}
	public void Init(GameObject target)
    {
        this.target = target;
        this.transform.rotation = Quaternion.Euler(72.23f, 0.7920001f, 0f);
    }
	void Update () {
        if (target  != null)
        {
            this.transform.position = target.transform.position + offset;
        }
    }
}
