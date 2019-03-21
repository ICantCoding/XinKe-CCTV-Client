
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
    public void SendU3DClientLoginInfoRequest()
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
}
