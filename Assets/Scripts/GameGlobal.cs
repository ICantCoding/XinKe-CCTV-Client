using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class GameGlobal : Singleton<GameGlobal>
{
    #region 客户端信息	
    private PlayerInfo m_playerInfo = null;
    public PlayerInfo PlayerInfo
    {
        get
        {
            if (m_playerInfo == null)
            {
                m_playerInfo = PlayerInfo.DeserializePlayerInfoFromXml();
                Debug.Log("id: " + m_playerInfo.Id.ToString());
            }
            return m_playerInfo;
        }
    }
    #endregion


    #region 其他信息

    #endregion
}



