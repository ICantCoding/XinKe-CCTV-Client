using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcAnimationType : System.UInt16
{
    StandUp = 1, //站立
    Walk = 2,   //行走
    OpenZhaJi = 3, //打开闸机
}

public enum NpcActionStatus
{
    EnterStationTrainUp_NpcActionStatus = 1,    //NPC 执行进站上行方向坐车
    EnterStationTrainDown_NpcActionStatus = 2,  //NPC 执行进站下行方向坐车
    ExitStationTrainUp_NpcActionStatus = 3,     //NPC 执行上行方向下车出站
    ExitStationTrainDown_NpcActionStatus = 4,	//NPC 执行下行方向下车出站

    None = 10000,
}


public class NpcAction : MonoBehaviour
{

    #region 常量字段
    private int EnterCheckTicketAnimatorHashValue = Animator.StringToHash("OpenZhaji");
    private int WalkAnimatorHashValue = Animator.StringToHash("Walk");
    private int StandUpAnimatorHashValue = Animator.StringToHash("StandUp");
    #endregion

    #region 字段
    [SerializeField]
    private int m_npcId;
    private Vector3 m_destionationPos;
    private Vector3 m_destionationAngle;
    #endregion

    #region 属性
    public int NpcId
    {
        get { return m_npcId; }
        set { m_npcId = value; }
    }
    public Vector3 DestionationPos
    {
        get { return m_destionationPos; }
        set { m_destionationPos = value; }
    }
    public Vector3 DestionationAngle
    {
        get { return m_destionationAngle; }
        set { m_destionationAngle = value; }
    }
    #endregion

    #region 组件字段
    private Animator m_animator = null;
    #endregion

    #region Unity生命周期
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        Walk();
    }
    void Update()
    {
        if (Vector3.Distance(m_destionationPos, transform.localPosition) > 0.01)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_destionationPos, 0.025f);
        }
        if (Vector3.Distance(m_destionationAngle, transform.localEulerAngles) > 0.01)
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, m_destionationAngle, 0.025f);
        }
    }
    #endregion

    #region Npc动画动作方法
    public void Walk()
    {
        m_animator.SetBool(StandUpAnimatorHashValue, false);
        m_animator.SetBool(WalkAnimatorHashValue, true);
    }
    public void StandUp()
    {
        m_animator.SetBool(WalkAnimatorHashValue, false);
        m_animator.SetBool(StandUpAnimatorHashValue, true);
    }
    public void OpenZhaJi()
    {
        m_animator.SetTrigger(EnterCheckTicketAnimatorHashValue);
    }
    #endregion
}
