using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class NetworkEngine : MonoBehaviour
{
    #region 字段
    private Transform m_playerTrans;
    //客户端网络引擎
    private PlayerNetworkEngine m_playerNetworkEngine;
    //存放站台网络引擎的List
    private List<StationNetworkEngine> m_stationNetworkEngineList = new List<StationNetworkEngine>();
    #endregion

    #region Unity生命周期
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    #region 公有方法
    public void Run()
    {
        AddPlayerNetworkEngine();
        AddStationNetworkEngine();
    }
    public void Stop()
    {
        if (m_playerNetworkEngine != null)
            m_playerNetworkEngine.Stop();
        int count = m_stationNetworkEngineList.Count;
        for (int i = 0; i < count; ++i)
        {
            if (m_stationNetworkEngineList[i] != null)
                m_stationNetworkEngineList[i].Stop();
        }
    }
    //根据站台索引，获取站台网络引擎
    public StationNetworkEngine GetStationNetworkEngineByStationIndex(System.UInt16 stationIndex, System.UInt16 stationClientType)
    {
        int count = m_stationNetworkEngineList.Count;
        StationNetworkEngine stationNetworkEngine = null;
        for (int i = 0; i < count; i++)
        {
            stationNetworkEngine = m_stationNetworkEngineList[i];
            if (stationNetworkEngine.StationIndex == stationIndex && stationNetworkEngine.StationClientType == stationClientType)
            {
                return (StationNetworkEngine)stationNetworkEngine;
            }
        }
        return null;
    }
    //各站台网络引擎开启, 用不到该方法
    public void AllStationNetworkEngineLink()
    {
        int count = m_stationNetworkEngineList.Count;
        for (int i = 0; i < count; ++i)
        {
            if (m_stationNetworkEngineList[i] != null)
                m_stationNetworkEngineList[i].Link();
        }
    }
    #endregion

    #region 私有方法
    private void AddPlayerNetworkEngine()
    {
        m_playerTrans = transform.Find("Player");
        if (m_playerTrans != null)
        {
            m_playerNetworkEngine = m_playerTrans.gameObject.AddComponent<PlayerNetworkEngine>();
        }
    }
    private void AddStationNetworkEngine()
    {
        int count = transform.childCount;
        for (int i = 1; i < count; ++i)
        {
            Transform stationTrans = transform.GetChild(i);
            string stationName = stationTrans.gameObject.name;
            StationInfo stationInfo = SingletonMgr.GameGlobalInfo.StationInfoList.GetStationInfo(stationName);
            for (int j = 0; j < stationTrans.childCount; ++j)
            {
                Transform item = stationTrans.GetChild(j);
                string engineName = item.gameObject.name;
                StationNetworkEngine engine = item.gameObject.AddComponent<StationNetworkEngine>();
                m_stationNetworkEngineList.Add(engine);
                switch (engineName)
                {
                    case StringMgr.ShangXingShangChe:
                        {
                            engine.StationClientType = IntMgr.ShangXingShangChe;
                            engine.StationIndex = stationInfo.Index;
                            break;
                        }
                    case StringMgr.ShangXingXiaChe:
                        {
                            engine.StationClientType = IntMgr.ShangXingXiaChe;
                            engine.StationIndex = stationInfo.Index;
                            break;
                        }
                    case StringMgr.XiaXingShangChe:
                        {
                            engine.StationClientType = IntMgr.XiaXingShangChe;
                            engine.StationIndex = stationInfo.Index;
                            break;
                        }
                    case StringMgr.XiaXingXiaChe:
                        {
                            engine.StationClientType = IntMgr.XiaXingXiaChe;
                            engine.StationIndex = stationInfo.Index;
                            break;
                        }
                }
            }
        }
    }
    #endregion	
}
