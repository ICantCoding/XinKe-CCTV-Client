using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationMgr : MonoBehaviour
{
    #region 字段
    public GameObject npcPrefab;
    public Dictionary<int, Station> m_stationDict = new Dictionary<int, Station>();
    #endregion    

    #region Unity生命周期
    void Awake()
    {
        int childCount = transform.childCount;
        Station station = null;
        for(int i = 0; i < childCount; ++i)
        {
            station = new Station();
            station.AddNpcMgr(new NpcMgr(NpcActionStatus.EnterStationTrainUp_NpcActionStatus));
            station.AddNpcMgr(new NpcMgr(NpcActionStatus.EnterStationTrainDown_NpcActionStatus));
            station.AddNpcMgr(new NpcMgr(NpcActionStatus.ExitStationTrainUp_NpcActionStatus));
            station.AddNpcMgr(new NpcMgr(NpcActionStatus.ExitStationTrainDown_NpcActionStatus));
            AddStation2Mgr(i, station);
        }
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
    public void AddStation2Mgr(int stationIndex, Station station)
    {
        if (station == null) return;
        if (m_stationDict.ContainsKey(stationIndex) == false)
        {
            m_stationDict.Add(stationIndex, station);
        }
    }
    public void AddNpcAction(int stationIndex, NpcActionStatus npcActionStatus, NpcAction npcAction)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return;
        station.AddNpcAction(npcActionStatus, npcAction);
    }
    public void RemoveNpcAction(int stationIndex, NpcActionStatus npcActionStatus, int npcId)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return;
        station.RemoveNpcAction(npcActionStatus, npcId);
    }
    public NpcAction GetNpcAction(int stationIndex, NpcActionStatus npcActionStatus, int npcId)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return null;
        return station.GetNpcAction(npcActionStatus, npcId);    
    }
    #endregion
}
