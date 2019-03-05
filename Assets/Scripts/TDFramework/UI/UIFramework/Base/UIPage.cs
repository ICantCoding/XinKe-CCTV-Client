

namespace TDFramework.UIFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using PureMVC.Patterns.Command;
    using PureMVC.Patterns.Mediator;
    using PureMVC.Patterns.Proxy;
    using PureMVC.Patterns.Facade;
    using PureMVC.Interfaces;

    //自定义UIPanel的类型
    public enum UIPanelType
    {
        Main,           //主界面UIPanel
        Task,           //任务界面UIPanel
        Backpack,       //背包界面UIPanel
                        //...
    }
    //自定义UIView的类型
    public enum UIViewType
    {
        FirstView,
        SecondView,
        ThirdView,
    }

    //UIPage 实现IRegisterPureMVC接口，就会具有能够注册上跟当前UIPage绑定的Command, Mediator, Proxy功能
    //UIPage 实现IMediator接口，就会让UIPage本身也是一个Mediator
    public class UIPage : MonoBehaviour, IRegisterPureMVC, IMediator
    {
        #region PureMVC相关字段
        protected ICommand m_command;
        protected IMediator m_mediator;
        protected IProxy m_proxy;
        #endregion

        #region UI字段和属性
        protected CanvasGroup m_canvasGroup;
        #endregion

        #region Unity生命周期
        protected virtual void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
        }
        #endregion

        #region 可继承重写方法
        public virtual void Hide()
        {
            m_canvasGroup.alpha = 0.0f;
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
        }
        public virtual void Show()
        {
            m_canvasGroup.alpha = 1.0f;
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
        }
        //进入回调
        public virtual void OnEnter() { }
        //暂停回调
        public virtual void OnPause() { }
        //继续回调
        public virtual void OnResume() { }
        //退出回调
        public virtual void OnExit() { }
        #endregion

        #region IRegisterPureMVC接口
        //注册除UIPage本身之外的IMediator
        public void RegisterMediator(IMediator mediator)
        {
            m_mediator = mediator;
            if (m_mediator != null)
            {
                Facade.Instance.RegisterMediator(m_mediator);
            }
        }
        public IMediator RetrieveMediator(string mediatorName)
        {
            if(string.IsNullOrEmpty(mediatorName)) return null;
            return Facade.Instance.RetrieveMediator(mediatorName);
        }
        public IMediator RemoveMediator(string mediatorName)
        {
            if(string.IsNullOrEmpty(mediatorName)) return null;

            return Facade.Instance.RemoveMediator(mediatorName);
        }
        public bool HasMediator(string mediatorName)
        {
            if(string.IsNullOrEmpty(mediatorName)) return false;
            return Facade.Instance.HasMediator(mediatorName);
        }
        //注册ICommand
        public void RegisterCommand(string notification, ICommand command)
        {
            if (string.IsNullOrEmpty(notification)) return;
            m_command = command;
            if (m_command != null)
            {
                Facade.Instance.RegisterCommand(notification, () => command);
            }
        }
        public void RemoveCommand(string notificationName)
        {
            if(string.IsNullOrEmpty(notificationName)) return;
            Facade.Instance.RemoveCommand(notificationName);
        }
        public bool HasCommand(string notificationName)
        {
            if(string.IsNullOrEmpty(notificationName)) return false;
            return Facade.Instance.HasCommand(notificationName);            
        }
        //注册IProxy
        public void RegisterProxy(IProxy proxy)
        {
            m_proxy = proxy;
            if (m_proxy != null)
            {
                Facade.Instance.RegisterProxy(m_proxy);
            }
        }
        public IProxy RetrieveProxy(string proxyName)
        {
            if(string.IsNullOrEmpty(proxyName)) return null;
            return Facade.Instance.RetrieveProxy(proxyName);
        }
        public IProxy RemoveProxy(string proxyName)
        {
            if(string.IsNullOrEmpty(proxyName)) return null;
            return Facade.Instance.RemoveProxy(proxyName);
        }
        public bool HasProxy(string proxyName)
        {
            if(string.IsNullOrEmpty(proxyName)) return false;
            return Facade.Instance.HasProxy(proxyName);            
        }
        #endregion

        #region IMediator接口实现, 实现UIPage自身的Mediator功能
        public virtual string[] ListNotificationInterests()
        {
            return null;
        }
        public virtual void HandleNotification(INotification notification)
        {

        }
        public virtual void OnRegister()
        {

        }
        public virtual void OnRemove()
        {

        }
        //Mediator本身具有实现发送消息通知的功能.
        public virtual void SendNotification(string notificationName, object body = null, string type = null)
        {
            if (string.IsNullOrEmpty(notificationName)) return;
            Facade.Instance.SendNotification(notificationName, body, type);
        }
        public virtual string MediatorName
        {
            get
            {
                return "XXXMediator";
            }
        }
        public virtual object ViewComponent
        {
            get; set;
        }
        #endregion  
    }
}