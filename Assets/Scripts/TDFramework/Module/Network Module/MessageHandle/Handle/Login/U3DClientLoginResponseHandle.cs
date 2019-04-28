using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;

public class U3DClientLoginResponseHandle : BaseHandle
{

    #region 构造函数
    public U3DClientLoginResponseHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "U3DClientLoginResponseHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        U3DClientLoginResponse response = new U3DClientLoginResponse(packet.m_data);
        if (response == null) return;
        if (response.m_resultId == ResultID.Success_ResultId)
        {
            //登录成功
            //发送PureMVC消息，通知客户端已经登录到服务器
            SendNotification(EventID_Cmd.U3DClientOnLineSuccess, null, null);
        }
        else
        {
            //登录失败
            string reasonMsg = response.m_msg;
            SendNotification(EventID_Cmd.U3DClientOnLineFail, reasonMsg, null);
        }
    }
    #endregion
}
