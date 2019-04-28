using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TrainDoorType
{
    Left,
    Right,
    None,
}

public class TrainDoor : MonoBehaviour
{
    #region 状态字段
    public TrainDoorType m_trainDoorType = TrainDoorType.None;
    #endregion

    #region 字段
    private Transform m_leftDoorTrans;
    private Transform m_rightDoorTrans;
    //原来的位置
    private Vector3 m_leftStartLocalPosition;
    private Vector3 m_rightStartLocalPosition;
    private Vector3 m_leftMiddleLocalPosition;
    private Vector3 m_rightMiddleLocalPosition;
    private Vector3 m_leftEndLocalPosition;
    private Vector3 m_rightEndLocalPosition;

    #region 列车左侧门动画数据
    //向外偏移的大小
    private Vector3 m_leftSideOutOffset = new Vector3(-0.070883f, 0.0f, 0.0f);
    //向前偏移的大小
    private Vector3 m_leftSideForwardOffset = new Vector3(0.0f, 0.0f, 0.683f);
    //向后偏移的大小
    private Vector3 m_leftSideBackOffset = new Vector3(0.0f, 0.0f, -0.685f);
    #endregion
    #region 列车右侧门动画数据
    //向外偏移的大小
    private Vector3 m_rightSideOutOffset = new Vector3(-0.070883f, 0.0f, 0.0f);
    //向前偏移的大小
    private Vector3 m_rightSideForwardOffset = new Vector3(0.0f, 0.0f, 0.683f);
    //向后偏移的大小
    private Vector3 m_rightSideBackOffset = new Vector3(0.0f, 0.0f, -0.685f);

    #endregion


    #endregion

    #region 属性

    #endregion

    #region Unity生命周期
    void Awake()
    {
        if(m_trainDoorType == TrainDoorType.Left)
        {
            m_leftSideOutOffset = new Vector3(-0.070883f, 0.0f, 0.0f);
            m_leftSideForwardOffset = new Vector3(0.0f, 0.0f, 0.683f);
            m_leftSideBackOffset = new Vector3(0.0f, 0.0f, -0.685f);
            m_rightSideOutOffset = new Vector3(-0.070883f, 0.0f, 0.0f);
            m_rightSideForwardOffset = new Vector3(0.0f, 0.0f, 0.683f);
            m_rightSideBackOffset = new Vector3(0.0f, 0.0f, -0.685f);
        }
        else if(m_trainDoorType == TrainDoorType.Right)
        {
            m_leftSideOutOffset = new Vector3(0.070883f, 0.0f, 0.0f);
            m_leftSideForwardOffset = new Vector3(0.0f, 0.0f, -0.683f);
            m_leftSideBackOffset = new Vector3(0.0f, 0.0f, -0.685f);
            m_rightSideOutOffset = new Vector3(0.070883f, 0.0f, 0.0f);
            m_rightSideForwardOffset = new Vector3(0.0f, 0.0f, 0.683f);
            m_rightSideBackOffset = new Vector3(0.0f, 0.0f, 0.685f);
        }
        m_leftDoorTrans = transform.Find("Left");
        m_leftStartLocalPosition = m_leftDoorTrans.localPosition;
        m_rightDoorTrans = transform.Find("Right");
        m_rightStartLocalPosition = m_rightDoorTrans.localPosition;
        m_leftMiddleLocalPosition = m_leftStartLocalPosition + m_leftSideOutOffset;
        m_rightMiddleLocalPosition = m_rightStartLocalPosition + m_rightSideOutOffset;
        m_leftEndLocalPosition = m_leftMiddleLocalPosition + m_leftSideForwardOffset;
        m_rightEndLocalPosition = m_rightMiddleLocalPosition + m_rightSideBackOffset;
    }
    #endregion

    #region 方法
    public void OpenDoor()
    {
        m_leftDoorTrans.DOLocalMove(m_leftMiddleLocalPosition, 0.1f).OnComplete(() =>
        {
            m_leftDoorTrans.DOLocalMove(m_leftEndLocalPosition, 0.5f);
        });
        m_rightDoorTrans.DOLocalMove(m_rightMiddleLocalPosition, 0.1f).OnComplete(() =>
        {
            m_rightDoorTrans.DOLocalMove(m_rightEndLocalPosition, 0.5f);
        });
    }
    public void CloseDoor()
    {
        m_leftDoorTrans.DOLocalMove(m_leftMiddleLocalPosition, 0.5f).OnComplete(() =>
        {
            m_leftDoorTrans.DOLocalMove(m_leftStartLocalPosition, 0.1f);
        });
        m_rightDoorTrans.DOLocalMove(m_rightMiddleLocalPosition, 0.5f).OnComplete(() =>
        {
            m_rightDoorTrans.DOLocalMove(m_rightStartLocalPosition, 0.1f);
        });
    }
    #endregion
}
