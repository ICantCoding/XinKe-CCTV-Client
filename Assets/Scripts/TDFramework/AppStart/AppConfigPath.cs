using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConfigPath
{

	public static string PlayerInfoXmlPath = Application.streamingAssetsPath + "/Xml/PlayerInfo.xml";
	public static string StationInfoXmlPath = Application.streamingAssetsPath + "/Xml/StationInfo.xml";
	public static string AppInfoXmlPath = Application.streamingAssetsPath + "/Xml/AppInfo.xml";
	
	#region 摄像头信息
	public static string ZhaoYingCameraInfoXmlPath = Application.streamingAssetsPath + "/Xml/ZhaoYingCameraInfo.xml";
	#endregion

	#region 列车跑线信息
	public static string TrainLineInfoXmlPath = Application.streamingAssetsPath + "/Xml/TrainLineInfo.xml";
	#endregion
	
}
