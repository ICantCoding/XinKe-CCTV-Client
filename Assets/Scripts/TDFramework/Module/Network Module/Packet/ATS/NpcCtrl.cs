using System;
using System.Collections;
using System.Collections.Generic;

public class NpcCtrl : IPacket
{
    #region 字段和属性
    //所对应的站台索引
    public UInt16 m_stationIndex;
    //上行还是下行, 0上行，1下行
    public byte m_upOrDownFlag;     
    //屏蔽门开关状态， 0上车，1下车
    public byte m_statusFlag; 
    //数据占用字节大小
    public UInt16 Size
    {
        get { return 4; }
    }
    #endregion

    #region 构造函数
    public NpcCtrl()
    {

    }
    public NpcCtrl(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_upOrDownFlag = bytes[readIndex];
        readIndex += 1;
        m_statusFlag = bytes[readIndex];
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] upOrDownFlagBytes = new byte[1]{m_upOrDownFlag};
        byte[] statusFlagBytes = new byte[1]{m_statusFlag};
        
        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(upOrDownFlagBytes, 0, bytes, startIndex, upOrDownFlagBytes.Length);
        startIndex += upOrDownFlagBytes.Length;
        Array.Copy(statusFlagBytes, 0, bytes, startIndex, statusFlagBytes.Length);
        return bytes;
    }
    #endregion
}
