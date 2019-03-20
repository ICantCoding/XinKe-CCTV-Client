using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationStatusCom : MonoBehaviour
{
    #region 
    [SerializeField]
    private Text m_shangXingShangCheStatusText;
    [SerializeField]
    private Text m_xiaXingShangCheStatusText;
    [SerializeField]
    private Text m_shangXingXiaCheStatusText;
    [SerializeField]
    private Text m_xiaXingXiaCheStatusText;
    #endregion

    #region 方法
    public void SetCell(System.UInt16 type, bool connectStatus)
    {
        string content = connectStatus ? "✔" : "✖";
        Color color = connectStatus ? Color.green : Color.red;
        if(type == 1)
        {
            m_shangXingShangCheStatusText.color = color;
            m_shangXingShangCheStatusText.text = content;    
        }
        else if(type == 2)
        {
            m_shangXingXiaCheStatusText.color = color;
            m_shangXingXiaCheStatusText.text = content;
        }
        else if(type == 3)
        {
            m_xiaXingShangCheStatusText.color = color;
            m_xiaXingShangCheStatusText.text = content;
        }
        else if(type == 4)
        {
            m_xiaXingXiaCheStatusText.color = color;
            m_xiaXingXiaCheStatusText.text = content;
        }
    }
    #endregion
}
