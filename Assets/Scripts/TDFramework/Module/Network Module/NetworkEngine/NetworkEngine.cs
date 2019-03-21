using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class NetworkEngine : MonoBehaviour
{

    #region 字段
    private Transform m_playerTrans;
    private List<BaseNetworkEngine> m_networkEngineList = new List<BaseNetworkEngine>();
    #endregion


    #region Unity生命周期
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    #region 公有方法
    public void DynamicAddNetwoekEngine2ChildGameObject()
    {
        AddPlayerNetworkEngine();
        AddStationNetworkEngine();
    }
    public void Stop()
    {
        int count = m_networkEngineList.Count;
        for(int i = 0; i < count; ++i)
        {
            m_networkEngineList[i].Stop();
        }
    }
    #endregion

    #region 私有方法
    private void AddPlayerNetworkEngine()
    {
        m_playerTrans = transform.Find("Player");
        if (m_playerTrans != null)
        {
            m_networkEngineList.Add(m_playerTrans.gameObject.AddComponent<PlayerNetworkEngine>());
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
                m_networkEngineList.Add(engine);
                switch (engineName)
                {
                    case "ShangXingShangChe":
                    {
                        engine.StationClientType = 1;
                        engine.StationIndex = stationInfo.Index;
                        break;
                    }
                    case "ShangXingXiaChe":
                    {
                        engine.StationClientType = 2;
                        engine.StationIndex = stationInfo.Index;
                        break;
                    }
                    case "XiaXingShangChe":
                    {
                        engine.StationClientType = 3;
                        engine.StationIndex = stationInfo.Index;
                        break;
                    }
                    case "XiaXingXiaChe":
                    {
                        engine.StationClientType = 4;
                        engine.StationIndex = stationInfo.Index;
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
        }
    }
    #endregion	
}
