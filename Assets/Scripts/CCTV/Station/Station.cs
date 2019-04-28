

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType : System.UInt16
{
    ChiDong,				//赵营
    QianDaYan,              //前大彦
    YuanBoYuan,             //园博园
    DaXueCheng,             //大学城
    ZiWeiLu,                //紫薇路
    ZhaoYing,               //赵营
    YuFuHe,                 //玉符河
    WangFuZhuang,           //王府庄
    DaYangZhuang,           //大杨庄
    JiNanXi,                //济南西
    YanMaZhuangXi,          //演马庄西
}

public class Station
{
    #region 构造函数
    public Station()
    {
        AddNpcMgr(new NpcMgr(NpcActionStatus.EnterStationTrainUp_NpcActionStatus));
        AddNpcMgr(new NpcMgr(NpcActionStatus.EnterStationTrainDown_NpcActionStatus));
        AddNpcMgr(new NpcMgr(NpcActionStatus.ExitStationTrainUp_NpcActionStatus));
        AddNpcMgr(new NpcMgr(NpcActionStatus.ExitStationTrainDown_NpcActionStatus));
    }
    #endregion


    #region 设备相关
    #region 字段
    private Dictionary<DeviceType, DeviceMgr> m_deviceMgrDict = new Dictionary<DeviceType, DeviceMgr>(); //用于管理不同类型设备    
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
    public List<Device> GetPingBiMenList(PingBiMenType pingBiMenType)
    {
        PingBiMenMgr mgr = (PingBiMenMgr)GetDeviceMgr(DeviceType.PingBiMen);
        if (mgr == null) return null;
        return pingBiMenType == PingBiMenType.Up ? mgr.ShangXingPingBiMenList : mgr.XiaXingPingBiMenList;
    }
    #endregion
    #endregion

    #region Npc相关
    #region 字段
    private Dictionary<NpcActionStatus, NpcMgr> m_npcMgrDict = new Dictionary<NpcActionStatus, NpcMgr>();
    #endregion

    #region 属性
    public Dictionary<NpcActionStatus, NpcMgr> NpcMgrDict
    {
        get { return m_npcMgrDict; }
    }
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

    #region 列车相关
    #region 字段
    private Train m_upTrain;    //站台对应的上行列车
    private Train m_downTrain;  //站台对应的下行列车
    #endregion

    #region 属性
    public Train UpTrain
    {
        get { return m_upTrain; }
        set
        {
            m_upTrain = value;
            m_upTrain.TrainType = TrainType.Up;
        }
    }
    public Train DownTrain
    {
        get { return m_downTrain; }
        set
        {
            m_downTrain = value;
            m_downTrain.TrainType = TrainType.Down;
        }
    }
    #endregion
    
    #region 方法
    public void OpenUpTrainLeftDoors()
    {
        UpTrain.OpenLeftDoors();
    }
    public void CloseUpTrainLeftDoors()
    {
        UpTrain.CloseLeftDoors();
    }
    public void OpenUpTrainRightDoors()
    {
        UpTrain.OpenRightDoors();
    }
    public void CloseUpTrainRightDoors()
    {
        UpTrain.CloseRightDoors();
    }
    public void OpenDownTrainLeftDoors()
    {
        DownTrain.OpenLeftDoors();
    }
    public void CloseDownTrainLeftDoors()
    {
        DownTrain.CloseLeftDoors();
    }
    public void OpenDownTrainRightDoors()
    {
        DownTrain.OpenRightDoors();
    }
    public void CloseDownTrainRightDoors()
    {
        DownTrain.CloseRightDoors();
    }
    #endregion
    #endregion

    #region 摄像头Camera相关
    #region 字段
    private Dictionary<UInt16, Camera> m_cameraDict = new Dictionary<UInt16, Camera>();
    #endregion

    #region 属性
    public Dictionary<UInt16, Camera> CameraDict
    {
        get{return m_cameraDict;}
    }
    #endregion

    #region 方法
    public void AddCamera2CameraDict(UInt16 cameraIndex, Camera pCamera)
    {
        if(CameraDict.ContainsKey(cameraIndex) == false)
        {
            CameraDict.Add(cameraIndex, pCamera);
        }
    }
    public void RemoveCamera4CameraDict(UInt16 cameraIndex)
    {
        CameraDict.Remove(cameraIndex);
    }
    public Camera GetCameraByCameraIndex(UInt16 cameraIndex)
    {
        Camera tempCamera = null;
        CameraDict.TryGetValue(cameraIndex, out tempCamera);
        return tempCamera;
    }
    #endregion
    #endregion
}
