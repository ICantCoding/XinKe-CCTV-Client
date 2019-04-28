using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Line : MonoBehaviour
{
    #region 组件字段
    private DOTweenPath m_tweenPath;
    #endregion

    #region  Unity生命周期
    void Awake()
    {
        m_tweenPath = GetComponent<DOTweenPath>();
    }
    void Start()
    {
        List<Vector3> list = ReadTxt.XXX();
        Debug.Log("list count: " + list.Count.ToString());
        for(int i = 0; i < list.Count; i++)
        {
            GameObject go = new GameObject(i.ToString());
            go.transform.SetParent(transform);
            go.transform.localPosition = list[i];
        }
    }
    #endregion
}
