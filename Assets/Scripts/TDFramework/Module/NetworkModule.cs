
namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class NetworkModule : IModule
    {
        #region 字段
        private NetworkEngine m_networkEngine = null;
        #endregion

        #region 抽象方法实现
        public override void Init()
        {
            m_networkEngine = SingletonMgr.NetworkEngine;
            m_networkEngine.Run(SingletonMgr.GameGlobalInfo.PlayerInfo.ServerIpAddress, SingletonMgr.GameGlobalInfo.PlayerInfo.ServerPort);
        }
        public override void Release()
        {
            //关闭客户端连接服务器
            if (m_networkEngine != null)
            {
                m_networkEngine.Stop();
                m_networkEngine = null;
            }
        }
        #endregion
    }
}
