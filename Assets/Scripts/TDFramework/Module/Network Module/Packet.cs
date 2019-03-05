
using System;
using System.Collections;
using System.Collections.Generic;

#region IPacket接口
//接口，继承该接口的类，具有转换为byte[]数组的功能
public interface IPacket
{
    byte[] Packet2Bytes();
}
#endregion

#region 网络传输数据包
public class Packet : IPacket
{
    #region 字段
    public System.UInt16 m_sendId;
    public System.UInt16 m_nodeId;
    public System.UInt16 m_firstId;
    public System.UInt16 m_secondId;
    public System.UInt16 m_msgLen;
    public byte[] m_data;//服务器响应报文的具体数据内容
    #endregion

    #region 构造方法
    public Packet(UInt16 sendId, UInt16 nodeId, UInt16 firstId, UInt16 secondId, UInt16 msgLen, byte[] bytes)
    {
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
#endregion

#region 客户端登录数据包
//客户端连接到服务器后，发送自身信息. 
// sendId=客户端自身ID, nodeId=0(0表示服务器Id), firstId=0, secondId=0, msglen=U3DClientLogin.Size, data=U3DClientLogin的字节
public class U3DClientLogin : IPacket
{
    #region 字段和属性
    public UInt16 m_clientId; //U3D客户端ID
    public string m_clientName; //U3D客户端名字
    //数据占用字节大小
    public UInt16 Size
    {
        get
        {
            byte[] clientIdBytes = BitConverter.GetBytes(m_clientId);
            byte[] clientNameBytes = System.Text.Encoding.UTF8.GetBytes(m_clientName);
            UInt16 size = (UInt16)(clientIdBytes.Length + clientNameBytes.Length);
            return size;
        }
    }
    #endregion

    #region 构造函数
    public U3DClientLogin()
    {

    }
    public U3DClientLogin(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_clientId = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_clientName = System.Text.Encoding.UTF8.GetString(bytes, readIndex, bytes.Length - readIndex);
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] clientIdBytes = BitConverter.GetBytes(m_clientId);
        byte[] clientNameBytes = System.Text.Encoding.UTF8.GetBytes(m_clientName);

        int size = clientIdBytes.Length + clientNameBytes.Length;
        byte[] bytes = new byte[size];
        int startIndex = 0;
        Array.Copy(clientIdBytes, 0, bytes, startIndex, clientIdBytes.Length);
        startIndex += clientIdBytes.Length;
        Array.Copy(clientNameBytes, 0, bytes, startIndex, clientNameBytes.Length);

        return bytes;
    }
    #endregion
}
public class U3DClientLoginResponse : IPacket
{
    #region 字段和属性
    public UInt16 m_resultId; //返回ResultId
    public string m_msg; //返回原因
    //数据占用字节大小
    public UInt16 Size
    {
        get
        {
            byte[] resultIdBytes = BitConverter.GetBytes(m_resultId);
            byte[] msgBytes = System.Text.Encoding.UTF8.GetBytes(m_msg);
            UInt16 size = (UInt16)(resultIdBytes.Length + msgBytes.Length);
            return size;
        }
    }
    #endregion

    #region 构造函数
    public U3DClientLoginResponse()
    {

    }
    public U3DClientLoginResponse(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_resultId = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_msg = System.Text.Encoding.UTF8.GetString(bytes, readIndex, bytes.Length - readIndex);
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] resultIdBytes = BitConverter.GetBytes(m_resultId);
        byte[] msgBytes = System.Text.Encoding.UTF8.GetBytes(m_msg);
        int size = resultIdBytes.Length + msgBytes.Length;
        byte[] bytes = new byte[size];
        int startIndex = 0;
        Array.Copy(resultIdBytes, 0, bytes, startIndex, resultIdBytes.Length);
        startIndex += resultIdBytes.Length;
        Array.Copy(msgBytes, 0, bytes, startIndex, msgBytes.Length);
        return bytes;
    }
    #endregion
}
#endregion