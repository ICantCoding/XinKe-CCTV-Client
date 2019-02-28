using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppStart : MonoBehaviour 
{
	#region 字段
	private NetworkEngine m_networkEngine = null;
	#endregion

	#region Unity生命周期
	void Start () {
		InitData();
		m_networkEngine = new NetworkEngine();
	}
	
	void Update () {
		if(m_networkEngine != null)
		{
			m_networkEngine.UpdateInMainThread();
		}
	}
	#endregion


	#region AppStart初始准备数据
	private void InitData()
	{
		PlayerInfo playerInfo = GameGlobal.Instance.PlayerInfo;
	}
	#endregion







}
