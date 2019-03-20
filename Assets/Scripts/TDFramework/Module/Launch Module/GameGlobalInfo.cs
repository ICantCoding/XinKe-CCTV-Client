using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class GameGlobalInfo : Singleton<GameGlobalInfo>
{
    #region 客户端信息	
    private PlayerInfo m_playerInfo = null;
    public PlayerInfo PlayerInfo
    {
        get { return m_playerInfo; }
        set
        {
            m_playerInfo = value;
        }
    }
    #endregion

    #region 站台信息
    private StationInfoList m_stationInfoList = null;
    public StationInfoList StationInfoList
    {
        get { return m_stationInfoList; }
        set
        {
            m_stationInfoList = value;
        }
    }
    #endregion


    #region 其他信息

    #endregion
}



