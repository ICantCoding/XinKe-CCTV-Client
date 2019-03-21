using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NpcActionStatus
{
    EnterStationTrainUp_NpcActionStatus = 1,    //NPC 执行进站上行方向坐车
    EnterStationTrainDown_NpcActionStatus = 2,  //NPC 执行进站下行方向坐车
    ExitStationTrainUp_NpcActionStatus = 3,     //NPC 执行上行方向下车出站
    ExitStationTrainDown_NpcActionStatus = 4,	//NPC 执行下行方向下车出站

    None = 10000,
}


public class NpcAction : MonoBehaviour
{
    #region 字段
    [SerializeField]
    private int m_npcId;
    #endregion

    #region 属性
    public int NpcId
    {
        get { return m_npcId; }
        set { m_npcId = value; }
    }
    #endregion
}
