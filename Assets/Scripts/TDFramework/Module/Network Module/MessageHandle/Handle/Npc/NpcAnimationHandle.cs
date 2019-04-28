using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimationHandle : BaseHandle
{
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

        //因为Socket连接在AppStart页面就已经创建了连接，那个时候就会有消息发送过来，但是当前MainStation场景都还没有准备好，所以这里需要一个return, 忽略这些消息
        StationMgr stationMgr = GameGlobalComponent.StationMgr;
        if (stationMgr == null) return;
        NpcAction npcAction = stationMgr.GetNpcAction(stationIndex, (NpcActionStatus)stationClientType, npcId);
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
