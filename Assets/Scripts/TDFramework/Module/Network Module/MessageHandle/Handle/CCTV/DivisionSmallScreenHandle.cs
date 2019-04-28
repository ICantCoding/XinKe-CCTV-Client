
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionSmallScreenHandle : BaseHandle
{
    #region 字段

    #endregion

    #region 构造函数
    public DivisionSmallScreenHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "DivisionSmallScreenHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        DivisionSmallScreen response = new DivisionSmallScreen(packet.m_data);
        if (response == null) return;
        // UInt16 bigScreenIndex = response.m_bigScreenIndex;
        // UInt16 x = response.m_smallScreenXDivisionCount;
        // UInt16 y = response.m_smallScreenYDivisionCount;
        //客户端实现指定大屏切割为小屏操作，通过PureMVC发送消息到UI
        UnityEngine.Debug.Log("接收到DivisionSmallScreen消息.");
        SendNotification(EventID_Cmd.DivisionSmallScreen, response, null);
    }
    #endregion
}
