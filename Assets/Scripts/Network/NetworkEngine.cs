
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NetworkEngine : IMainLoop
{
    #region 字段和属性
    public Queue<Packet> m_pendingPacketQueue = new Queue<Packet>();
    #endregion

    #region 构造函数
    public NetworkEngine()
    {
        RemoteClient client = new RemoteClient(this);
        client.Connect("127.0.0.1", 3322);
    }
    #endregion

    public void QueueInLoop(Packet packet)
    {
        lock (this)
        {
            m_pendingPacketQueue.Enqueue(packet);
        }
    }
    // Main Thread Update, 每帧在主线程中执行完所有的回调
    private static int count = 0;
    public void UpdateInMainThread()
    {
        while (m_pendingPacketQueue.Count > 0)
        {
            lock (this)
            {
                Packet packet = m_pendingPacketQueue.Dequeue();
                count++;
                Debug.Log("count: " + count);
            }
        }
    }
}
