using System;
using System.Collections;
using System.Collections.Generic;

public class TrainDoorCtrl : IPacket
{
    #region 字段和属性
    //屏蔽门所对应的站台索引
    public UInt16 m_stationIndex;
    //上行列车还是下行列车，0表示上行，1表示下行
    public byte m_upOrDownFalg;
    //列车左侧车门或列车右侧车门
    public byte m_leftOrRightDoorFlag;
    //屏蔽门开关状态
    public byte m_statusFlag;
    //数据占用字节大小
    public UInt16 Size
    {
        get { return 5; }
    }
    #endregion

    #region 构造函数
    public TrainDoorCtrl()
    {

    }
    public TrainDoorCtrl(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_upOrDownFalg = bytes[readIndex];
        readIndex += 1;
        m_leftOrRightDoorFlag = bytes[readIndex];
        readIndex += 1;
        m_statusFlag = bytes[readIndex];
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] upOrDownFlag = new byte[1] { m_upOrDownFalg };
        byte[] leftOrRightDoorFlag = new byte[1] { m_leftOrRightDoorFlag };
        byte[] statusFlagBytes = new byte[1] { m_statusFlag };

        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(upOrDownFlag, 0, bytes, startIndex, upOrDownFlag.Length);
        startIndex += upOrDownFlag.Length;
        Array.Copy(leftOrRightDoorFlag, 0, bytes, startIndex, leftOrRightDoorFlag.Length);
        startIndex += leftOrRightDoorFlag.Length;
        Array.Copy(statusFlagBytes, 0, bytes, startIndex, statusFlagBytes.Length);
        return bytes;
    }
    #endregion
}
