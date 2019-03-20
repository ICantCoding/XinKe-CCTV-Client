

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //LaunchModule 模块用于在启动程序之前从各种配置文件中读取程序所需的配置信息
    public class LaunchModule : IModule
    {
        #region 抽象方法实现
        public override void Init()
        {
            //读取 U3D客户端信息(包括ClientId, ClientName, ServerIPAddress, ServerPort)
            SingletonMgr.GameGlobalInfo.PlayerInfo = PlayerInfo.DeserializePlayerInfoFromXml();
            //读取 Station站台信息(包括站台客户端Id, Name, Index)
            SingletonMgr.GameGlobalInfo.StationInfoList = StationInfoList.DeserializeStationInfoListFromXml();
        }
        public override void Release()
        {

        }
        #endregion
    }
}

