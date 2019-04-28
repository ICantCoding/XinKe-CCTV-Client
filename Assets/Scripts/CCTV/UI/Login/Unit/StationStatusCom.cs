using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationStatusCom : MonoBehaviour
{
    #region 字段
    [SerializeField]
    private System.UInt16 m_stationIndex;
    [SerializeField]
    private Text m_shangXingShangCheStatusText;
    [SerializeField]
    private Text m_xiaXingShangCheStatusText;
    [SerializeField]
    private Text m_shangXingXiaCheStatusText;
    [SerializeField]
    private Text m_xiaXingXiaCheStatusText;
    [SerializeField]
    private Button m_shangshangBtn;
    [SerializeField]
    private Button m_xiashangBtn;
    [SerializeField]
    private Button m_shangxiaBtn;
    [SerializeField]
    private Button m_xiaxiaBtn;
    [SerializeField]
    private Text m_shangshangSuccessText;
    [SerializeField]
    private Text m_xiashangSuccessText;
    [SerializeField]
    private Text m_shangxiaSuccessText;
    [SerializeField]
    private Text m_xiaxiaSuccessText;
    #endregion

    #region Unity生命周期
    void Start()
    {
        //给按钮添加重连动作
        TDFramework.NetworkModule module = (TDFramework.NetworkModule)TDFramework.SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
        if(module == null) return;
        if(m_shangshangBtn != null)
        {
            m_shangshangBtn.onClick.AddListener(module.GetStationNetworkEngineByStationIndex(m_stationIndex, 1).ReLink);
        }
        if(m_xiashangBtn != null)
        {
            m_xiashangBtn.onClick.AddListener(module.GetStationNetworkEngineByStationIndex(m_stationIndex, 3).ReLink);
        }
        if(m_shangxiaBtn != null)
        {
            m_shangxiaBtn.onClick.AddListener(module.GetStationNetworkEngineByStationIndex(m_stationIndex, 2).ReLink);
        }
        if(m_xiaxiaBtn != null)
        {
            m_xiaxiaBtn.onClick.AddListener(module.GetStationNetworkEngineByStationIndex(m_stationIndex, 4).ReLink);
        }
    }
    #endregion

    #region 方法
    public void SetCell(System.UInt16 type, bool connectStatus)
    {
        string content = connectStatus ? "✔" : "✖";
        Color color = connectStatus ? Color.green : Color.red;
        if(type == 1)
        {
            m_shangXingShangCheStatusText.color = color;
            m_shangshangBtn.gameObject.SetActive(!connectStatus);
            m_shangshangSuccessText.gameObject.SetActive(connectStatus);
            m_shangXingShangCheStatusText.text = content;  
        }
        else if(type == 2)
        {
            m_shangXingXiaCheStatusText.color = color;
            m_shangxiaBtn.gameObject.SetActive(!connectStatus);
            m_shangxiaSuccessText.gameObject.SetActive(connectStatus);
            m_shangXingXiaCheStatusText.text = content;
        }
        else if(type == 3)
        {
            m_xiaXingShangCheStatusText.color = color;
            m_xiashangBtn.gameObject.SetActive(!connectStatus);
            m_xiashangSuccessText.gameObject.SetActive(connectStatus);
            m_xiaXingShangCheStatusText.text = content;
        }
        else if(type == 4)
        {
            m_xiaXingXiaCheStatusText.color = color;
            m_xiaxiaBtn.gameObject.SetActive(!connectStatus);
            m_xiaxiaSuccessText.gameObject.SetActive(connectStatus);
            m_xiaXingXiaCheStatusText.text = content;
        }
    }
    #endregion
}
