using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceActionHandle : BaseHandle
{
    #region 字段

    #endregion

    #region 构造函数
    public DeviceActionHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "DeviceActionHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        DeviceAction response = new DeviceAction(packet.m_data);
        if (response == null) return;
        int deviceId = response.m_deviceId;
        int deviceType = response.m_deviceType;
        UInt16 stationIndex = response.m_stationIndex;
        byte deviceStatus = response.m_deviceStatus;

        //因为Socket连接在AppStart页面就已经创建了连接，那个时候就会有消息发送过来，但是当前MainStation场景都还没有准备好，所以这里需要一个return, 忽略这些消息
        StationMgr stationMgr = GameGlobalComponent.StationMgr;
        if (stationMgr == null) return;
        Device device = stationMgr.GetDevice(stationIndex, (DeviceType)deviceType, deviceId);
        if(device != null)
        {
            if(deviceStatus == 0)
            {
                device.Close(null);
            }
            else if(deviceStatus == 1)
            {
                device.Open(null);
            }
        }
    }
    #endregion
}
