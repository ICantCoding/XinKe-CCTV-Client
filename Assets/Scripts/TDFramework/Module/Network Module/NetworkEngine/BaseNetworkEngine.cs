
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TDFramework;
using PureMVC.Patterns.Observer;

public class BaseNetworkEngine : MonoBehaviour, INetworkEngine
{
    #region 组合功能
    protected Notifier m_notifier = null;
    #endregion

    #region 字段
    //待处理的数据包Packet队列.
    protected Queue<Packet> m_pendingPacketQueue = new Queue<Packet>();
    protected RemoteClient m_remoteClient = null;
    #endregion

    #region 状态属性
    //客户端是否已经连接到服务器
    public bool IsConnected
    {
        get
        {
            if (m_remoteClient != null)
            {
                return m_remoteClient.Valid();
            }
            return false;
        }
    }
    #endregion

    #region Unity生命周期
    protected virtual void Awake()
    {
        //网络引擎不可销毁
        m_notifier = new Notifier();
    }
    protected virtual void OnDestroy()
    {
        Stop();
    }
    protected virtual void OnApplicationQuit()
    {
        Stop();
    }
    #endregion

    #region INetworkEngine抽象方法实现
    public void Packet2NetworkEnginePendingPacketQueue(Packet packet)
    {
        lock (this)
        {
            m_pendingPacketQueue.Enqueue(packet);
        }
    }
    #endregion

    #region 方法
    public virtual void Run(RemoteClientConnectServerSuccessCallback successCallback,
        RemoteClientConnectServerFailCallback failCallback)
    {
        Run(SingletonMgr.GameGlobalInfo.PlayerInfo.ServerIpAddress,
            SingletonMgr.GameGlobalInfo.PlayerInfo.ServerPort,
            successCallback,
            failCallback);
    }
    public virtual void Run(string serverIp, int serverPort, RemoteClientConnectServerSuccessCallback successCallback,
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
    public virtual void Stop()
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
    protected IEnumerator UpdateInMainThread4PacketQueue()
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
                        else if (firstId == 0 && secondId == 1)
                        {
                            ReceiveStationClientLoginInfoResponse(packet);
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
    public virtual void SendU3DClientLoginInfoRequest()
    {

    }
    public virtual void SendStationClientLoginInfoRequest()
    {

    }
    #endregion

    #region Receive网络消息处理方法
    public virtual void ReceiveU3DClientLoginInfoResponse(Packet packet)
    {

    }
    public virtual void ReceiveStationClientLoginInfoResponse(Packet packet)
    {

    }
    #endregion
}
