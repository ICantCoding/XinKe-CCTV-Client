using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (m_stationMgr == null)
        {
            GameObject go = GameObject.Find("StationMgr");
            if (go != null)
            {
                m_stationMgr = go.GetComponent<StationMgr>();
            }
        }
        if (m_stationMgr == null) return;
        NpcAction npcAction = m_stationMgr.GetNpcAction(0, NpcActionStatus.EnterStationTrainUp_NpcActionStatus, npcId);
        if (npcAction == null)
        {
            GameObject npcGo = GameObject.Instantiate(m_stationMgr.npcPrefab);
            npcAction = npcGo.GetComponent<NpcAction>();
            npcAction.NpcId = npcId;
            m_stationMgr.AddNpcAction(0, NpcActionStatus.EnterStationTrainUp_NpcActionStatus, npcAction);
        }
        if (npcAction != null)
        {
            npcAction.transform.localPosition = new Vector3(response.m_posX, response.m_posY, response.m_posZ);
        }
    }
    #endregion
}
