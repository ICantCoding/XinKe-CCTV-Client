
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
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.NetworkModuleName);
        }
		void OnDestroy()
		{
			SingletonMgr.ModuleMgr.RemoveModule(StringMgr.LaunchModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.NetworkModuleName);
		}
        void OnApplicationQuit()
        {
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.LaunchModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.NetworkModuleName);
        }
        #endregion
    }
}

