
using System;
using System.Collections;
using System.Collections.Generic;

public class ScreenBindCamera : IPacket
{
    #region 字段和属性
    //站台索引
    public UInt16 m_stationIndex;
    //摄像机索引
    public UInt16 m_cameraIndex;
    //大屏屏幕Index
    public UInt16 m_bigScreenIndex;
    //小屏屏幕Index
    public UInt16 m_smallScreenIndex;
    //摄像机名称
    public string m_cameraName;
    #endregion

    //数据占用字节大小
    public UInt16 Size
    {
        get
        {
            byte[] cameraNameBytes = System.Text.Encoding.UTF8.GetBytes(m_cameraName);
            UInt16 size = (UInt16)(8 + cameraNameBytes.Length);
            return size;
        }
    }

    #region 构造函数
    public ScreenBindCamera()
    {

    }
    public ScreenBindCamera(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_cameraIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_bigScreenIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_smallScreenIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_cameraName = System.Text.Encoding.UTF8.GetString(bytes, readIndex, bytes.Length - readIndex);
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] cameraIndexBytes = BitConverter.GetBytes(m_cameraIndex);
        byte[] bigScreenIndexBytes = BitConverter.GetBytes(m_bigScreenIndex);
        byte[] smallScreenIndexBytes = BitConverter.GetBytes(m_smallScreenIndex);
        byte[] cameraNameBytes = System.Text.Encoding.UTF8.GetBytes(m_cameraName);

        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(cameraIndexBytes, 0, bytes, startIndex, cameraIndexBytes.Length);
        startIndex += cameraIndexBytes.Length;
        Array.Copy(bigScreenIndexBytes, 0, bytes, startIndex, bigScreenIndexBytes.Length);
        startIndex += bigScreenIndexBytes.Length;
        Array.Copy(smallScreenIndexBytes, 0, bytes, startIndex, smallScreenIndexBytes.Length);
        startIndex += smallScreenIndexBytes.Length;
        Array.Copy(cameraNameBytes, 0, bytes, startIndex, cameraNameBytes.Length);
        return bytes;
    }
    #endregion
}
