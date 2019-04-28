using System;
using System.Collections;
using System.Collections.Generic;

public class DivisionBigScreenHandle : BaseHandle
{
    #region 字段

    #endregion

    #region 构造函数
    public DivisionBigScreenHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "DivisionBigScreenHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        DivisionBigScreen response = new DivisionBigScreen(packet.m_data);
        if (response == null) return;
        // UInt16 x = response.m_bigScreenXDivisionCount;
        // UInt16 y = response.m_bigScreenYDivisionCount;
        //客户端实现分屏操作，通过PureMVC发送消息到UI
        UnityEngine.Debug.Log("接收到DivisionBigScreen消息.");
        SendNotification(EventID_Cmd.DivisionBigScreen, response, null);
    }
    #endregion
}
