using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*======================================================================================================

StationMgr管理着所有的Station， 而Station管理着站台内的所有东西(Npc, Device, Train)
Station中的方法会暴露给StationMgr， StationMgr充当中介者的效果

========================================================================================================*/

public class StationMgr : MonoBehaviour
{
    #region 字段
    public Dictionary<int, Station> m_stationDict = new Dictionary<int, Station>();
    private PlayerNetworkEngine m_playerNetworkEngine;
    #endregion    


    #region Unity生命周期
    void Awake()
    {
        int childCount = transform.childCount;
        Station station = null;
        string stationName = "";
        UInt16 stationIndex = 0;
        for (int i = 0; i < childCount; ++i)
        {
            stationName = transform.GetChild(i).name;
            stationIndex = (UInt16)System.Enum.Parse(typeof(StationType), stationName);
            station = new Station();
            AddStation2Dict(stationIndex, station);
        }
        //读取所有站台列车数据并管理
        ReadStationPoint.BuildStationTrains(this);
        //读取所有站台设备数据并管理
        ReadStationPoint.BuildStationDevices(this);
        //获取PlayerNetworkEngine
        m_playerNetworkEngine = GameObject.Find("NetworkEngine/Player").GetComponent<PlayerNetworkEngine>();
    }
    void Start()
    {
        //这里，请求一次站台屏蔽门的状态信息，用于重连机制
        m_playerNetworkEngine.SendU3DClientReconnectRequest();
    }
    #endregion


    #region 属性
    public int Count
    {
        get { return m_stationDict.Count; }
    }
    #endregion


    #region 站台管理相关
    public Station GetStation(int stationIndex)
    {
        Station station = null;
        m_stationDict.TryGetValue(stationIndex, out station);
        return station;
    }
    public void AddStation2Dict(int stationIndex, Station station)
    {
        if (station == null) return;
        if (m_stationDict.ContainsKey(stationIndex) == false)
        {
            m_stationDict.Add(stationIndex, station);
        }
    }
    #endregion

    #region Npc相关
    public void AddNpcAction(int stationIndex, NpcActionStatus npcActionStatus, NpcAction npcAction)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.AddNpcAction(npcActionStatus, npcAction);
    }
    public void RemoveNpcAction(NpcAction npcAction)
    {
        if(npcAction == null) return;
        RemoveNpcAction(npcAction.StationIndex, (NpcActionStatus)npcAction.NpcActionStatus, npcAction.NpcId);
    }
    public void RemoveNpcAction(int stationIndex, NpcActionStatus npcActionStatus, int npcId)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.RemoveNpcAction(npcActionStatus, npcId);
    }
    public NpcAction GetNpcAction(int stationIndex, NpcActionStatus npcActionStatus, int npcId)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetNpcAction(npcActionStatus, npcId);
    }
    #endregion


    #region 设备相关
    public Device GetDevice(int stationIndex, DeviceType deviceType, int deviceId)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetDevice(deviceType, deviceId);
    }
    public List<Device> GetPingBiMenList(int stationIndex, PingBiMenType pingBiMenType)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetPingBiMenList(pingBiMenType);
    }
    #endregion


    #region 列车相关
    //获取某个站台的列车
    public Train GetTrain(int stationIndex, int upOrDownFlag)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        if (upOrDownFlag == 0)
        {
            return station.UpTrain;
        }
        else if (upOrDownFlag == 1)
        {
            return station.DownTrain;
        }
        return null;
    }
    public void OpenUpTrainLeftDoors(int stationIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.OpenUpTrainLeftDoors();
    }
    public void CloseUpTrainLeftDoors(int stationIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.CloseUpTrainLeftDoors();
    }
    public void OpenUpTrainRightDoors(int stationIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.OpenUpTrainRightDoors();
    }
    public void CloseUpTrainRightDoors(int stationIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.CloseUpTrainRightDoors();
    }
    public void OpenDownTrainLeftDoors(int stationIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.OpenDownTrainLeftDoors();
    }
    public void CloseDownTrainLeftDoors(int stationIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.CloseDownTrainLeftDoors();
    }
    public void OpenDownTrainRightDoors(int stationIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.OpenDownTrainRightDoors();
    }
    public void CloseDownTrainRightDoors(int stationIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.CloseDownTrainRightDoors();
    }
    #endregion

    #region 摄像机Camera相关
    public void AddCamera2CameraDict(UInt16 stationIndex, UInt16 cameraIndex, Camera pCamera)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.AddCamera2CameraDict(cameraIndex, pCamera);
    }
    public void RemoveCamera4CameraDict(UInt16 stationIndex, UInt16 cameraIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.RemoveCamera4CameraDict(cameraIndex);
    }
    public Camera GetCameraByCameraIndex(UInt16 stationIndex, UInt16 cameraIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetCameraByCameraIndex(cameraIndex);
    }
    #endregion
}
