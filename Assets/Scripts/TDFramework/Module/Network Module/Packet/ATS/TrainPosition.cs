using System;
using System.Collections;
using System.Collections.Generic;

public class TrainPosition : IPacket
{
    #region 字段和属性
    //站台索引
    public UInt16 m_stationIndex;
    //上行还是下行,0表示上行,1表示下行
    public byte m_upOrDownFlag;
    //正向行车还是反向行车,0表示正向行车,1表示反向行车
    public byte m_positiveOrNegativeDir;
    //对标距离
    public float m_markDistance;
    public UInt16 Size
    {
        get { return 8; }
    }
    #endregion

    #region 构造函数
    public TrainPosition()
    {

    }
    public TrainPosition(byte[] bytes)
    {
        if (bytes.Length <= 0) return;
        int readIndex = 0;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_upOrDownFlag = bytes[readIndex];
        readIndex += 1;
        m_positiveOrNegativeDir = bytes[readIndex];
        readIndex += 1;
        m_markDistance = BitConverter.ToSingle(bytes, readIndex);
    }
    #endregion 

    public byte[] Packet2Bytes()
    {
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] upOrDownFlagBytes = BitConverter.GetBytes(m_upOrDownFlag);
        byte[] positiveOrNegativeDirBytes = BitConverter.GetBytes(m_positiveOrNegativeDir);
        byte[] markDistanceBytes = BitConverter.GetBytes(m_markDistance);
        byte[] bytes = new byte[Size];

        int startIndex = 0;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(upOrDownFlagBytes, 0, bytes, startIndex, upOrDownFlagBytes.Length);
        startIndex += upOrDownFlagBytes.Length;
        Array.Copy(positiveOrNegativeDirBytes, 0, bytes, startIndex, positiveOrNegativeDirBytes.Length);
        startIndex += positiveOrNegativeDirBytes.Length;
        Array.Copy(markDistanceBytes, 0, bytes, startIndex, markDistanceBytes.Length);
        return bytes;
    }
}
