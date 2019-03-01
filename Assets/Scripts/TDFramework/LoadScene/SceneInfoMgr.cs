
//////////////////////////////////////////////////////////////////
//                            _ooOoo_                           //
//                           o8888888o                          //
//                           88" . "88                          //
//                           (| -_- |)                          //
//                           O\  =  /O                          //    
//                        ____/`---'\____                       //
//                      .'  \\|     |//  `.                     //
//                     /  \\|||  :  |||//  \                    //
//                    /  _||||| -:- |||||-  \                   //
//                    |   | \\\  -  /// |   |                   //
//                    | \_|  ''\---/''  |   |                   //
//                    \  .-\__  `-`  ___/-. /                   //
//                  ___`. .'  /--.--\  `. . __                  //
//               ."" '<  `.___\_<|>_/___.'  >'"".               //
//              | | :  `- \`.;`\ _ /`;.`/ - ` : | |             //
//              \  \ `-.   \_ __\ /__ _/   .-` /  /             //
//         ======`-.____`-.___\_____/___.-`____.-'======        //
//                            `=---='                           //
//        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^         //
//           佛祖保佑 程序员一生平安,健康,快乐,没有Bug!            //
//////////////////////////////////////////////////////////////////

// ***************************************************************
// Copyright (C) 2017 The company name
//
// 文件名(File Name):             SceneInfoTable.cs
// 作者(Author):                  田山杉
// 创建时间(CreateTime):          2019-01-27 10:38:38
// 修改者列表(modifier):
// 模块描述(Module description):  用来管理缓存每个场景信息名字, 索引等信息
// ***************************************************************

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //表示一个Scene的基本信息, 我这里的思想还不错
    public class SceneInfo
    {
        public int Index;
        public string Name;
    }

    public class SceneInfoMgr : Singleton<SceneInfoMgr>
    {
        #region 常量
        private const string Loading = "Loading";
        private const string LaunchSplash = "LaunchSplash";
        private const string AppStart = "AppStart";
        #endregion

        #region 集合管理
        public Dictionary<string, SceneInfo> SceneInfoDict = new Dictionary<string, SceneInfo>();
        #endregion

        #region 属性
        //从LoadingScene场景即将跳转到的目标场景的SceneInfo
        public SceneInfo NextSceneInfo{get; set;}
        public string LaunchSplashScene
        {
            get
            {
                return GetSceneInfoByName(LaunchSplash).Name;
            }
        }
        public string AppStartScene
        {
            get
            {
                return GetSceneInfoByName(AppStart).Name;
            }
        }
        public string LoadingScene
        {
            get
            {
                return GetSceneInfoByName(Loading).Name;
            }
        }
        #endregion

        public SceneInfoMgr()
        {
            //异步加载场景过渡Scene
            SceneInfo sceneInfo = new SceneInfo()
            {
                Index = 0,
                Name = Loading,
            };
            SceneInfoDict.Add(sceneInfo.Name, sceneInfo);
            //启动LaunchSplash的Scene
            sceneInfo = new SceneInfo()
            {
                Index = 1,
                Name = LaunchSplash,
            };
            SceneInfoDict.Add(sceneInfo.Name, sceneInfo);
            //AppStart的Scene
            sceneInfo = new SceneInfo()
            {
                Index = 2,
                Name = AppStart,
            };
            SceneInfoDict.Add(sceneInfo.Name, sceneInfo);
        }

        #region 方法
        public SceneInfo GetSceneInfoByName(string name)
        {
            return SceneInfoDict[name];
        }
        public SceneInfo GetSceneInfoByIndex(int index)
        {
            foreach (var sceneInfo in SceneInfoDict)
            {
                if (sceneInfo.Value.Index == index)
                {
                    return sceneInfo.Value;
                }
            }
            return null;
        }
        #endregion
    }
}
