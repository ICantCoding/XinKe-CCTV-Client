/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-15 14:58:04

** 功能描述:  屏蔽门组件

** version:   v1.2.0

*********************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PingBiMenDevice : Device
{

    #region Delegate
    public System.Action OpenDoorCallback;
    public System.Action CloseDoorCallback;
    #endregion

    #region 组件字段
    private Transform m_left;
    private Transform m_right;
    #endregion

    #region 状态字段
    //屏蔽门左侧门打开偏移量
    public Vector3 m_leftOpenLocalPositionOffset;
    //屏蔽门右侧门打开偏移量
    public Vector3 m_rightOpenLocalPositionOffset;
    private Vector3 m_leftOriginLocalPosition;
    private Vector3 m_rightOriginLocalPosition;
    private Vector3 m_leftOpenLocalPosition;
    private Vector3 m_rightOpenLocalPosition;
    private float m_animationTime = 1.0f;

    [SerializeField]
    private bool m_canUp = false; //是否可以上车, 默认不可以上车
    [SerializeField]
    private bool m_canDown = false; //是否可以下车，默认不可以下车
    #endregion

    #region 属性
    public override DeviceType DeviceType
    {
        get { return DeviceType.PingBiMen; }
    }
    public bool CanUp
    {
        get { return m_canUp; }
        set { m_canUp = value; }
    }
    public bool CanDown
    {
        get { return m_canDown; }
        set { m_canDown = value; }
    }
    #endregion

    #region Unity生命周期
    void Awake()
    {
        m_left = transform.Find("Left");
        m_right = transform.Find("Right");
        m_leftOriginLocalPosition = m_left.localPosition;
        m_rightOriginLocalPosition = m_right.localPosition;
        m_leftOpenLocalPosition = m_leftOriginLocalPosition + m_leftOpenLocalPositionOffset;
        m_rightOpenLocalPosition = m_rightOriginLocalPosition + m_rightOpenLocalPositionOffset;
    }
    #endregion

    #region 方法
    public override void Open(System.Action openCallback)
    {
        m_left.DOLocalMove(m_leftOpenLocalPosition, m_animationTime);
        m_right.DOLocalMove(m_rightOpenLocalPosition, m_animationTime).OnComplete(() =>
        {
            CanUp = true;
            CanDown = true;
            if (openCallback != null)
            {
                openCallback();
            }
            if (OpenDoorCallback != null)
            {
                OpenDoorCallback();
            }
        });
    }
    public override void Close(System.Action closeCallback)
    {
        CanUp = false;
        CanDown = false;
        m_left.DOLocalMove(m_leftOriginLocalPosition, m_animationTime);
        m_right.DOLocalMove(m_rightOriginLocalPosition, m_animationTime).OnComplete(() =>
        {
            if (closeCallback != null)
            {
                closeCallback();
            }
            if (CloseDoorCallback != null)
            {
                CloseDoorCallback();
            }
        });
    }
    #endregion
}
