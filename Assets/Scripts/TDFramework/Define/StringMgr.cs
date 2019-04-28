using System;
using System.Collections;
using System.Collections.Generic;

public class StringMgr
{
    #region 各种引擎名字
    public const string ResourceEngineName = "ResourceEngine";
    public const string NetworkEngineName = "NetworkEngine";
    #endregion

    #region 游戏物体名称
    public const string StationName = "Station";
    #endregion

    #region 模块名称
    public const string LaunchModuleName = "LaunchModule";
    public const string NetworkModuleName = "NetworkModule";
    public const string ResourcesModuleName = "ResourcesModule";
    #endregion

    #region 预制件路径名称
    public static string Man1_Npc = "Assets/Prefabs/Npc/Man1_Npc.prefab";
    public static string Man2_Npc = "Assets/Prefabs/Npc/Man2_Npc.prefab";
    public static string Man3_Npc = "Assets/Prefabs/Npc/Man3_Npc.prefab";
    public static string Man4_Npc = "Assets/Prefabs/Npc/Man4_Npc.prefab";
    public static string Man5_Npc = "Assets/Prefabs/Npc/Man5_Npc.prefab";
    public static string Woman1_Npc = "Assets/Prefabs/Npc/Woman1_Npc.prefab";
    public static string Woman2_Npc = "Assets/Prefabs/Npc/Woman2_Npc.prefab";
    public static string Woman3_Npc = "Assets/Prefabs/Npc/Woman3_Npc.prefab";
    public static string Woman4_Npc = "Assets/Prefabs/Npc/Woman4_Npc.prefab";
    public static string Woman5_Npc = "Assets/Prefabs/Npc/Woman5_Npc.prefab";
    public static string BigScreen = "Assets/Prefabs/UI/BigScreen.prefab";
    public static string SmallScreen = "Assets/Prefabs/UI/SmallScreen.prefab";
    public static string RawBigScreen = "Assets/Prefabs/UI/RawBigScreen.prefab";
    public static string RawSmallScreen = "Assets/Prefabs/UI/RawSmallScreen.prefab";
    public static string CCTVCamera = "Assets/Prefabs/Camera/CCTV Camera.prefab";
    #endregion	

    #region Npc模型路径名
    public static string[] NpcModelNameList = new string[10]
    {
        Man1_Npc,
        Man2_Npc,
        Man3_Npc,
        Man4_Npc,
        Man5_Npc,
        Woman1_Npc,
        Woman2_Npc,
        Woman3_Npc,
        Woman4_Npc,
        Woman5_Npc
    };
    #endregion

    #region 行车方向
    public const string ShangXingShangChe = "ShangXingShangChe";
    public const string ShangXingXiaChe = "ShangXingXiaChe";
    public const string XiaXingShangChe = "XiaXingShangChe";
    public const string XiaXingXiaChe = "XiaXingXiaChe";
    #endregion
    
}
