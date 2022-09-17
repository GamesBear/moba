using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {
    public static MiniMap Instance;
    public GameObject Icon;
    private Dictionary<MiniMapObject, RectTransform> IconDic = new Dictionary<MiniMapObject, RectTransform>();

    void Awake()
    {
        Instance = this;
    }
    void Start () {
        StartCoroutine(UpdateIcon());
    }
    private IEnumerator UpdateIcon()
    {
        while (true)
        {
            foreach (var item in IconDic)
            {
                print("跟新坐标:" + item.Value.name + ":" + item.Value.localPosition);
                item.Value.localPosition = item.Key.transform.position*4-Vector3.back * 4;
            }
            //更新图标位置
            yield return new WaitForSeconds(0.3f);
        }
    }
    //添加icon
    public void AddIcon(MiniMapObject body)
    {
        IconDic.Add(  body   , Instantiate(Icon, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>()    );
        //设置为minimap的子对象
        IconDic[body].parent = this.transform;
        IconDic[body].localPosition = body.transform.position * 4-Vector3.back * 4;
        print("生成一个对象icon:" + body.name + ":" + IconDic[body].localPosition);
    }

    public void removeIcon(MiniMapObject body)
    {
        Destroy(IconDic[body].gameObject);
        IconDic.Remove(body);
    }
}
