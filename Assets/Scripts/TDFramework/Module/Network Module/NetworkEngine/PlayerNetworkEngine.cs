
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
    //发送U3D客户端登录消息
    public void SendU3DClientLoginInfoRequest()
    {
        U3DClientLogin u3dClientLogin = new U3DClientLogin()
        {
            m_clientId = SingletonMgr.GameGlobalInfo.PlayerInfo.Id,
            m_clientName = SingletonMgr.GameGlobalInfo.PlayerInfo.Name,
        };
        Packet packet = new Packet(u3dClientLogin.m_clientId, SingletonMgr.GameGlobalInfo.PlayerInfo.CctvServerId, 
            SingletonMgr.MessageIDMgr.U3DClientLoginMessageID, u3dClientLogin.Size, u3dClientLogin.Packet2Bytes());
        if (m_remoteClient != null)
        {
            m_remoteClient.Send(packet.Packet2Bytes());
        }
    }
    //发送客户端重连消息
    public void SendU3DClientReconnectRequest()
    {
        ClientReConnect clientReConnect = new ClientReConnect();
        Packet packet = new Packet(SingletonMgr.GameGlobalInfo.PlayerInfo.Id, SingletonMgr.GameGlobalInfo.PlayerInfo.CctvServerId, 
            SingletonMgr.MessageIDMgr.ClientRelinkMessageId, clientReConnect.Size, clientReConnect.Packet2Bytes());
        if(m_remoteClient != null)
        {
            m_remoteClient.Send(packet.Packet2Bytes());
        }
    }
    #endregion
}
