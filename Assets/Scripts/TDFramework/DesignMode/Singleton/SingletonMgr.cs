
namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SingletonMgr
    {
        //应用内所有配置相关的数据管理单例类
        public static GameGlobalInfo GameGlobalInfo = GameGlobalInfo.Instance;
        //网络消息映射处理类的管理单例类
        public static NetworkMsgHandleFuncMap NetworkMsgHandleFuncMap = NetworkMsgHandleFuncMap.Instance;
        //应用内管理所有模块的管理单例类
        public static ModuleMgr ModuleMgr = ModuleMgr.Instance;
        //应用内场景跳转的场景信息管理单例类
        public static SceneInfoMgr SceneInfoMgr = SceneInfoMgr.Instance;
        public static LoadSceneMgr LoadSceneMgr = LoadSceneMgr.Instance;
    }
}
