
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TDFramework;
using PureMVC.Patterns.Observer;

public class PlayerNetworkEngine : BaseNetworkEngine
{
    #region Send网络消息方法
    public override void SendU3DClientLoginInfoRequest()
    {
        U3DClientLogin u3dClientLogin = new U3DClientLogin()
        {
            m_clientId = SingletonMgr.GameGlobalInfo.PlayerInfo.Id,
            m_clientName = SingletonMgr.GameGlobalInfo.PlayerInfo.Name,
        };
        Packet packet = new Packet(u3dClientLogin.m_clientId, 0, 0, 0, u3dClientLogin.Size, u3dClientLogin.Packet2Bytes());
        if (m_remoteClient != null)
        {
            m_remoteClient.Send(packet.Packet2Bytes());
        }
    }
    #endregion

    #region Receive网络消息处理方法
    public override void ReceiveU3DClientLoginInfoResponse(Packet packet)
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
