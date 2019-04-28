
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TDFramework;
using TDFramework.UIFramework;
using PureMVC.Interfaces;

public class UILoginPanel : UIPanel
{
    #region 字段
    private InputField m_u3dIdInput;
    private InputField m_u3dNameInput;
    private InputField m_serverIpInput;
    private InputField m_serverPortInput;
    private Button m_linkBtn;
    private Button m_loginBtn;
    private Text m_responseMsgText;
    private List<StationStatusCom> m_stationStatusComList = new List<StationStatusCom>();
    #endregion

    #region  事件状态字段
    private bool m_isLinked = false;
    private bool m_isLoginSuccess = false;
    #endregion

    #region 网络引擎字段
    private PlayerNetworkEngine m_playerNetworkEngine;
    #endregion

    #region Unity生命周期
    protected override void Awake()
    {
        base.Awake();

        //绑定跟当前UIPanel相关的Mediator, Command, Proxy
        UILoginPanel_Command command = new UILoginPanel_Command();
        RegisterCommand(EventID_Cmd.U3DClientOnLineSuccess, command);
        RegisterCommand(EventID_Cmd.U3DClientOnLineFail, command);
        RegisterCommand(EventID_Cmd.StationClientOnLineSuccess, command);
        RegisterCommand(EventID_Cmd.StationClientOnLineFail, command);
        RegisterMediator(new UILoginPanel_Mediator()); //注册Mediator
        RegisterMediator(this); //注册UIPanelMediator
        RegisterProxy(new UILoginPanel_Proxy());    //注册Proxy

        //获取UI
        m_u3dIdInput = transform.Find("Image/Image/U3DIDInput").GetComponent<InputField>();
        m_u3dNameInput = transform.Find("Image/Image/U3DNameInput").GetComponent<InputField>();
        m_serverIpInput = transform.Find("Image/Image/IPInput").GetComponent<InputField>();
        m_serverPortInput = transform.Find("Image/Image/PortInput").GetComponent<InputField>();
        m_linkBtn = transform.Find("Image/Image/LinkBtn").GetComponent<Button>();
        if (m_linkBtn != null)
        {
            m_linkBtn.onClick.AddListener(OnLinkBtnClick);
        }
        m_loginBtn = transform.Find("Image/Image/LoginBtn").GetComponent<Button>();
        if (m_loginBtn != null)
        {
            m_loginBtn.onClick.AddListener(OnLoginBtnClick);
        }
        m_responseMsgText = transform.Find("Image/Image/ResponseMsgText").GetComponent<Text>();

        m_playerNetworkEngine = GameObject.Find("NetworkEngine/Player").GetComponent<PlayerNetworkEngine>();

        Transform stationStausTrans = transform.Find("Station Staus/Scroll View/Viewport/Content");
        int count = stationStausTrans.childCount;
        for (int i = 0; i < count; ++i)
        {
            m_stationStatusComList.Add(stationStausTrans.GetChild(i).GetComponent<StationStatusCom>());
        }
    }
    void Start()
    {
        string u3dId = SingletonMgr.GameGlobalInfo.PlayerInfo.Id.ToString();
        string u3dName = SingletonMgr.GameGlobalInfo.PlayerInfo.Name.ToString();
        string serverIp = SingletonMgr.GameGlobalInfo.PlayerInfo.ServerIpAddress.ToString();
        string serverPort = SingletonMgr.GameGlobalInfo.PlayerInfo.ServerPort.ToString();
        if (m_u3dIdInput != null)
        {
            m_u3dIdInput.text = u3dId;
        }
        if (m_u3dNameInput != null)
        {
            m_u3dNameInput.text = u3dName;
        }
        if (m_serverPortInput != null)
        {
            m_serverIpInput.text = serverIp;
        }
        if (m_serverPortInput != null)
        {
            m_serverPortInput.text = serverPort;
        }
    }
    void Update()
    {
        if (m_isLoginSuccess)
        {
            SingletonMgr.LoadSceneMgr.LoadLoadingSceneToOtherScene(SingletonMgr.SceneInfoMgr.MainStationScene);
            m_isLoginSuccess = false;
        }
        if (m_isLinked)
        {
            m_linkBtn.interactable = false;
            m_linkBtn.GetComponent<Image>().color = new Color(117.0f / 255.0f, 123.0f / 255.0f, 115.0f / 255.0f, 160.0f / 255.0f);
        }
    }
    protected void OnDestroy()
    {
        RemoveCommand(EventID_Cmd.U3DClientOnLineSuccess);
        RemoveCommand(EventID_Cmd.U3DClientOnLineFail);
        RemoveCommand(EventID_Cmd.StationClientOnLineSuccess);
        RemoveCommand(EventID_Cmd.StationClientOnLineFail);
        RemoveMediator(UILoginPanel_Mediator.NAME);
        RemoveMediator(this.MediatorName);
        RemoveProxy(UILoginPanel_Proxy.NAME);
    }
    #endregion

    #region Mediator功能实现
    public override string[] ListNotificationInterests()
    {
        return new string[]{
            EventID_UI.U3DClientOnLineSuccess,
            EventID_UI.U3DClientOnLineFail,
            EventID_UI.StationClientOnLineSuccess,
            EventID_UI.StationClientOnLineFail,
        };
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case EventID_UI.U3DClientOnLineSuccess:
                {
                    U3DClientOnLineSuccess_Callback(notification);
                    break;
                }
            case EventID_UI.U3DClientOnLineFail:
                {
                    U3DClientOnLineFail_Callback(notification);
                    break;
                }
            case EventID_UI.StationClientOnLineSuccess:
                {
                    StationClientOnLineSuccess_Callback(notification);
                    break;
                }
            case EventID_UI.StationClientOnLineFail:
                {
                    StationClientOnLineFail_Callback(notification);
                    break;
                }
            default:
                break;
        }
    }
    public override string MediatorName
    {
        get { return "UILoginPanel"; }
    }
    #endregion

    #region 方法
    //检查表单信息是否合格, 目前只做了简单的是否为空判断
    private bool CheckTableInfoIsCorrect()
    {
        if (m_u3dIdInput != null && string.IsNullOrEmpty(m_u3dIdInput.text))
        {
            ShowResponseMsgText("请填写客户端Id...");
            return false;
        }
        if (m_serverIpInput != null && string.IsNullOrEmpty(m_serverIpInput.text))
        {
            ShowResponseMsgText("请填写服务器Ip地址...");
            return false;
        }
        if (m_serverPortInput != null && string.IsNullOrEmpty(m_serverPortInput.text))
        {
            ShowResponseMsgText("请填写服务器端口号...");
            return false;
        }
        return true;
    }
    #endregion

    #region UI事件处理
    private void OnLinkBtnClick()
    {
        if (CheckTableInfoIsCorrect())
        {
            PlayerInfo.SerializePlayerInfo2Xml(UInt16.Parse(m_u3dIdInput.text),
            m_u3dNameInput.text, m_serverIpInput.text, int.Parse(m_serverPortInput.text));
            SingletonMgr.GameGlobalInfo.PlayerInfo = PlayerInfo.DeserializePlayerInfoFromXml();
            m_playerNetworkEngine.Run(RemoteClientConnectServerSuccess_Callback,
                        RemoteClientConnectServerFail_Callback);
        }
    }
    private void OnLoginBtnClick()
    {
        if (m_isLinked && m_playerNetworkEngine != null && m_playerNetworkEngine.IsConnected)
        {
            m_playerNetworkEngine.SendU3DClientLoginInfoRequest();
        }
    }
    #endregion	

    #region PureMVC消息处理
    private void U3DClientOnLineSuccess_Callback(INotification notification)
    {
        m_isLoginSuccess = true;
    }
    private void U3DClientOnLineFail_Callback(INotification notification)
    {
        string msg = (string)notification.Body;
        ShowResponseMsgText(msg);
    }
    private void StationClientOnLineSuccess_Callback(INotification notification)
    {
        UInt16[] objs = (UInt16[])notification.Body;
        UInt16 stationIndex = objs[0];
        UInt16 stationSocketType = objs[1];
        StationStatusCom com = m_stationStatusComList[stationIndex];
        com.SetCell(stationSocketType, true);
    }
    private void StationClientOnLineFail_Callback(INotification notification)
    {
        UInt16[] objs = (UInt16[])notification.Body;
        UInt16 stationIndex = objs[0];
        UInt16 stationSocketType = objs[1];
        StationStatusCom com = m_stationStatusComList[stationIndex];
        com.SetCell(stationSocketType, false);
    }
    #endregion

    #region 回调方法处理
    private void RemoteClientConnectServerSuccess_Callback()
    {
        m_isLinked = true;
    }
    private void RemoteClientConnectServerFail_Callback()
    {
        ShowResponseMsgText("连接CCTV服务器失败!");
    }
    #endregion

    #region UI数据更新
    private void ShowResponseMsgText(string content)
    {
        m_responseMsgText.text = content;
        HideResponseMsgTextEffect();
    }
    #endregion

    #region UI效果实现
    private void HideResponseMsgTextEffect()
    {
        StartCoroutine(HideResponseMsgText());
    }
    private IEnumerator HideResponseMsgText()
    {
        yield return new WaitForSeconds(2.0f);
        m_responseMsgText.text = "";
    }
    #endregion
}

