
using System;
using System.Collections;
using System.Collections.Generic;

//设备行为同步消息
// sendId=Station自身ID, nodeId=0(0表示服务器Id), firstId=0, secondId=5, msglen=DeviceAction.Size, data=DeviceAction的字节
public class DeviceAction : IPacket
{
    #region 字段和属性
    public int m_deviceId;
    public int m_deviceType;
    public UInt16 m_stationIndex;
    public byte m_deviceStatus;         //设备行为状态值 0表示关，1表示打开
    //数据占用字节大小
    public UInt16 Size
    {
        get { return 11; }
    }
    #endregion

    #region 构造函数
    public DeviceAction()
    {

    }
    public DeviceAction(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_deviceId = BitConverter.ToInt32(bytes, readIndex);
        readIndex += 4;
        m_deviceType = BitConverter.ToInt32(bytes, readIndex);
        readIndex += 4;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_deviceStatus = bytes[readIndex];
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] deviceIdBytes = BitConverter.GetBytes(m_deviceId);
        byte[] deviceTypeBytes = BitConverter.GetBytes(m_deviceType);
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] deviceStatus = BitConverter.GetBytes(m_deviceStatus);

        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(deviceIdBytes, 0, bytes, startIndex, deviceIdBytes.Length);
        startIndex += deviceIdBytes.Length;
        Array.Copy(deviceTypeBytes, 0, bytes, startIndex, deviceTypeBytes.Length);
        startIndex += deviceTypeBytes.Length;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(deviceStatus, 0, bytes, startIndex, deviceStatus.Length);
        return bytes;
    }
    #endregion
}


