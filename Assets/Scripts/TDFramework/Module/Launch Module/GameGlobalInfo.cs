using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;


/*
GameGlobalInfo 用来保存全局配置文件的对象
*/
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
        set { m_stationInfoList = value; }
    }
    #endregion

    #region 应用程序信息
    private AppInfo m_appInfo = null;
    public AppInfo AppInfo
    {
        get { return m_appInfo; }
        set { m_appInfo = value; }
    }
    #endregion

    #region 站台摄像头数据信息
    private Dictionary<System.UInt16, CameraInfoList> m_cameraInfoListDict = null;
    public Dictionary<System.UInt16, CameraInfoList> CameraInfoListDict
    {
        get { return m_cameraInfoListDict; }
        set { m_cameraInfoListDict = value; }
    }
    #endregion

    #region 列车数据信息
    private AllTrainLineInfo m_allTrainLineInfo = null;
    public AllTrainLineInfo AllTrainLineInfo
    {
        get { return m_allTrainLineInfo; }
        set { m_allTrainLineInfo = value; }
    }
    #endregion

    #region 其他信息

    #endregion
}



