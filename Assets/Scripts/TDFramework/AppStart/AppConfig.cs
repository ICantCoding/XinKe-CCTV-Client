

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum GamePlatform
    {
        GamePlatform_Editor = 0,    //编辑器模式
        GamePlatform_PC = 1,        //PC模式
        GamePlatform_Mobbile = 2,   //移动端
    }

    public class GameConfig
    {
        //指定游戏平台
        public static GamePlatform gamePlatform = GamePlatform.GamePlatform_Editor; //默认在编辑器平台下
    }

    public class AppConfig
    {
        //应用开启，读取的配置文件
        public static string ApplicatioinConfigFileName = "ApplicationConfig.ini";

        #region 版本更新相关配置参数
        //本地version.json文件相对路径
        public static string VersionFilePath = "Config/Version/version.json";
        //远程version.json下载地址
        public static string RemoteVersionFileUrl = "http://192.168.0.111:3333/" + VersionFilePath;
        //下载version.json的超时时间设置
        public static int DownloadFileTimeout = 10;
        //下载version.json的最多次数
        public static int DownloadFileFailedTryCount = 3;
        //下载失败下次下载的延时时间
        public static float DownloadFileTryAgainDelay = 2.0f;
        #endregion

        #region 资源更新相关配置参数
        //本地Md5File.txt文件相对路径
        public static string Md5FilePath = "Config/Md5/md5file.txt";
        //下载缓存的TempMd5File.txt文件相对路径
        public static string Temp_Md5FilePath = "Config/Md5/temp_md5file.txt";
        //远程Md5Fiel.txt下载地址
        public static string RemoteMd5FileUrl = "http://192.168.0.111:3333/" + Md5FilePath;
        //资源下载地址
        public static string ResourcesDownloadUrl = "http://192.168.0.111:3333/";
        #endregion

    }

    public class GameLayers
    {
        
    }

    public class GameTags
    {

    }
}
