using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TDFramework;
using TDFramework.UIFramework;
using PureMVC.Interfaces;

public class Template_Panel_Mediator : UIPanel
{
    #region 字段
    private Button m_btn1;
    private Button m_btn2;
    #endregion

    #region Unity生命周期
    protected override void Awake()
    {
        base.Awake();

        //绑定跟当前UIPanel相关的Mediator, Command, Proxy
        Template_Command command = new Template_Command();
        RegisterCommand("Event1", command);
        RegisterCommand("Event2", command);
        RegisterCommand("Event3", command); //注册Command
        RegisterMediator(new Template_Mediator()); //注册Mediator
        RegisterMediator(this); //注册UIPanelMediator
        RegisterProxy(new Template_Proxy());    //注册Proxy

        //获取UI
        m_btn1 = transform.Find("Button1").GetComponent<Button>();
        if (m_btn1 != null)
        {
            m_btn1.onClick.AddListener(OnBtn1Click);
        }
        m_btn2 = transform.Find("Button2").GetComponent<Button>();
        if (m_btn2 != null)
        {
            m_btn2.onClick.AddListener(OnBtn2Click);
        }
    }
    protected void OnDestroy()
    {
        RemoveCommand("Event1");
        RemoveCommand("Event2");
        RemoveCommand("Event3");
        RemoveMediator(Template_Mediator.NAME);
        RemoveMediator(this.MediatorName);
        RemoveProxy(Template_Proxy.NAME);
    }
    #endregion

    #region Mediator功能实现
    public override string[] ListNotificationInterests()
    {
        return new string[]{
            "Event1",
            "Event2",
            "Event3",
            "Event4",
            "Event5"
        };
    }
    public override void HandleNotification(INotification notification)
    {
        if (notification.Name == "Event1")
        {
            UnityEngine.Debug.Log("UIPanel Handle Event1 Message.");
        }
        else if (notification.Name == "Event2")
        {
            UnityEngine.Debug.Log("UIPanel Handle Event2 Message.");
        }
        else if (notification.Name == "Event3")
        {
            UnityEngine.Debug.Log("UIPanel Handle Event3 Message.");
        }
        else if (notification.Name == "Event4")
        {
            UnityEngine.Debug.Log("UIPanel Handle Event4 Message.");
        }
        else if(notification.Name == "Event5")
        {
            UnityEngine.Debug.Log("UIPanel Handle Event5 Message.");
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
            return "Template_Panel_Mediator";
        }
    }
    #endregion

    #region UI事件处理
    private void OnBtn1Click()
    {
        SendNotification("Event1");
    }
    private void OnBtn2Click()
    {
        SendNotification("Event2");
    }
    #endregion	
}
