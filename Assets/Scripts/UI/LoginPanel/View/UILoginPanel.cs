

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
    private Button m_saveBtn;
    private Button m_loginBtn;
    private Text m_responseMsgText;
    private List<StationStatusCom> m_stationStatusComList = new List<StationStatusCom>();
    #endregion

    #region  事件状态字段
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
        m_saveBtn = transform.Find("Image/Image/SaveBtn").GetComponent<Button>();
        if (m_saveBtn != null)
        {
            m_saveBtn.onClick.AddListener(OnSaveBtnClick);
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
        if (m_isLoginSuccess == true)
        {
            SingletonMgr.LoadSceneMgr.LoadLoadingSceneToOtherScene(SingletonMgr.SceneInfoMgr.MainStationScene);
            m_isLoginSuccess = false;
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
    public override void OnRegister()
    {

    }
    public override void OnRemove()
    {

    }
    public override string MediatorName
    {
        get
        {
            return "UILoginPanel";
        }
    }
    #endregion

    #region 方法
    //检查表单信息是否合格, 目前只做了简单的是否为空判断
    private bool CheckTableInfoIsCorrect()
    {
        if (m_u3dIdInput != null && string.IsNullOrEmpty(m_u3dIdInput.text))
        {
            return false;
        }
        if (m_serverIpInput != null && string.IsNullOrEmpty(m_serverIpInput.text))
        {
            return false;
        }
        if (m_serverPortInput != null && string.IsNullOrEmpty(m_serverPortInput.text))
        {
            return false;
        }
        return true;
    }
    #endregion

    #region UI事件处理
    private void OnSaveBtnClick()
    {
        if (CheckTableInfoIsCorrect())
        {
            PlayerInfo.SerializePlayerInfo2Xml(System.UInt16.Parse(m_u3dIdInput.text),
            m_u3dNameInput.text, m_serverIpInput.text, int.Parse(m_serverPortInput.text));
            SingletonMgr.GameGlobalInfo.PlayerInfo = PlayerInfo.DeserializePlayerInfoFromXml();
        }
    }
    private void OnLoginBtnClick()
    {
        if (CheckTableInfoIsCorrect())
        {
            if (m_playerNetworkEngine != null)
            {
                if (m_playerNetworkEngine.IsConnected)
                {
                    //如果客户端Socket已经连接到服务器，那么只需向服务器发送客户端基本信息（针对登录失败后，再次登录的情况）
                    m_playerNetworkEngine.SendU3DClientLoginInfoRequest();
                }
                else
                {
                    //如果首次客户端连接服务器，需要先连接客户端后，再向服务器发送客户端基本信息
                    PlayerInfo.SerializePlayerInfo2Xml(System.UInt16.Parse(m_u3dIdInput.text),
                        m_u3dNameInput.text,
                        m_serverIpInput.text,
                        int.Parse(m_serverPortInput.text));
                    SingletonMgr.GameGlobalInfo.PlayerInfo = PlayerInfo.DeserializePlayerInfoFromXml();
                    m_playerNetworkEngine.Run(RemoteClientConnectServerSuccess_Callback,
                        RemoteClientConnectServerFail_Callback);
                }
            }
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
        object[] objs = (object[])notification.Body;
        System.UInt16 stationIndex = (System.UInt16)objs[0];
        System.UInt16 stationSocketType = (System.UInt16)objs[1];
        StationStatusCom com = m_stationStatusComList[stationIndex];
        com.SetCell(stationSocketType, true);
    }
    private void StationClientOnLineFail_Callback(INotification notification)
    {
        object[] objs = (object[])notification.Body;
        System.UInt16 stationIndex = (System.UInt16)objs[0];
        System.UInt16 stationSocketType = (System.UInt16)objs[1];
        StationStatusCom com = m_stationStatusComList[stationIndex];
        com.SetCell(stationSocketType, true);
    }
    #endregion

    #region 回调方法处理
    private void RemoteClientConnectServerSuccess_Callback()
    {
        if (m_playerNetworkEngine != null)
        {
            System.Threading.Thread.Sleep(20);
            m_playerNetworkEngine.SendU3DClientLoginInfoRequest();
        }
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

