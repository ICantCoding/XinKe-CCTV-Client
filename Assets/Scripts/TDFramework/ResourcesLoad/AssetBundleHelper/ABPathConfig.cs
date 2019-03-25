

namespace TDFramework
{
    using System;
    using UnityEngine;

    public class ABPathConfig
    {
        public static string AssetBundleConfigPath =
            "Assets/Scripts/TDFramework/ResourcesLoad/AssetBundleHelper/Config/AssetBundleConfig.asset"; //AB包打包方式配置文件
        public static string AssetBundleBuildTargetPath = Application.streamingAssetsPath + "/AssetBundles";    //AB包打包生成目录
        public static string AssetBundleDependenceXmlPath =
            Application.dataPath + "/Scripts/TDFramework/ResourcesLoad/AssetBundleHelper/Config/AssetBundleDependenceConfig.xml"; //AB包依赖xml文件
        public static string AssetBundleDependenceBytePath =
        Application.dataPath + "/Scripts/TDFramework/ResourcesLoad/AssetBundleHelper/Config/AssetBundleDependenceConfig.bytes"; //AB包依赖二进制文件
        //AB包依赖文件所在的AB包（依赖文件也被打包进了AB包, 并以assetbundleconfig为该AB包命名）
        public static string DependenceFile4AssetBundle = 
			AssetBundleBuildTargetPath + "/assetbundleconfig"; 
		public static string DependenceFileName = "AssetBundleDependenceConfig.bytes"; //依赖文件名字
    }
}
