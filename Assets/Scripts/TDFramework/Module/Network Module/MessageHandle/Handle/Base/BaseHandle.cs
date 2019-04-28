using System;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Observer;

public class BaseHandle
{
    #region 字段
    public string Name;
    protected Notifier m_notifier = null;
    protected BaseNetworkEngine m_networkEngine;
    #endregion

    #region 构造函数
    public BaseHandle(BaseNetworkEngine networkEngine)
    {
        m_notifier = new Notifier();
        m_networkEngine = networkEngine;
    }
    #endregion

    #region 方法
    public void SendNotification(string notificationName, object body, string type)
    {
        if (m_notifier != null)
        {
            m_notifier.SendNotification(notificationName, body, type);
        }
    }
    #endregion

    #region 虚方法
    public virtual void ReceivePacket(Packet packet)
    {

    }
    #endregion
}
