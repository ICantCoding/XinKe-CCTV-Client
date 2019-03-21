using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMgr
{
    #region 字段
    private NpcActionStatus m_npcActionStatus;
    private Dictionary<int, NpcAction> m_npcActionDict = new Dictionary<int, NpcAction>();
    #endregion

    #region 构造函数
    public NpcMgr(NpcActionStatus npcActionStatus)
    {
        m_npcActionStatus = npcActionStatus;
    }
    #endregion

    #region 属性
    public NpcActionStatus NpcActionStatus
    {
        get { return m_npcActionStatus; }
        set { m_npcActionStatus = value; }
    }
    public Dictionary<int, NpcAction> NpcActionDict
    {
        get { return m_npcActionDict; }
    }
    #endregion

    #region 方法
    public void AddNpcAction(NpcAction npcAction)
    {
        if (npcAction == null) return;
        if (m_npcActionDict.ContainsKey(npcAction.NpcId) == false)
        {
            m_npcActionDict.Add(npcAction.NpcId, npcAction);
        }
    }
    public void RemoveNpcAction(int npcId)
    {
        if (m_npcActionDict.ContainsKey(npcId))
        {
            m_npcActionDict.Remove(npcId);
        }
    }
    public NpcAction GetNpcAction(int npcId)
    {
        NpcAction npcAction = null;
        m_npcActionDict.TryGetValue(npcId, out npcAction);
        return npcAction;
    }
    #endregion
}
