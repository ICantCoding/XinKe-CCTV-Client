using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;
using UnityEngine;

public class NpcDestroyHandle : BaseHandle
{
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

        //因为Socket连接在AppStart页面就已经创建了连接，那个时候就会有消息发送过来，但是当前MainStation场景都还没有准备好，所以这里需要一个return, 忽略这些消息
        StationMgr stationMgr = GameGlobalComponent.StationMgr;
        if (stationMgr == null) return;
        NpcAction npcAction = stationMgr.GetNpcAction(stationIndex, (NpcActionStatus)stationClientType, npcId);
        DestroyNpc4ObjectManager(npcAction.gameObject);
        stationMgr.RemoveNpcAction(npcAction);
    }
    private void DestroyNpc4ObjectManager(GameObject go)
    {
        if(go == null) return;
        if (ObjectManager.Instance.IsCreatedByObjectManager(go))
        {
            ObjectManager.Instance.ReleaseGameObjectItem(go);
        }
        else
        {
            GameObject.Destroy(go);
        }
    }
    #endregion
}
