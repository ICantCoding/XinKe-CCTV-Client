using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadTrainHandle : BaseHandle
{
    #region 构造函数
    public UnloadTrainHandle(BaseNetworkEngine networkEngine) :
        base(networkEngine)
    {
        Name = "UnloadTrainHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
        //转发给所有的客户端
    }
    #endregion
}
