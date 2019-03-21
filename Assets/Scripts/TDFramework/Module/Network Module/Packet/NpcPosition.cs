using System;
using System.Collections;
using System.Collections.Generic;

public class NpcPosition : IPacket
{
    #region 字段和属性
    public float m_posX;
    public float m_posY;
    public float m_posZ;
    public float m_angleX;
    public float m_angleY;
    public float m_angleZ;
    public int m_npcId;
    public UInt16 Size
    {
        get
        {
            byte[] posXBytes = BitConverter.GetBytes(m_posX);
            byte[] posYBytes = BitConverter.GetBytes(m_posY);
            byte[] posZBytes = BitConverter.GetBytes(m_posZ);
            byte[] angleXBytes = BitConverter.GetBytes(m_angleX);
            byte[] angleYBytes = BitConverter.GetBytes(m_angleY);
            byte[] angleZBytes = BitConverter.GetBytes(m_angleZ);
            byte[] npcIdBytes = BitConverter.GetBytes(m_npcId);
            UInt16 size = (UInt16)(posXBytes.Length + posYBytes.Length + posZBytes.Length + angleXBytes.Length + angleYBytes.Length +
                angleZBytes.Length + npcIdBytes.Length);
            return size;
        }
    }
    #endregion

    #region 构造函数
    public NpcPosition()
    {

    }
    public NpcPosition(byte[] bytes)
    {
        if (bytes.Length <= 0) return;
        int readIndex = 0;
        m_posX = BitConverter.ToSingle(bytes, readIndex);
        readIndex += 4;
        m_posY = BitConverter.ToSingle(bytes, readIndex);
        readIndex += 4;
        m_posZ = BitConverter.ToSingle(bytes, readIndex);
        readIndex += 4;
        m_angleX = BitConverter.ToSingle(bytes, readIndex);
        readIndex += 4;
        m_angleY = BitConverter.ToSingle(bytes, readIndex);
        readIndex += 4;
        m_angleZ = BitConverter.ToSingle(bytes, readIndex);
        readIndex += 4;
        m_npcId = BitConverter.ToInt32(bytes, readIndex);
    }
    #endregion 

    public byte[] Packet2Bytes()
    {
        byte[] posXBytes = BitConverter.GetBytes(m_posX);
        byte[] posYBytes = BitConverter.GetBytes(m_posY);
        byte[] posZBytes = BitConverter.GetBytes(m_posZ);
        byte[] angleXBytes = BitConverter.GetBytes(m_angleX);
        byte[] angleYBytes = BitConverter.GetBytes(m_angleY);
        byte[] angleZBytes = BitConverter.GetBytes(m_angleZ);
        byte[] npcIdBytes = BitConverter.GetBytes(m_npcId);
        int size = posXBytes.Length + posYBytes.Length + posZBytes.Length + angleXBytes.Length + angleYBytes.Length +
                angleZBytes.Length + npcIdBytes.Length;
        byte[] bytes = new byte[size];
        int startIndex = 0;
        Array.Copy(posXBytes, 0, bytes, startIndex, posXBytes.Length);
        startIndex += posXBytes.Length;
        Array.Copy(posYBytes, 0, bytes, startIndex, posYBytes.Length);
        startIndex += posYBytes.Length;
        Array.Copy(posZBytes, 0, bytes, startIndex, posZBytes.Length);
        startIndex += posZBytes.Length;
        Array.Copy(angleXBytes, 0, bytes, startIndex, angleXBytes.Length);
        startIndex += angleXBytes.Length;
        Array.Copy(angleYBytes, 0, bytes, startIndex, angleYBytes.Length);
        startIndex += angleYBytes.Length;
        Array.Copy(angleZBytes, 0, bytes, startIndex, angleZBytes.Length);
        startIndex += angleZBytes.Length;
        Array.Copy(npcIdBytes, 0, bytes, startIndex, npcIdBytes.Length);

        return bytes;
    }
}
