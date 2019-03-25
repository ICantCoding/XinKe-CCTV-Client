using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class NpcPositionHandle : BaseHandle
{
    #region 字段
    private StationMgr m_stationMgr;
    #endregion

    #region 构造函数
    public NpcPositionHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "NpcPositionHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        NpcPosition response = new NpcPosition(packet.m_data);
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
        if (npcAction == null)
        {
            GameObject npcGo = SingletonMgr.ObjectManager.Instantiate("Assets/Prefabs/Npc/NpcMan.prefab", false, false);
            npcAction = npcGo.GetComponent<NpcAction>();
            npcAction.NpcId = npcId;
            m_stationMgr.AddNpcAction(stationIndex, (NpcActionStatus)stationClientType, npcAction);
            npcAction.transform.localPosition = new Vector3(response.m_posX, response.m_posY, response.m_posZ);
            npcAction.transform.localEulerAngles = new Vector3(response.m_angleX, response.m_angleY, response.m_angleZ);
        }
        else
        {
            npcAction.DestionationPos = new Vector3(response.m_posX, response.m_posY, response.m_posZ);
            npcAction.DestionationAngle = new Vector3(response.m_angleX, response.m_angleY, response.m_angleZ);
        }
    }
    #endregion
}
