
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourceEngine : MonoBehaviour
    {
        #region Unity生命周期
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion

        #region 方法
        public void InitResourceEngine(Transform poolGos, Transform sceneGos)
        {
            ResourceMgr.Instance.Init(this); //开启异步加载的协程
            ObjectManager.Instance.InitGoPool(poolGos, sceneGos); //初始化对象池数据
        }
        #endregion
    }
}