
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
            GameObject networkEngineGo = GameObject.Find("NetworkEngine");
            m_networkEngine = networkEngineGo.GetComponent<NetworkEngine>();
            if(m_networkEngine != null)
            {
                m_networkEngine.DynamicAddNetwoekEngine2ChildGameObject();
            }
        }
        public override void Release()
        {
            if(m_networkEngine != null)
            {
                m_networkEngine.Stop();
            }
        }
        #endregion
    }
}
