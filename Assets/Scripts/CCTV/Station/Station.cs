using System;
using System.Collections;
using System.Collections.Generic;

public class Station
{
    #region 设备相关

    #endregion

    #region Npc相关

    #region 字段
    private Dictionary<NpcActionStatus, NpcMgr> m_npcMgrDict = new Dictionary<NpcActionStatus, NpcMgr>();
    #endregion

    #region 方法
    public void AddNpcMgr(NpcMgr npcMgr)
    {
        if (npcMgr == null) return;
        if (m_npcMgrDict.ContainsKey(npcMgr.NpcActionStatus) == false)
        {
            m_npcMgrDict.Add(npcMgr.NpcActionStatus, npcMgr);
        }
    }
    public NpcMgr GetNpcMgr(NpcActionStatus npcActionStatus)
    {
        NpcMgr npcMgr = null;
        m_npcMgrDict.TryGetValue(npcActionStatus, out npcMgr);
        return npcMgr;
    }
    public void AddNpcAction(NpcActionStatus npcActionStatus, NpcAction npcAction)
    {
        NpcMgr npcMgr = GetNpcMgr(npcActionStatus);
        if (npcMgr == null) return;
        npcMgr.AddNpcAction(npcAction);
    }
    public void RemoveNpcAction(NpcActionStatus npcActionStatus, int npcId)
    {
        NpcMgr npcMgr = GetNpcMgr(npcActionStatus);
        if (npcMgr == null) return;
        npcMgr.RemoveNpcAction(npcId);
    }
    public NpcAction GetNpcAction(NpcActionStatus npcActionStatus, int npcId)
    {
        NpcMgr npcMgr = GetNpcMgr(npcActionStatus);
        if(npcMgr == null) return null;
        return npcMgr.GetNpcAction(npcId);
    }
    #endregion

    #endregion
}
