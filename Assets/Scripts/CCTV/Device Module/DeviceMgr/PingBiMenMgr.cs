using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingBiMenMgr : DeviceMgr
{
    #region 字段
    //上行屏蔽门列表
    private List<Device> m_shangXingPingBiMenList;
    //下行屏蔽门列表
    private List<Device> m_xiaXingPingBiMenList;
    #endregion

    #region 属性
    public List<Device> ShangXingPingBiMenList
    {
        get { return m_shangXingPingBiMenList; }
    }
    public List<Device> XiaXingPingBiMenList
    {
        get { return m_xiaXingPingBiMenList; }
    }
    #endregion

    #region 构造函数
    public PingBiMenMgr() : base()
    {
        m_shangXingPingBiMenList = new List<Device>();
        m_xiaXingPingBiMenList = new List<Device>();
    }
    #endregion

    #region 方法
    public void AddDevice2ShangXingPingBiMenList(Device device)
    {
        if (device == null) return;
        m_shangXingPingBiMenList.Add(device);
    }
    public void AddDevice2XiaXingPingBiMenList(Device device)
    {
        if (device == null) return;
        m_xiaXingPingBiMenList.Add(device);
    }
    #endregion
}
