
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
// 文件名(File Name):             UIPanelAssetInfo.cs
// 作者(Author):                  田山杉
// 创建时间(CreateTime):          2019-01-10 22:18:07
// 修改者列表(modifier):
// 模块描述(Module description):  生成UIPanel配置的ScriptableObject, 生成目录在Resources/UI/Config下
// ***************************************************************

namespace TDFramework.UIFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.IO;

    [System.Serializable]
    public class UIPanelAssetInfo
    {
        public UIPanelType PanelType;
        public string ResourcePanelPath;
    }

    public class UIPanelAssetInfoConfig : ScriptableObject
    {
        public List<UIPanelAssetInfo> list;
    }

    public class UIPanelAssetInfoConfigCreator : Editor
    {
        [MenuItem("Tools/UI/生成UIPanelAssetInfoConfig配置文件", false, 1)]
        private static void CreateUIPanelAssetInfoConfig()
        {
            UIPanelAssetInfoConfig config = ScriptableObject.CreateInstance<UIPanelAssetInfoConfig>();
            string configFileName = "/TDFramework/UI/UIFramework/Resources/UI/Config/UIPanelAssetInfoConfig.asset";
            if(File.Exists(Application.dataPath + configFileName))
            {
                File.Delete(Application.dataPath + configFileName);
            }
            AssetDatabase.CreateAsset(config, "Assets" + configFileName);
            AssetDatabase.Refresh();
        }
    }

}

