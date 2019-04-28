
using System;
using System.Collections;
using System.Collections.Generic;

public class DivisionSmallScreen : IPacket
{
    #region 字段和属性
    public UInt16 m_bigScreenIndex;            //将那一块大屏进行二级分屏
    public UInt16 m_smallScreenXDivisionCount; //二级分屏横向个数
    public UInt16 m_smallScreenYDivisionCount; //二级分屏纵向个数
    //数据占用字节大小
    public UInt16 Size
    {
        get { return 6; }
    }
    #endregion

    #region 构造函数
    public DivisionSmallScreen()
    {

    }
    public DivisionSmallScreen(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_bigScreenIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_smallScreenXDivisionCount = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_smallScreenYDivisionCount = BitConverter.ToUInt16(bytes, readIndex);
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] bigScreenIndexBytes = BitConverter.GetBytes(m_bigScreenIndex);
        byte[] smallScreenXDivisionCountBytes = BitConverter.GetBytes(m_smallScreenXDivisionCount);
        byte[] smallScreenYDivisionCountBytes = BitConverter.GetBytes(m_smallScreenYDivisionCount);

        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(bigScreenIndexBytes, 0, bytes, startIndex, bigScreenIndexBytes.Length);
        startIndex += bigScreenIndexBytes.Length;
        Array.Copy(smallScreenXDivisionCountBytes, 0, bytes, startIndex, smallScreenXDivisionCountBytes.Length);
        startIndex += smallScreenXDivisionCountBytes.Length;
        Array.Copy(smallScreenYDivisionCountBytes, 0, bytes, startIndex, smallScreenYDivisionCountBytes.Length);
        return bytes;
    }
    #endregion    
}
