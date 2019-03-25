
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AppStart : MonoBehaviour
    {
        #region Unity生命周期
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
			//初始化LaunchModule, 加载应用所需的配置数据
			SingletonMgr.ModuleMgr.RegisterModule(StringMgr.LaunchModuleName);
            //加载NetworkModule模块，用于启动网络客户端，与服务端连接通信
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.NetworkModuleName);
            //加载ResourcesModule模块，用于加载AssetBundle打包的各种资源和对象
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.ResourcesModuleName);
        }
		void OnDestroy()
		{
			SingletonMgr.ModuleMgr.RemoveModule(StringMgr.LaunchModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.NetworkModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.ResourcesModuleName);
		}
        void OnApplicationQuit()
        {
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.LaunchModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.NetworkModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.ResourcesModuleName);
        }
        #endregion
    }
}

