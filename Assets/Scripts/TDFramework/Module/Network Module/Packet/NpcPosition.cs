using System;
using System.Collections;
using System.Collections.Generic;

//Npc同步位置信息 
// sendId=Station自身ID, nodeId=0(0表示服务器Id), firstId=0, secondId=2, msglen=NpcPosition.Size, data=NpcPosition的字节
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
    public int m_npcType;
    public UInt16 m_stationIndex;
    public UInt16 m_stationClientType;
    public UInt16 Size
    {
        get { return 32; }
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
        readIndex += 4;
        m_npcType = BitConverter.ToInt32(bytes, readIndex);
        readIndex += 4;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_stationClientType = BitConverter.ToUInt16(bytes, readIndex);
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
        byte[] npcTypeBytes = BitConverter.GetBytes(m_npcType);
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] stationClientTypeBytes = BitConverter.GetBytes(m_stationClientType);
        byte[] bytes = new byte[Size];

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
        startIndex += npcIdBytes.Length;
        Array.Copy(npcTypeBytes, 0, bytes, startIndex, npcTypeBytes.Length);
        startIndex += npcTypeBytes.Length;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(stationClientTypeBytes, 0, bytes, startIndex, stationClientTypeBytes.Length);
        return bytes;
    }
}
