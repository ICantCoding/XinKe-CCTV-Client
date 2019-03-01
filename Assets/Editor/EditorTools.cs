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
}
