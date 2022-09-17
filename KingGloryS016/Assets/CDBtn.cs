using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDBtn : MonoBehaviour {
    private Image cdImg;
    private ETCButton btn;
    public float cdtime;
    void Start () {
        cdImg = transform.Find("cdImg").GetComponent<Image>();
        btn = GetComponent<ETCButton>();

    }

    void Update () {
		
	}

    public void OnBtn()
    {
        cdImg.fillAmount = 1;
        StartCoroutine(StartCD());
    }
    private IEnumerator StartCD()
    {
        print("开始携程");
        //计算cd后的事件
        var t = Time.time + cdtime;
        //按钮禁用掉
        btn.activated = false;
        while (true)
        {
            cdImg.fillAmount = (t - Time.time) / cdtime;//范围是1-0
            if (t <= Time.time)
            {
                btn.activated = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
