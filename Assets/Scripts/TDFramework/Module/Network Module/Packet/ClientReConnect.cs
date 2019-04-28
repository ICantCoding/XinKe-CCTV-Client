using System;
using System.Collections;
using System.Collections.Generic;

//客户端重连数据包
// sendId=U3DId, nodeId=0(0表示服务器Id), firstId=0, secondId=6, msglen=ClientReConnect.Size, data=ClientReConnect的字节
public class ClientReConnect : IPacket
{
    #region 字段和属性

    //数据占用字节大小
    public UInt16 Size
    {
        get { return 0; }
    }
    #endregion

    #region 构造函数
    public ClientReConnect()
    {

    }
    public ClientReConnect(byte[] bytes)
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
