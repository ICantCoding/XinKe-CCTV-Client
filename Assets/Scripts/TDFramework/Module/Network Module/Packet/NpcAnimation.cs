
using System;
using System.Collections;
using System.Collections.Generic;

//Npc同步动作信息 
// sendId=Station自身ID, nodeId=0(0表示服务器Id), firstId=0, secondId=2, msglen=NpcPosition.Size, data=NpcPosition的字节
public class NpcAnimation : IPacket
{
    #region 字段和属性
    public UInt16 m_npcAnimationType; //Npc进行的行为
    public int m_npcId;
    public UInt16 m_stationIndex;
    public UInt16 m_stationClientType;
    //数据占用字节大小
    public UInt16 Size
    {
        get { return 10; }
    }
    #endregion

    #region 构造函数
    public NpcAnimation()
    {

    }
    public NpcAnimation(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_npcAnimationType = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_npcId = BitConverter.ToInt32(bytes, readIndex);
        readIndex += 4;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_stationClientType = BitConverter.ToUInt16(bytes, readIndex);
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] npcAnimationTypeBytes = BitConverter.GetBytes(m_npcAnimationType);
        byte[] npcIdBytes = BitConverter.GetBytes(m_npcId);
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] stationClientTypeBytes = BitConverter.GetBytes(m_stationClientType);

        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(npcAnimationTypeBytes, 0, bytes, startIndex, npcAnimationTypeBytes.Length);
        startIndex += npcAnimationTypeBytes.Length;
        Array.Copy(npcIdBytes, 0, bytes, startIndex, npcIdBytes.Length);
        startIndex += npcIdBytes.Length;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(stationClientTypeBytes, 0, bytes, startIndex, stationClientTypeBytes.Length);
        return bytes;
    }
    #endregion
}


