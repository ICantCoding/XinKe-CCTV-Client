
using System;
using System.Collections;
using System.Collections.Generic;

public class DivisionBigScreen : IPacket
{
    #region 字段和属性
    public UInt16 m_bigScreenXDivisionCount; //一级分屏横向个数
    public UInt16 m_bigScreenYDivisionCount; //一级分屏纵向个数
    //数据占用字节大小
    public UInt16 Size
    {
        get { return 4; }
    }
    #endregion

    #region 构造函数
    public DivisionBigScreen()
    {

    }
    public DivisionBigScreen(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_bigScreenXDivisionCount = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_bigScreenYDivisionCount = BitConverter.ToUInt16(bytes, readIndex);
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] bigScreenXDivisionCountBytes = BitConverter.GetBytes(m_bigScreenXDivisionCount);
        byte[] bigScreenYDivisionCountBytes = BitConverter.GetBytes(m_bigScreenYDivisionCount);

        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(bigScreenXDivisionCountBytes, 0, bytes, startIndex, bigScreenXDivisionCountBytes.Length);
        startIndex += bigScreenXDivisionCountBytes.Length;
        Array.Copy(bigScreenYDivisionCountBytes, 0, bytes, startIndex, bigScreenYDivisionCountBytes.Length);
        return bytes;
    }
    #endregion
}
