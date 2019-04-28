

/* 
    LaunchModule 模块用于在启动程序之前从各种配置文件中读取程序所需的配置信息
*/


namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LaunchModule : IModule
    {
        #region 抽象方法实现
        public override void Init()
        {
            //读取 AppInfo， 屏幕信息，分屏信息
            SingletonMgr.GameGlobalInfo.AppInfo = AppInfo.DeserializeAppInfoFromXml();

            //读取 U3D客户端信息(包括ClientId, ClientName, ServerIPAddress, ServerPort)
            SingletonMgr.GameGlobalInfo.PlayerInfo = PlayerInfo.DeserializePlayerInfoFromXml();

            //读取 Station站台信息(包括站台客户端Id, Name, Index)
            SingletonMgr.GameGlobalInfo.StationInfoList = StationInfoList.DeserializeStationInfoListFromXml();

            //读取 Station摄像头信息
            Dictionary<System.UInt16, CameraInfoList> m_cameraInfoListDict = new Dictionary<ushort, CameraInfoList>();
            //赵营
            CameraInfoList zhaoYingCameraInfoList = CameraInfoList.DeserializeCameraInfoFromXml(AppConfigPath.ZhaoYingCameraInfoXmlPath);
            m_cameraInfoListDict.Add(5, zhaoYingCameraInfoList);
            SingletonMgr.GameGlobalInfo.CameraInfoListDict = m_cameraInfoListDict;

            //读取列车信息
            SingletonMgr.GameGlobalInfo.AllTrainLineInfo = AllTrainLineInfo.DeserializeTrainLineInfoFromXml();
        }
        public override void Release()
        {
            
        }
        #endregion
    }
}



