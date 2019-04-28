using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
GameGlobalComponent 中提供了应用中需要经常访问的全局组件，以提高访问组件效率
*/
public class GameGlobalComponent
{
    #region 字段
    private static StationMgr m_stationMgr;
    #endregion 

    #region 属性
    public static StationMgr StationMgr
    {
        get
        {
            if (m_stationMgr == null)
            {
                GameObject go = GameObject.Find(StringMgr.StationName);
                if (go != null)
                {
                    m_stationMgr = GameObject.Find(StringMgr.StationName).GetComponent<StationMgr>();
                }
            }
            return m_stationMgr;
        }
    }
    #endregion
}
