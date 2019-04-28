using System;
using System.Collections;
using System.Collections.Generic;

public class CameraRotate : IPacket
{
    #region 字段和属性
    //站台索引
    public UInt16 m_stationIndex;
    //摄像机索引
    public UInt16 m_cameraIndex;
    //旋转控制，0向左，1向右，2向上，3向下
    public byte m_rotateUpOrDown;    
    //数据占用字节大小
    public UInt16 Size {
        get { return 5; }
    }
    #endregion

    #region 构造函数
    public CameraRotate () {

    }
    public CameraRotate (byte[] bytes) {
        if (bytes.Length <= 0) {
            return;
        }
        int readIndex = 0;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_cameraIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_rotateUpOrDown = bytes[readIndex];
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes () {
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] cameraIndexBytes = BitConverter.GetBytes(m_cameraIndex);
        byte[] rotateUpOrDownBytes = new byte[]{m_rotateUpOrDown};

        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(cameraIndexBytes, 0, bytes, startIndex, cameraIndexBytes.Length);
        startIndex += cameraIndexBytes.Length;
        Array.Copy (rotateUpOrDownBytes, 0, bytes, startIndex, rotateUpOrDownBytes.Length);
        return bytes;
    }
    #endregion
}
