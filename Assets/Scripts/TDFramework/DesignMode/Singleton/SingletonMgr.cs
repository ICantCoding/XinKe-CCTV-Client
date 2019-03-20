﻿
namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SingletonMgr
    {
        public static GameGlobalInfo GameGlobalInfo = GameGlobalInfo.Instance;
        public static ModuleMgr ModuleMgr = ModuleMgr.Instance;
        public static SceneInfoMgr SceneInfoMgr = SceneInfoMgr.Instance;
        public static LoadSceneMgr LoadSceneMgr = LoadSceneMgr.Instance;
    }
}
