
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AppStart : MonoBehaviour
    {
        #region Unity生命周期
        void Start()
        {
			//初始化LaunchModule, 加载应用所需的配置数据
			SingletonMgr.ModuleMgr.RegisterModule("LaunchModule");
			//初始化NetworkModule, 让应用连接到服务器
			SingletonMgr.ModuleMgr.RegisterModule("NetworkModule");
        }
		void OnDestroy()
		{
			SingletonMgr.ModuleMgr.RemoveModule("LaunchModule");
            SingletonMgr.ModuleMgr.RemoveModule("NetworkModule");
		}
        void OnApplicationQuit()
        {
            SingletonMgr.ModuleMgr.RemoveModule("LaunchModule");
            SingletonMgr.ModuleMgr.RemoveModule("NetworkModule");
        }
        #endregion
    }
}

