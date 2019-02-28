using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


class MessageReader
{

    enum READ_STATE
    {
        READ_STATE_SENDID = 0,          //发送者ID   2个字节
        READ_STATE_NODEID = 1,          //接收者ID   2个字节
        READ_STATE_FIRSTID = 2,           //FirstID    2个字节
        READ_STATE_SECONDID = 3,          //SecondID   2个字节
        READ_STATE_SIZE = 4,            //Size大小   2个字节
        READ_STATE_BODY = 5,            //数据报文实际真实传输内容，二进制数据
    }
    public delegate void MessageHandler(Packet msg);

    #region 字段
    private System.UInt16 m_sendId;
    private System.UInt16 m_nodeId;
    private System.UInt16 m_firstId;
    private System.UInt16 m_secondId;
    private System.UInt16 m_msgLen;

    private READ_STATE m_state = READ_STATE.READ_STATE_SENDID;
    private System.UInt32 m_expectSize = 2;

    private MemoryStream m_stream = new MemoryStream();
    public MessageHandler m_messageHandler = null;
    #endregion

    #region 构造函数
    public MessageReader()
    {
        m_expectSize = 2;
        m_state = READ_STATE.READ_STATE_SENDID;
    }
    public MessageReader(MessageHandler handler) : this()
    {
        m_messageHandler = handler;
    }
    #endregion

    #region 方法
    public void Process(byte[] datas, System.UInt32 length)
    {
        System.UInt32 totallength = 0;
        while (length > 0 && m_expectSize > 0)
        {
            if (m_state == READ_STATE.READ_STATE_SENDID) //读取SendID, 消息发送者ID
            {
                if (length >= m_expectSize)
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, m_expectSize);
                    totallength += m_expectSize;
                    m_stream.wpos += m_expectSize;
                    length -= m_expectSize;

                    m_sendId = m_stream.ReadUInt16();
                    m_stream.Clear();

                    m_state = READ_STATE.READ_STATE_NODEID;
                    m_expectSize = 2;
                }
                else
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, length);
                    m_stream.wpos += length;
                    m_expectSize -= length;
                    break;
                }
            }
            else if (m_state == READ_STATE.READ_STATE_NODEID) //读取NodeID, 消息接收者ID
            {
                if (length >= m_expectSize)
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, m_expectSize);
                    totallength += m_expectSize;
                    m_stream.wpos += m_expectSize;
                    length -= m_expectSize;

                    m_nodeId = m_stream.ReadUInt16();
                    m_stream.Clear();

                    m_state = READ_STATE.READ_STATE_FIRSTID;
                    m_expectSize = 2;
                }
                else
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, length);
                    m_stream.wpos += length;
                    m_expectSize -= length;
                    break;
                }
            }
            else if (m_state == READ_STATE.READ_STATE_FIRSTID) //读取FirstID
            {
                if (length >= m_expectSize)
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, m_expectSize);
                    totallength += m_expectSize;
                    m_stream.wpos += m_expectSize;
                    length -= m_expectSize;

                    m_firstId = m_stream.ReadUInt16();
                    m_stream.Clear();

                    m_state = READ_STATE.READ_STATE_SECONDID;
                    m_expectSize = 2;
                }
                else
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, length);
                    m_stream.wpos += length;
                    m_expectSize -= length;
                    break;
                }
            }
            else if (m_state == READ_STATE.READ_STATE_SECONDID) //读取SecondID
            {
                if (length >= m_expectSize)
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, m_expectSize);
                    totallength += m_expectSize;
                    m_stream.wpos += m_expectSize;
                    length -= m_expectSize;

                    m_secondId = m_stream.ReadUInt16();
                    m_stream.Clear();

                    m_state = READ_STATE.READ_STATE_SIZE;
                    m_expectSize = 2; 
                }
                else
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, length);
                    m_stream.wpos += length;
                    m_expectSize -= length;
                    break;
                }
            }
            else if(m_state == READ_STATE.READ_STATE_SIZE) //读取Size大小
            {
                if (length >= m_expectSize)
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, m_expectSize);
                    totallength += m_expectSize;
                    m_stream.wpos += m_expectSize;
                    length -= m_expectSize;

                    m_msgLen = m_stream.ReadUInt16();
                    m_stream.Clear();

                    m_state = READ_STATE.READ_STATE_BODY;
                    m_expectSize = m_msgLen; //msglen大小表示后边数据部分的大小
                }
                else
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, length);
                    m_stream.wpos += length;
                    m_expectSize -= length;
                    break;
                }
            }

            if (m_state == READ_STATE.READ_STATE_BODY) //读取数据包真实数据部分
            {
                if (length >= m_expectSize)
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, m_expectSize);
                    totallength += m_expectSize;
                    m_stream.wpos += m_expectSize;
                    length -= m_expectSize;

                    string content = Encoding.UTF8.GetString(m_stream.Data, 0, (int)m_stream.wpos);
                    byte[] bytes = new byte[m_stream.wpos];
                    Array.Copy(m_stream.Data, 0, bytes, 0, bytes.Length);
                    Debug.Log("服务器接收的数据并解析: " + content);
                    Packet p = new Packet(m_sendId, m_nodeId, m_firstId, m_secondId, m_msgLen, bytes);
                    if (m_messageHandler != null)
                    {
                        m_messageHandler(p);
                    }
                    m_stream.Clear();
                    m_state = READ_STATE.READ_STATE_SENDID;
                    m_expectSize = 2;
                }
                else
                {
                    Array.Copy(datas, totallength, m_stream.Data, m_stream.wpos, length);
                    m_stream.wpos += length;
                    m_expectSize -= length;
                    break;
                }
            }
        }
    }
    #endregion
}

