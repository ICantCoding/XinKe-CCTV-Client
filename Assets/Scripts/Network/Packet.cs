
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
        
        AppendNetworkBytes(bytes, m_sendId, ref startIndex);
        AppendNetworkBytes(bytes, m_nodeId, ref startIndex);
        AppendNetworkBytes(bytes, m_firstId, ref startIndex);
        AppendNetworkBytes(bytes, m_secondId, ref startIndex);
        AppendNetworkBytes(bytes, m_msgLen, ref startIndex);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(m_data, 0, m_data.Length);
        }
        Array.Copy(m_data, 0, bytes, startIndex, m_data.Length);

        return bytes;
    }
    private void AppendNetworkBytes(byte[] sourcesBytes, System.UInt16 appendData, ref int startIndex)
    {   
        byte[] appendBytes = BitConverter.GetBytes(appendData);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(appendBytes, 0, appendBytes.Length);
        }
        Array.Copy(appendBytes, 0, sourcesBytes, startIndex, appendBytes.Length);
        startIndex += appendBytes.Length;
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
