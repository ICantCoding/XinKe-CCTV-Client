using System;
using System.Collections;
using System.Collections.Generic;

public class CameraZoom : IPacket 
{
    #region 字段和属性
    //站台索引
    public UInt16 m_stationIndex;
    //摄像机索引
    public UInt16 m_cameraIndex;
    //缩放控制，0缩小,1放大
    public byte m_zoomUpOrDown;    
    //数据占用字节大小
    public UInt16 Size {
        get { return 5; }
    }
    #endregion

    #region 构造函数
    public CameraZoom () {

    }
    public CameraZoom (byte[] bytes) {
        if (bytes.Length <= 0) {
            return;
        }
        int readIndex = 0;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_cameraIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_zoomUpOrDown = bytes[readIndex];
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes () {
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] cameraIndexBytes = BitConverter.GetBytes(m_cameraIndex);
        byte[] zoomUpOrDownBytes = new byte[]{m_zoomUpOrDown};

        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(cameraIndexBytes, 0, bytes, startIndex, cameraIndexBytes.Length);
        startIndex += cameraIndexBytes.Length;
        Array.Copy (zoomUpOrDownBytes, 0, bytes, startIndex, zoomUpOrDownBytes.Length);
        return bytes;
    }
    #endregion
}