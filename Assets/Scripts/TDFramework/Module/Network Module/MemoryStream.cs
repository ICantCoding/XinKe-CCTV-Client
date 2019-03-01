


using System;
using System.Collections;
using System.Collections.Generic;

public class MemoryStream
{
    #region 常量
    public const int BUFFER_MAX = 1460 * 4;
    #endregion

    #region 字段和属性
    public System.UInt32 rpos = 0;
    public System.UInt32 wpos = 0; //记录当前写字节处的位置索引
    private byte[] datas = new byte[BUFFER_MAX];
    public byte[] Data
    {
        get { return datas; }
    }
    #endregion

    #region 方法
    public System.Byte ReadByte()
    {
        return datas[rpos++];
    }
    public UInt32 ReadUInt32()
    {
        rpos += 4;
        //判断当前系统是否是小端序， 注意网路传输（网络字节序）是大端序
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(datas, (int)(rpos - 4), 4);
        }
        return BitConverter.ToUInt32(datas, (int)(rpos - 4));
    }
    public UInt16 ReadUInt16()
    {
        rpos += 2;
        //判断当前系统是否是小端序， 注意网路传输（网络字节序）是大端序
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(datas, (int)(rpos - 2), 2);
        }
        return BitConverter.ToUInt16(datas, (int)(rpos - 2));
    }
    public Int16 ReadInt16()
    {
        rpos += 2;
        //判断当前系统是否是小端序， 注意网路传输（网络字节序）是大端序
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(datas, (int)(rpos - 2), 2);
        }
        return BitConverter.ToInt16(datas, (int)(rpos - 2));
    }
    public byte[] GetBytes(System.UInt32 startIndex, System.UInt32 length)
    {
        byte[] bytes = new byte[length];
        Array.Copy(Data, startIndex, bytes, 0, length);
        return bytes;
    }
    public void Clear()
    {
        rpos = wpos = 0;
    }
    #endregion
}

