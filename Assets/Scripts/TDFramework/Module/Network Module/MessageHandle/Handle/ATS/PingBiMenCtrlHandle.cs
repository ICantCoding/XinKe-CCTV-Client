using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingBiMenCtrlHandle : BaseHandle
{
    #region 构造函数
    public PingBiMenCtrlHandle(BaseNetworkEngine networkEngine) :
        base(networkEngine)
    {
        Name = "PingBiMenCtrlHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
        PingBiMenCtrl response = new PingBiMenCtrl(packet.m_data);
        UInt16 stationIndex = response.m_stationIndex;
        byte upOrDownFlag = response.m_upOrDownFlag;
        byte statusFlag = response.m_statusFlag;

        //屏蔽门执行动画===============================================
        StationMgr stationMgr = GameGlobalComponent.StationMgr;
        if (stationMgr == null) return;
        List<Device> devices = stationMgr.GetPingBiMenList(stationIndex, (PingBiMenType)upOrDownFlag);
        foreach(var device in devices)
        {
            if(statusFlag == 0)
            {
                //关
                device.Close(null);
            }
            else if(statusFlag == 1)
            {
                //开
                device.Open(null);
            }
        }
    }
    #endregion
}
