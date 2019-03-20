using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationNetworkEngine : BaseNetworkEngine
{
    #region 字段
    //客户端Id
    public UInt16 m_stationSocketType = 0;
    //站台Index
    public UInt16 m_stationIndex = 0;
    #endregion

    #region 属性
    public UInt16 StationSocketType
    {
        get { return m_stationSocketType; }
        set { m_stationSocketType = value; }
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

    #region 重写方法
    public override void SendStationClientLoginInfoRequest()
    {
        StationClientLogin stationClientLogin = new StationClientLogin()
        {
            m_stationSocketType = StationSocketType,
            m_stationIndex = StationIndex,
        };
        Packet packet = new Packet(0, 0, 0, 1, stationClientLogin.Size, stationClientLogin.Packet2Bytes());
        if (m_remoteClient != null)
        {
            m_remoteClient.Send(packet.Packet2Bytes());
        }
    }
    public override void ReceiveStationClientLoginInfoResponse(Packet packet)
    {
        StationClientLoginResponse response = new StationClientLoginResponse(packet.m_data);
        if (response == null) return;
        if (response.m_resultId == ResultID.Success_ResultId)
        {
            //StationSocket连接服务器成功
            object[] objs = new object[2];
            objs[0] = StationIndex;
            objs[1] = StationSocketType;
            SendNotification(EventID_Cmd.StationClientOnLineSuccess, objs, null);
        }
        else
        {
            //StationSocket连接服务器失败
            object[] objs = new object[2];
            objs[0] = StationIndex;
            objs[1] = StationSocketType;
            SendNotification(EventID_Cmd.StationClientOnLineFail, objs, null);
        }
    }
    #endregion

    #region 网络连接，回调方法
    private void RemoteClientConnectServerSuccess_Callback()
    {
        System.Threading.Thread.Sleep(10);
        SendStationClientLoginInfoRequest();
    }
    private void RemoteClientConnectServerFail_Callback()
    {
        
    }
    #endregion
}
