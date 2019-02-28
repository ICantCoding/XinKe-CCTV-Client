
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class SendBuffer
{
    public int position = 0;
    public byte[] buffer;
    public int Size
    {
        get { return buffer.Length - position; }
    }
}

public delegate void MessageHandler(Packet msg);

public class RemoteClient
{
    #region 常量
    private const int BufferSize = 8192;
    #endregion

    #region 字段
    private Socket m_clientSocket;
    IPEndPoint m_endPoint;
    private byte[] m_buffer = new byte[BufferSize]; //接收数据流缓存，Tcp数据流无界限
    private List<SendBuffer> m_sendBufferList = new List<SendBuffer>();
    private MessageReader m_messageReader = null;
    private Dictionary<uint, MessageHandler> m_flowHandlerDict = new Dictionary<uint, MessageHandler>();
    private IMainLoop m_mainLoop = null;
    #endregion

    #region 构造方法
    public RemoteClient(IMainLoop loop)
    {
        m_messageReader = new MessageReader(HandleMessage);
        m_mainLoop = loop;
    }
    #endregion

    #region 方法
    //MessageReader对象获取到完整的一个数据包后，需执行这个回调
    void HandleMessage(Packet packet)
    {
        //接下来就是处理消息....最好是放入队里中处理.
        if (m_mainLoop != null)
        {
            m_mainLoop.QueueInLoop(packet);
        }
    }
    //连接服务器
    public void Connect(string ip, int port)
    {
        m_endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        try
        {
            m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult result = m_clientSocket.BeginConnect(m_endPoint, new AsyncCallback(OnConnectCallback), null);
            //开启线程，检查客户端连接服务器是否成功.
            Thread thread = new Thread(CheckConnected);
            thread.Start(result);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
            Close();
        }
    }
    //断开服务器的连接，销毁Socket
    public void Close()
    {
        try
        {
            if (m_clientSocket != null && m_clientSocket.Connected)
            {
                m_clientSocket.Shutdown(SocketShutdown.Both);
            }
            m_clientSocket.Close();
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
        m_clientSocket = null;
    }
    //判断客户端是否连接有效
    public bool Valid()
    {
        if (m_clientSocket != null && m_clientSocket.Connected == true)
        {
            return true;
        }
        return false;
    }
    private void OnConnectCallback(IAsyncResult ar)
    {
        if (m_clientSocket == null)
        {
            return;
        }
        bool success = false;
        try
        {
            m_clientSocket.EndConnect(ar);
            success = true;
        }
        catch (Exception exception)
        {
            m_clientSocket.Close();
            Debug.LogError(exception.Message);
            success = false;
        }
        if (success)
        {
            //连接服务器成功， 可在此添加连接服务器成功的操作, 目前还不知道需要执行什么操作，暂时保留
            Debug.Log("连接成功...");
        }
    }

    private void OnReceiveCallback(IAsyncResult ar)
    {
        int bytes = 0;
        try
        {
            if (Valid())
            {
                bytes = m_clientSocket.EndReceive(ar);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
            Close();
        }
        if (bytes <= 0)
        {
            Close();
        }
        else
        {
            uint num = (uint)bytes;
            if (m_messageReader != null)
            {
                m_messageReader.Process(m_buffer, num);
            }
            try
            {
                m_clientSocket.BeginReceive(m_buffer, 0, m_buffer.Length, SocketFlags.None,
                    new AsyncCallback(OnReceiveCallback), null);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                Close();
            }
        }
    }
    public void Send(byte[] datas)
    {
        lock (m_sendBufferList)
        {
            SendBuffer sb = new SendBuffer()
            {
                position = 0,
                buffer = datas
            };
            m_sendBufferList.Add(sb);
            if (m_sendBufferList.Count == 1)
            {
                try
                {
                    IAsyncResult ar = m_clientSocket.BeginSend(sb.buffer, sb.position, sb.Size,
                        SocketFlags.None, new AsyncCallback(OnSendCallback), null);
                    //开启线程检查发送数据是否正确, 是否超时, 瞬间发送10000条数据，那这里岂不是瞬间要开启10000个线程，这里有问题哈！
                    ThreadPool.QueueUserWorkItem(CheckSendTimeout, ar);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                    Close();
                }
            }
        }
    }
    private void OnSendCallback(IAsyncResult ar)
    {
        int num = 0;
        try
        {
            num = m_clientSocket.EndSend(ar);
        }
        catch (Exception exception)
        {
            num = 0;
            Close();
            Debug.LogError(exception.Message);
            return;
        }
        lock (m_sendBufferList)
        {
            if (Valid())
            {
                SendBuffer sb = m_sendBufferList[0];
                SendBuffer nextSb = null;
                if (sb.Size == num)
                {
                    //表明SendBuffer中的数据全部发送完成
                    m_sendBufferList.RemoveAt(0);
                    if (m_sendBufferList.Count > 0)
                    {
                        nextSb = m_sendBufferList[0];
                    }
                }
                else if (sb.Size > num)
                {
                    //表明SendBuffer中的数据没有全部发送出去
                    sb.position += num;
                    nextSb = m_sendBufferList[0];
                }
                else
                {
                    //表明上一个SendBuffer数据包发送错误，我们就不管上一个数据包了
                    m_sendBufferList.RemoveAt(0);
                    if (m_sendBufferList.Count > 0)
                    {
                        nextSb = m_sendBufferList[0];
                    }
                }
                if (nextSb != null)
                {
                    try
                    {
                        m_clientSocket.BeginSendTo(nextSb.buffer, nextSb.position, nextSb.Size,
                            SocketFlags.None, m_endPoint, OnSendCallback, null);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError(exception.Message);
                        Close();
                    }
                }
            }
        }
    }
    private void CheckSendTimeout(object obj)
    {
        try
        {
            IAsyncResult ar = (IAsyncResult)obj;
            if (!ar.AsyncWaitHandle.WaitOne(3000))
            {
                Debug.LogError("Send Timeout.");
                Close();
            }
            else
            {
                UnityEngine.Debug.Log("消息发送成功.");
            }
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
    }
    #endregion

    #region 线程执行方法
    private void CheckConnected(object obj)
    {
        IAsyncResult result = (IAsyncResult)obj;
        //阻止当前线程，直到当前 WaitHandle 收到信号，同时使用 32 位带符号整数指定时间间隔（以毫秒为单位）
        //到信号，则为 true
        if (result != null && !result.AsyncWaitHandle.WaitOne(5000)) //3秒无响应则连接服务器失败
        {
            Debug.LogError("Connect Server Error.");
            Close();
        }
        else
        {
            //开始接受数据
            StartReceive();
            //首次向服务器发送客户端信息.
            Debug.Log("连接服务器成功，需要传递自身信息到服务器...");
            Packet packet = new Packet(GameGlobal.Instance.PlayerInfo.Id, 101, 102, 103, 2, BitConverter.GetBytes(GameGlobal.Instance.PlayerInfo.Id));
            Send(packet.Packet2Bytes());
        }
    }
    private void StartReceive()
    {
        try
        {
            m_clientSocket.BeginReceive(m_buffer, 0, m_buffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallback), null);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
            Close();
        }
    }
    #endregion

    #region Send方法扩展

    #endregion
}
