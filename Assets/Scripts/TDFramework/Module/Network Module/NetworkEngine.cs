
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TDFramework;
using PureMVC.Patterns.Observer;

public class NetworkEngine : MonoSingleton<NetworkEngine>
{

    #region 组合功能
    private Notifier m_notifier = null;
    #endregion

    #region 字段
    //待处理的数据包Packet队列.
    private Queue<Packet> m_pendingPacketQueue = new Queue<Packet>();
    private RemoteClient m_remoteClient = null;
    #endregion

    #region 状态属性
    //客户端是否已经连接到服务器
    public bool IsConnected
    {
        get
        {
            if(m_remoteClient != null)
            {
                return m_remoteClient.Valid();
            }
            return false;
        }
    }
    #endregion

    #region Unity生命周期
    void Awake()
    {
        //网络引擎不可销毁
        DontDestroyOnLoad(this.gameObject);
        m_notifier = new Notifier();
    }
    #endregion

    #region 方法
    public void Packet2NetworkEnginePendingPacketQueue(Packet packet)
    {
        lock (this)
        {
            m_pendingPacketQueue.Enqueue(packet);
        }
    }
    public void Run(string serverIp, int serverPort, RemoteClientConnectServerSuccessCallback successCallback,
        RemoteClientConnectServerFailCallback failCallback)
    {
        m_remoteClient = new RemoteClient(this, serverIp, serverPort);
        if (m_remoteClient != null)
        {
            m_remoteClient.ConnectServerSuccessCallback = successCallback;
            m_remoteClient.ConnectServerFailCallback = failCallback;
            m_remoteClient.Connect();
            StartCoroutine(UpdateInMainThread4PacketQueue());
        }
    }
    public void Stop()
    {
        if (m_remoteClient != null)
        {
            m_remoteClient.Close();
        }
        StopAllCoroutines();
    }
    #endregion

    #region 处理队列中的Packet
    //处理队列Packet消息，方法二, 目前我尝试采用这种方式
    //使用协程技术，在主线程中处理PacketQueue队列中的Packet包
    IEnumerator UpdateInMainThread4PacketQueue()
    {
        while (true)
        {
            while (m_pendingPacketQueue.Count > 0)
            {
                lock (this)
                {
                    Packet packet = m_pendingPacketQueue.Dequeue();
                    if (packet != null)
                    {
                        //执行消息Packet的后续处理...
                        UInt16 firstId = packet.m_firstId;
                        UInt16 secondId = packet.m_secondId;
                        if (firstId == 0 && secondId == 0)
                        {
                            //一个客户端登录请求的Response
                            ReceiveU3DClientLoginInfoResponse(packet);
                        }
                        else
                        {

                        }
                    }
                }
            }
            yield return null;
        }
    }
    #endregion

    #region PureMVC消息发送
    public void SendNotification(string notificationName, object body, string type)
    {
        if (m_notifier != null)
        {
            m_notifier.SendNotification(notificationName, body, type);
        }
    }
    #endregion

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

    #region Receive网络消息处理方法
    private void ReceiveU3DClientLoginInfoResponse(Packet packet)
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
            Debug.Log("resultId: " + response.m_resultId.ToString());
            string reasonMsg = response.m_msg;
            SendNotification(EventID_Cmd.U3DClientOnLineFail, reasonMsg, null);
        }
    }
    #endregion
}
