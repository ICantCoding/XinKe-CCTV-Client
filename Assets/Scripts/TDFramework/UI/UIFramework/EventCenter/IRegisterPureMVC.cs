

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using PureMVC.Interfaces;
    using PureMVC.Patterns.Command;
    using PureMVC.Patterns.Mediator;
    using PureMVC.Patterns.Proxy;
    using UnityEngine;

    /*用于注册到PureMVC消息中心的接口， 类如果要接入PureMVC消息中心，需要继承这个接口*/
    public interface IRegisterPureMVC
    {
        void RegisterMediator(IMediator mediator);
        IMediator RetrieveMediator(string mediatorName);
        IMediator RemoveMediator(string mediatorName);
        bool HasMediator(string mediatorName);

        void RegisterCommand(string notification, ICommand command);
        void RemoveCommand(string notificationName);
        bool HasCommand(string notificationName);

        void RegisterProxy(IProxy proxy);
        IProxy RetrieveProxy(string proxyName);
        IProxy RemoveProxy(string proxyName);
        bool HasProxy(string proxyName);
    }
}