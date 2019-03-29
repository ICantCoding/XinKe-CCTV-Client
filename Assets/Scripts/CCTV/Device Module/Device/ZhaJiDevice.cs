/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-06 16:10:50

** 功能描述:  闸机脚本组件

** version:   v1.2.0

*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ZhaJiDevice : Device
{
    #region 组件字段
    private Transform m_left;
    private Transform m_right;
    #endregion

    #region 状态字段
    //打开闸机左扇叶旋转角度偏移
    public Vector3 m_leftOpenLocalEulerAngleOffset;
    //打开闸机右扇叶旋转角度偏移
    public Vector3 m_rightOpenLocalEulerAngleOffset;
    private Vector3 m_leftOriginLocalEulerAngle;
    private Vector3 m_rightOriginLocalEulerAngle;
    private Vector3 m_leftOpenLocalEulerAngle;
    private Vector3 m_rightOpenLocalEulerAngle;
    private float m_animationTime = 1.0f;

    [SerializeField]
    protected bool m_canOpen = false; //默认不可以被打开
    #endregion

    #region 属性
    public override DeviceType DeviceType
    {
        get { return DeviceType.ZhaJi; }
    }
    public bool CanOpen
    {
        get { return m_canOpen; }
        set { m_canOpen = value; }
    }
    #endregion

    #region Unity生命周期
    void Awake()
    {
        m_left = transform.Find("Left");
        m_right = transform.Find("Right");
        m_leftOriginLocalEulerAngle = m_left.localEulerAngles;
        m_rightOriginLocalEulerAngle = m_right.localEulerAngles;
        m_leftOpenLocalEulerAngle = m_leftOriginLocalEulerAngle + m_leftOpenLocalEulerAngleOffset;
        m_rightOpenLocalEulerAngle = m_rightOriginLocalEulerAngle + m_rightOpenLocalEulerAngleOffset;
    }
    #endregion

    #region 方法
    public override void Open(System.Action openCallback)
    {
        CanOpen = false;
        m_left.DOLocalRotate(m_leftOpenLocalEulerAngle, m_animationTime);
        m_right.DOLocalRotate(m_rightOpenLocalEulerAngle, m_animationTime).OnComplete(() =>
        {
            if (openCallback != null)
            {
                openCallback();
            }
        });
    }
    public override void Close(System.Action closeCallback)
    {
        m_left.DOLocalRotate(m_leftOriginLocalEulerAngle, m_animationTime);
        m_right.DOLocalRotate(m_rightOriginLocalEulerAngle, m_animationTime).OnComplete(() =>
        {
            CanOpen = true;
            if (closeCallback != null)
            {
                closeCallback();
            }
        });
    }
    #endregion
}
