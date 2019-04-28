using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;
using UnityEngine;

public class StationNetworkEngine : BaseNetworkEngine
{
    #region 字段
    public UInt16 m_stationIndex = 0;           //站台Index
    public UInt16 m_stationClientType = 0;      //Station客户端类型
    #endregion


    #region 属性
    public UInt16 StationClientType
    {
        get { return m_stationClientType; }
        set { m_stationClientType = value; }
    }
    public UInt16 StationIndex
    {
        get { return m_stationIndex; }
        set { m_stationIndex = value; }
    }
    #endregion



    #region Unity生命周期
    void Start()
    {
        Run(RemoteClientConnectServerSuccess_Callback, RemoteClientConnectServerFail_Callback);
    }
    #endregion



    #region 方法
    //连接
    public void Link()
    {
        Run(RemoteClientConnectServerSuccess_Callback, RemoteClientConnectServerFail_Callback);
    }
    //重新连接
    public void ReLink()
    {
        Link();
    }
    #endregion



    #region 网络连接，回调方法
    private void RemoteClientConnectServerSuccess_Callback()
    {
        //Socket连接成功后, 等待20毫秒后向服务器发送登录信息
        System.Threading.Thread.Sleep(20);
        SendStationClientLoginInfoRequest();
    }
    private void RemoteClientConnectServerFail_Callback()
    {
        //连接服务器失败后，停止删除之前连接的资源
        Stop(); 
        UInt16[] objs = new UInt16[2];
        objs[0] = StationIndex;
        objs[1] = StationClientType;
        SendNotification(EventID_Cmd.StationClientOnLineFail, objs, null);
    }
    #endregion




    #region 发送网络数据
    //发送站台Socket登录消息
    public void SendStationClientLoginInfoRequest()
    {
        StationClientLogin stationClientLogin = new StationClientLogin()
        {
            m_u3dId = SingletonMgr.GameGlobalInfo.PlayerInfo.Id,
            m_stationClientType = StationClientType,
            m_stationIndex = StationIndex,
        };
        Packet packet = new Packet(stationClientLogin.m_u3dId, SingletonMgr.GameGlobalInfo.PlayerInfo.CctvServerId,
            SingletonMgr.MessageIDMgr.StationClientLoginMessageID, stationClientLogin.Size, stationClientLogin.Packet2Bytes());
        if (m_remoteClient != null)
        {
            m_remoteClient.Send(packet.Packet2Bytes());
        }
    }
    #endregion
}
