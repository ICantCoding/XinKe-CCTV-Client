using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceActionHandle : BaseHandle
{
    #region 字段
    private StationMgr m_stationMgr;
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

        if (m_stationMgr == null)
        {
            GameObject go = GameObject.Find(StringMgr.StationMgrName);
            if (go != null)
            {
                m_stationMgr = go.GetComponent<StationMgr>();
            }
        }
        if (m_stationMgr == null) return;
        Device device = m_stationMgr.GetDevice(stationIndex, (DeviceType)deviceType, deviceId);
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
