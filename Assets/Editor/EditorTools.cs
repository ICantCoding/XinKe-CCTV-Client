using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTools
{
	[MenuItem("Tools/Xml/生成PlayerInfo.xml",false ,1)]
	private static void CreatePlayerInfoXml()
	{
		PlayerInfo.SerializePlayerInfo2Xml();
		AssetDatabase.Refresh();
		Debug.Log("PlayerInfo.xml生成完成.");
	}
	[MenuItem("Tools/Xml/生成StationInfo.xml", false, 2)]
	private static void CreateStationInfoXml()
	{
		StationInfoList.SerializeStationInfoList2Xml();
		AssetDatabase.Refresh();
		Debug.Log("StationInfo.xml生成完成.");
	}
	[MenuItem("Tools/Xml/生成AppInfo.Xml", false, 3)]
	private static void CreateAppInfoXml()
	{
		AppInfo.SerializeAppInfo2Xml();
		AssetDatabase.Refresh();
		Debug.Log("AppInfo.xml生成完成.");
	}
	[MenuItem("Tools/Xml/摄像头信息/生成ZhaoYingCameraInfo.Xml", false, 1)]
	private static void CreateZhaoYingCameraInfoXml()
	{
		GameObject go = GameObject.Find("Camera Info/CameraRoot/ZhaoYing");
		if(go == null) return;
		CameraInfoList.SerializeCameraInfo2Xml(AppConfigPath.ZhaoYingCameraInfoXmlPath, go.transform);
		AssetDatabase.Refresh();
		Debug.Log("赵营摄像头数据Xml生成完成.");
	}
	[MenuItem("Tools/Xml/生成TrainLineInfo.xml", false, 4)]
	private static void CreateTrainLineInfoXml()
	{
		AllTrainLineInfo.SerializeTrainLineInfo2Xml();
		AssetDatabase.Refresh();
		Debug.Log("TrainLineInfo.xml生成完成.");
	}
}
