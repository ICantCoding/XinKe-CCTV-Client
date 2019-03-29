using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;
using UnityEngine;

public class NpcDestroyHandle : BaseHandle
{
    #region 字段
    private StationMgr m_stationMgr;
    #endregion

    #region 构造函数
    public NpcDestroyHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "NpcDestroyHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        NpcDestroy response = new NpcDestroy(packet.m_data);
        if (response == null) return;
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
        GameObject.Destroy(npcAction.gameObject);
    }
    #endregion

}
