
namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NetworkModule : IModule
    {
        #region 字段
        private NetworkEngine m_networkEngine = null;
        #endregion

        #region 抽象方法实现
        public override void Init()
        {
            GameObject networkEngineGo = GameObject.Find(StringMgr.NetworkEngineName);
            m_networkEngine = networkEngineGo.AddComponent<NetworkEngine>();
            m_networkEngine.Run();
        }
        public override void Release()
        {
            if (m_networkEngine != null)
            {
                m_networkEngine.Stop();
            }
        }
        #endregion

        #region 公有方法
        //根据站台索引，获取站台网络引擎
        public StationNetworkEngine GetStationNetworkEngineByStationIndex(System.UInt16 stationIndex, System.UInt16 stationClientType)
        {
            return m_networkEngine.GetStationNetworkEngineByStationIndex(stationIndex, stationClientType);
        }
        //各站台网络引擎开启, 用不到该方法， 也不要使用该方法
        public void AllStationNetworkEngineLink()
        {
            m_networkEngine.AllStationNetworkEngineLink();
        }
        #endregion
    }
}
