using System;
using System.Collections;
using System.Collections.Generic;

//Station连接到服务器后，发送自身信息. 
// sendId=Station自身ID, nodeId=0(0表示服务器Id), firstId=0, secondId=1, msglen=StationClientLogin.Size, data=StationClientLogin的字节
public class StationClientLogin : IPacket
{
    #region 字段和属性
    // public const UInt16 ShangXingShangChe = 1;
    // public const UInt16 ShangXingXiaChe = 2;
    // public const UInt16 XiaXingShangChe = 3;
    // public const UInt16 XiaXingXiaChe = 4;
    public UInt16 m_stationClientType; //StationSocket类型, 1, 2, 3, 4
    public UInt16 m_stationIndex; //Station索引值
    public UInt16 Size
    {
        get
        {
            byte[] stationClientTypeBytes = BitConverter.GetBytes(m_stationClientType);
            byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
            UInt16 size = (UInt16)(stationClientTypeBytes.Length + stationIndexBytes.Length);
            return size;
        }
    }
    #endregion

    #region 构造函数
    public StationClientLogin()
    {
        
    }
    public StationClientLogin(byte[] bytes)
    {
        if(bytes.Length <= 0) return;
        int readIndex = 0;
        m_stationClientType = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
    }
    #endregion 

    public byte[] Packet2Bytes()
    {
        byte[] stationClientTypeBytes = BitConverter.GetBytes(m_stationClientType);
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);

        int size = stationClientTypeBytes.Length + stationIndexBytes.Length;
        byte[] bytes = new byte[size];
        int startIndex = 0;
        Array.Copy(stationClientTypeBytes, 0, bytes, startIndex, stationClientTypeBytes.Length);
        startIndex += stationClientTypeBytes.Length;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);

        return bytes;
    }
}
public class StationClientLoginResponse : IPacket
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
    public StationClientLoginResponse()
    {

    }
    public StationClientLoginResponse(byte[] bytes)
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

