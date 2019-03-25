using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;
using UnityEngine;

public class StationNetworkEngine : BaseNetworkEngine
{
    #region 字段
    //Station客户端类型
    public UInt16 m_stationClientType = 0;
    //站台Index
    public UInt16 m_stationIndex = 0;
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
    public void SendStationClientLoginInfoRequest()
    {
        StationClientLogin stationClientLogin = new StationClientLogin()
        {
            m_u3dId = SingletonMgr.GameGlobalInfo.PlayerInfo.Id,
            m_stationClientType = StationClientType,
            m_stationIndex = StationIndex,
        };
        Packet packet = new Packet(0, 0, 0, 1, stationClientLogin.Size, stationClientLogin.Packet2Bytes());
        if (m_remoteClient != null)
        {
            m_remoteClient.Send(packet.Packet2Bytes());
        }
    }
    #endregion

    #region 网络连接，回调方法
    private void RemoteClientConnectServerSuccess_Callback()
    {
        System.Threading.Thread.Sleep(20);
        SendStationClientLoginInfoRequest();
    }
    private void RemoteClientConnectServerFail_Callback()
    {
        object[] objs = new object[2];
        objs[0] = StationIndex;
        objs[1] = StationClientType;
        SendNotification(EventID_Cmd.StationClientOnLineFail, objs, null);
    }
    #endregion
}
