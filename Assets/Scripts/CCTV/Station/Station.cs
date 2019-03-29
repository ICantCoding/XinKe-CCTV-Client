using System;
using System.Collections;
using System.Collections.Generic;

public enum StationType : System.UInt16
{
    ZhaoYing,				//赵营
}

public class Station
{
    #region 设备相关
    #region 字段
    //用于管理不同类型设备
    private Dictionary<DeviceType, DeviceMgr> m_deviceMgrDict = new Dictionary<DeviceType, DeviceMgr>();
    #endregion

    #region 属性
    public Dictionary<DeviceType, DeviceMgr> DeviceMgrDict
    {
        get { return m_deviceMgrDict; }
    }
    #endregion

    #region 方法 
    public void AddDeviceMgr(DeviceMgr mgr)
    {
        if (mgr == null) return;
        if (m_deviceMgrDict.ContainsKey(mgr.DeviceType) == false)
        {
            m_deviceMgrDict.Add(mgr.DeviceType, mgr);
        }
    }
    public DeviceMgr GetDeviceMgr(DeviceType deviceType)
    {
        DeviceMgr mgr = null;
        m_deviceMgrDict.TryGetValue(deviceType, out mgr);
        return mgr;
    }
    public Device GetDevice(DeviceType deviceType, int deviceId)
    {
        DeviceMgr mgr = GetDeviceMgr(deviceType);
        if (mgr == null) return null;
        return mgr.GetDevice(deviceId);
    }
    #endregion
    #endregion

    #region Npc相关

    #region 字段
    private Dictionary<NpcActionStatus, NpcMgr> m_npcMgrDict = new Dictionary<NpcActionStatus, NpcMgr>();
    #endregion

    #region 方法
    public void AddNpcMgr(NpcMgr npcMgr)
    {
        if (npcMgr == null) return;
        if (m_npcMgrDict.ContainsKey(npcMgr.NpcActionStatus) == false)
        {
            m_npcMgrDict.Add(npcMgr.NpcActionStatus, npcMgr);
        }
    }
    public NpcMgr GetNpcMgr(NpcActionStatus npcActionStatus)
    {
        NpcMgr npcMgr = null;
        m_npcMgrDict.TryGetValue(npcActionStatus, out npcMgr);
        return npcMgr;
    }
    public void AddNpcAction(NpcActionStatus npcActionStatus, NpcAction npcAction)
    {
        NpcMgr npcMgr = GetNpcMgr(npcActionStatus);
        if (npcMgr == null) return;
        npcMgr.AddNpcAction(npcAction);
    }
    public void RemoveNpcAction(NpcActionStatus npcActionStatus, int npcId)
    {
        NpcMgr npcMgr = GetNpcMgr(npcActionStatus);
        if (npcMgr == null) return;
        npcMgr.RemoveNpcAction(npcId);
    }
    public NpcAction GetNpcAction(NpcActionStatus npcActionStatus, int npcId)
    {
        NpcMgr npcMgr = GetNpcMgr(npcActionStatus);
        if (npcMgr == null) return null;
        return npcMgr.GetNpcAction(npcId);
    }
    #endregion

    #endregion
}
