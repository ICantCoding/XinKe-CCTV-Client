
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TDFramework;

public class NetworkEngine : MonoSingleton<NetworkEngine>
{
    #region 字段和属性
    //待处理的数据包Packet队列.
    private Queue<Packet> m_pendingPacketQueue = new Queue<Packet>();
    private RemoteClient m_remoteClient = null;
    #endregion

    #region Unity生命周期
    void Awake()
    {
        //网络引擎不可销毁
        DontDestroyOnLoad(this.gameObject);
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
    public void Run(string serverIp, int serverPort)
    {
        m_remoteClient = new RemoteClient(this, serverIp, serverPort);
        if (m_remoteClient != null)
        {
            m_remoteClient.Connect();
            StartCoroutine(UpdateInMainThread4PacketQueue());
        }
    }
    public void Stop()
    {
        if(m_remoteClient != null)
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
        while (m_pendingPacketQueue.Count > 0)
        {
            lock (this)
            {
                Packet packet = m_pendingPacketQueue.Dequeue();
                //执行消息Packet的后续处理...

            }
        }
        yield return null;
    }
    #endregion
}
