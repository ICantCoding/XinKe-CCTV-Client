
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
        }
        public override void Release()
        {
            Stop();
        }
        #endregion

        #region 方法
        public void Run(RemoteClientConnectServerSuccessCallback successCallback,
            RemoteClientConnectServerFailCallback failCallback)
        {
            m_networkEngine.Run(SingletonMgr.GameGlobalInfo.PlayerInfo.ServerIpAddress,
                SingletonMgr.GameGlobalInfo.PlayerInfo.ServerPort, successCallback, failCallback);
        }
        public void Run(string serverIp, int serverPort,
            RemoteClientConnectServerSuccessCallback successCallback, RemoteClientConnectServerFailCallback failCallback)
        {
            m_networkEngine.Run(serverIp, serverPort, successCallback, failCallback);
        }
        public void Stop()
        {
            if (m_networkEngine != null)
            {
                m_networkEngine.Stop();
                m_networkEngine = null;
            }
        }
        #endregion

        #region Send网络消息方法
        //客户端登录请求
        public void SendU3DClientLoginInfoRequest()
        {
            if(m_networkEngine != null)
            {
                m_networkEngine.SendU3DClientLoginInfoRequest();
            }
        }
        #endregion

    }
}
