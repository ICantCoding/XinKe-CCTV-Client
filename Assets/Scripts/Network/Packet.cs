
using System;
using System.Collections;
using System.Collections.Generic;

public class Packet
{
    #region 字段
    private System.UInt16 m_sendId;
    private System.UInt16 m_nodeId;
    private System.UInt16 m_firstId;
    private System.UInt16 m_secondId;
    private System.UInt16 m_msgLen;
    public byte[] m_data;//服务器响应报文的具体数据内容
    #endregion

    #region 构造方法
    public Packet(UInt16 sendId, UInt16 nodeId, UInt16 firstId, UInt16 secondId, UInt16 msgLen, byte[] bytes)
    {
        UnityEngine.Debug.Log("SendId: " + sendId.ToString());
        this.m_sendId = sendId;
        this.m_nodeId = nodeId;
        this.m_firstId = firstId;
        this.m_secondId = secondId;
        this.m_msgLen = msgLen;
        this.m_data = bytes;
    }
    public Packet()
    {

    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        int length = 2 + 2 + 2 + 2 + 2 + m_data.Length;
        int startIndex = 0;
        byte[] bytes = new byte[length];
        
        byte[] sendIdBytes = BitConverter.GetBytes(m_sendId);
        Array.Copy(sendIdBytes, 0, bytes, 0, sendIdBytes.Length);
        startIndex += sendIdBytes.Length;

        byte[] nodeIdBytes = BitConverter.GetBytes(m_nodeId);
        Array.Copy(nodeIdBytes, 0, bytes, startIndex, nodeIdBytes.Length);
        startIndex += nodeIdBytes.Length;

        byte[] firstIdBytes = BitConverter.GetBytes(m_firstId);
        Array.Copy(firstIdBytes, 0, bytes, startIndex, firstIdBytes.Length);
        startIndex += firstIdBytes.Length;

        byte[] secondIdBytes = BitConverter.GetBytes(m_secondId);
        Array.Copy(secondIdBytes, 0, bytes, startIndex, secondIdBytes.Length);
        startIndex += secondIdBytes.Length;

        byte[] msgLenBytes = BitConverter.GetBytes(m_msgLen);
        Array.Copy(msgLenBytes, 0, bytes, startIndex, msgLenBytes.Length);
        startIndex += msgLenBytes.Length;

        Array.Copy(m_data, 0, bytes, startIndex, m_data.Length);

        //判断当前系统是否是小端序， 注意网路传输（网络字节序）是大端序
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes, 0, bytes.Length);
        }

        return bytes;
    }
    #endregion
}

/*
网络传输数据解析:
========================================================
1. 客户端告知服务器自身ID信息
sendId = 客户端自身ID
nodeId = 0 表示服务器ID
firstId = 0
secondId = 0
m_msgLen = 2 表示数据部分2个字节
dada = sendId
















*/
