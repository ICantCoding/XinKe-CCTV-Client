
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class NpcPositionHandle : BaseHandle
{
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
        int npcType = response.m_npcType;
        UInt16 stationIndex = response.m_stationIndex;
        UInt16 stationClientType = response.m_stationClientType;

        //因为Socket连接在AppStart页面就已经创建了连接，那个时候就会有消息发送过来，但是当前MainStation场景都还没有准备好，所以这里需要一个return, 忽略这些消息
        StationMgr stationMgr = GameGlobalComponent.StationMgr;
        if (stationMgr == null) return;
        NpcAction npcAction = stationMgr.GetNpcAction(stationIndex, (NpcActionStatus)stationClientType, npcId);
        if (npcAction == null)
        {
            Transform parent = GameObject.Find("Npc").transform; //获取生成Npc需要放入到的容器
            GameObject npcGo = SingletonMgr.ObjectManager.Instantiate(StringMgr.NpcModelNameList[npcType]);
            npcGo.transform.SetParent(parent);
            npcAction = npcGo.GetComponent<NpcAction>();
            npcAction.NpcId = npcId;
            npcAction.StationIndex = stationIndex;
            npcAction.NpcActionStatus = stationClientType;
            stationMgr.AddNpcAction(stationIndex, (NpcActionStatus)stationClientType, npcAction);
            npcAction.transform.localPosition = new Vector3(response.m_posX, response.m_posY, response.m_posZ);
            npcAction.transform.localEulerAngles = new Vector3(response.m_angleX, response.m_angleY, response.m_angleZ);
        }
        npcAction.DestionationPos = new Vector3(response.m_posX, response.m_posY, response.m_posZ);
        npcAction.DestionationAngle = new Vector3(response.m_angleX, response.m_angleY, response.m_angleZ);
    }
    #endregion
}
