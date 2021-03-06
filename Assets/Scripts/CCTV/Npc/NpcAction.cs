﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcAnimationType : System.UInt16
{
    StandUp = 1, //站立
    Walk = 2,   //行走
    OpenZhaJi = 3, //打开闸机
}

public enum NpcType
{
    Man1,           //男1
    Man2,           //男2
    Man3,           //男3
    Man4,           //男4
    Man5,           //男5
    Woman1,         //女1
    Woman2,         //女2
    Woman3,         //女3
    Woman4,         //女4
    Woman5,         //女5
    None,
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
    private UInt16 m_stationIndex;
    private UInt16 m_npcActionStatus;
    private Vector3 m_destionationPos;
    private Vector3 m_destionationAngle;
    #endregion

    #region 属性
    public int NpcId
    {
        get { return m_npcId; }
        set { m_npcId = value; }
    }
    public UInt16 StationIndex
    {
        get { return m_stationIndex; }
        set { m_stationIndex = value; }
    }
    public UInt16 NpcActionStatus
    {
        get { return m_npcActionStatus; }
        set { m_npcActionStatus = value; }
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
    }
    void Start()
    {
        //Npc生成初始执行移动动画
        Walk();
    }
    void Update()
    {
        if (Vector3.Distance(m_destionationPos, transform.localPosition) > 0.01)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_destionationPos, 0.12f);
        }
        if (Vector3.Distance(m_destionationAngle, transform.localEulerAngles) > 0.01)
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, m_destionationAngle, 0.1f);
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
