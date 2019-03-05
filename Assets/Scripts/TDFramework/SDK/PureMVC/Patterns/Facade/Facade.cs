
using System;
using PureMVC.Interfaces;
using PureMVC.Core;
using PureMVC.Patterns.Observer;

namespace PureMVC.Patterns.Facade
{
    public class Facade : IFacade
    {
        #region 字段和属性
        protected IController controller;
        protected IModel model;
        protected IView view;
        protected const string Singleton_MSG = "Facade Singleton already constructed!";
        #endregion

        #region 单例
        protected static IFacade instance;
        public static IFacade Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Facade();
                }
                return instance;
            }
        }
        #endregion

        #region 构造方法
        public Facade()
        {
            if (instance != null) throw new Exception(Singleton_MSG);
            instance = this;
            InitializeFacade();
        }
        #endregion

        #region 方法
        protected virtual void InitializeFacade()
        {
            InitializeModel();
            InitializeController();
            InitializeView();
        }
        protected virtual void InitializeController()
        {
            controller = Controller.GetInstance(() => new Controller());
        }
        protected virtual void InitializeModel()
        {
            model = Model.GetInstance(() => new Model());
        }
        protected virtual void InitializeView()
        {
            view = View.GetInstance(() => new View());
        }
        public virtual void RegisterCommand(string notificationName, Func<ICommand> commandFunc)
        {
            controller.RegisterCommand(notificationName, commandFunc);
        }
        public virtual void RemoveCommand(string notificationName)
        {
            controller.RemoveCommand(notificationName);
        }
        public virtual bool HasCommand(string notificationName)
        {
            return controller.HasCommand(notificationName);
        }
        public virtual void RegisterProxy(IProxy proxy)
        {
            model.RegisterProxy(proxy);
        }
        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return model.RetrieveProxy(proxyName);
        }
        public virtual IProxy RemoveProxy(string proxyName)
        {
            return model.RemoveProxy(proxyName);
        }
        public virtual bool HasProxy(string proxyName)
        {
            return model.HasProxy(proxyName);
        }
        public virtual void RegisterMediator(IMediator mediator)
        {
            view.RegisterMediator(mediator);
        }
        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return view.RetrieveMediator(mediatorName);
        }
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            return view.RemoveMediator(mediatorName);
        }
        public virtual bool HasMediator(string mediatorName)
        {
            return view.HasMediator(mediatorName);
        }
        public virtual void SendNotification(string notificationName, object body = null, string type = null)
        {
            NotifyObservers(new Notification(notificationName, body, type));
        }
        public virtual void NotifyObservers(INotification notification)
        {
            view.NotifyObservers(notification);
        }
        #endregion
    }
}
