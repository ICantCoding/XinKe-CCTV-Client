
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
// 文件名(File Name):             SyncLoadScene.cs
// 作者(Author):                  田山杉
// 创建时间(CreateTime):          2019-01-27 10:17:27
// 修改者列表(modifier):
// 模块描述(Module description):  同步加载场景
// ***************************************************************

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LoadSceneMgr : Singleton<LoadSceneMgr>
    {
        #region 方法
        //直接加载某个场景,中间不需要Loading场景过渡
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        //加载过渡场景,再转入需要跳转到的场景
        public void LoadLoadingSceneToOtherScene(string toSceneName)
        {
            SingletonMgr.SceneInfoMgr.NextSceneInfo = SingletonMgr.SceneInfoMgr.GetSceneInfoByName(toSceneName);
            SceneManager.LoadScene(SingletonMgr.SceneInfoMgr.LoadingScene);
        }
        #endregion
    }
}