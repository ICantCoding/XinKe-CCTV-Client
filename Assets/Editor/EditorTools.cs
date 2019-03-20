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
	}
	[MenuItem("Tools/Xml/生成StationInfo.xml", false, 2)]
	private static void CreateStationInfoXml()
	{
		StationInfoList.SerializeStationInfoList2Xml();
	}
}
