using System;
using System.Collections;
using System.Collections.Generic;

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

