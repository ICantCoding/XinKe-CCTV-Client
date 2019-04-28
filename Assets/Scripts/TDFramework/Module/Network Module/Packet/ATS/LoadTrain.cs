using System;
using System.Collections;
using System.Collections.Generic;

public class LoadTrain : IPacket
{
    #region 字段和属性
    //数据占用字节大小
    public UInt16 Size
    {
        get { return 0; }
    }
    #endregion

    #region 构造函数
    public LoadTrain()
    {

    }
    public LoadTrain(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] bytes = new byte[Size];
        return bytes;
    }
    #endregion
}
