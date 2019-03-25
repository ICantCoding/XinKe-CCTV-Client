using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimationHandle : BaseHandle
{
    #region 字段
    private StationMgr m_stationMgr;
    #endregion

    #region 构造函数
    public NpcAnimationHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "NpcAnimationHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        NpcAnimation response = new NpcAnimation(packet.m_data);
        if (response == null) return;
        UInt16 npcAnimationType = response.m_npcAnimationType;
        int npcId = response.m_npcId;
        UInt16 stationIndex = response.m_stationIndex;
        UInt16 stationClientType = response.m_stationClientType;

        if (m_stationMgr == null)
        {
            GameObject go = GameObject.Find(StringMgr.StationMgrName);
            if (go != null)
            {
                m_stationMgr = go.GetComponent<StationMgr>();
            }
        }
        if (m_stationMgr == null) return;
        NpcAction npcAction = m_stationMgr.GetNpcAction(stationIndex, (NpcActionStatus)stationClientType, npcId);
        if (npcAction != null)
        {
            if (npcAnimationType == (UInt16)NpcAnimationType.Walk)
            {
                npcAction.Walk();
            }
            else if (npcAnimationType == (UInt16)NpcAnimationType.StandUp)
            {
                npcAction.StandUp();
            }
            else if (npcAnimationType == (UInt16)NpcAnimationType.OpenZhaJi)
            {
                npcAction.OpenZhaJi();
            }
        }
    }
    #endregion
}
