
using System;
using System.Collections;
using System.Collections.Generic;

public class ScreenBindCameraHandle : BaseHandle
{
    #region 字段

    #endregion

    #region 构造函数
    public ScreenBindCameraHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "ScreenBindCameraHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        ScreenBindCamera response = new ScreenBindCamera(packet.m_data);
        if (response == null) return;
        // UInt16 stationIndex = response.m_stationIndex;
        // UInt16 cameraIndex = response.m_cameraIndex;
        // UInt16 bigScreenIndex = response.m_bigScreenIndex;
        // UInt16 smallScreenIndex = response.m_smallScreenIndex;
        // string cameraName = response.m_cameraName;
        UnityEngine.Debug.Log("接收到ScreenBindCamera消息.");
        SendNotification(EventID_Cmd.ScreenBindCamera, response, null);
    }
    #endregion
}
