using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCtrlCompleteHandle : BaseHandle
{
    #region 构造函数
    public NpcCtrlCompleteHandle(BaseNetworkEngine networkEngine) :
        base(networkEngine)
    {
        Name = "NpcCtrlCompleteHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
    }
    #endregion
}
