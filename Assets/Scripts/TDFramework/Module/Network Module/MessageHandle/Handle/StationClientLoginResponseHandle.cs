using System;
using System.Collections;
using System.Collections.Generic;

public class StationClientLoginResponseHandle : BaseHandle
{

    #region 构造函数
    public StationClientLoginResponseHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "StationClientLoginResponseHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        StationClientLoginResponse response = new StationClientLoginResponse(packet.m_data);
        if (response == null) return;
        if (response.m_resultId == ResultID.Success_ResultId)
        {
            //StationSocket连接服务器成功
            SendNotification(EventID_Cmd.StationClientOnLineSuccess);
        }
        else
        {
            //StationSocket连接服务器失败
            SendNotification(EventID_Cmd.StationClientOnLineFail);
        }
    }
    #endregion

    #region 私有方法
    private void SendNotification(string eventName)
    {
        object[] objs = new object[2];
        objs[0] = ((StationNetworkEngine)m_networkEngine).StationIndex;
        objs[1] = ((StationNetworkEngine)m_networkEngine).StationClientType;
        SendNotification(eventName, objs, null);
    }
    #endregion
}
