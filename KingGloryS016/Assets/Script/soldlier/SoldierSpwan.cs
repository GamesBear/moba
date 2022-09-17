using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpwan : MonoBehaviour
{
    //保存协程，用于稍后关闭
    private Coroutine Coroutine;
    public GameObject SodierPF;
    public List<GameObject> FinalTargetList;

    public int group;
    private  int index;

    private void OnEnable()
    {
        BattleFieldManager.Instance.OnInitCallbak += StartSpwan;
    }
    void StartSpwan()
    {
        //开启协程
        Coroutine = StartCoroutine(SpwanSolider());
    }

    private void OnDisable()
    {
        //关闭协程
        StopCoroutine(Coroutine);
    }

    private IEnumerator SpwanSolider()
    {
        yield return new WaitForSeconds(4f);
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
               var s =  Instantiate(SodierPF, this.transform.position, Quaternion.identity);
             //   print(s+":" + s.GetComponent<Soldier>());
                s.GetComponent<Soldier>().Init(FinalTargetList,group,6000+index+group*100);
                index++;
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(30f);
        }
    }

}
