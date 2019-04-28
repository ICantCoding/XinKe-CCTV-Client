using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCtrlHandle : BaseHandle
{
    #region 构造函数
    public NpcCtrlHandle(BaseNetworkEngine networkEngine) :
        base(networkEngine)
    {
        Name = "NpcCtrlHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
    }
    #endregion
}
