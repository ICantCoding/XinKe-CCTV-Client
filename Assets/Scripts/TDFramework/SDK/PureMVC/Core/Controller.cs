
using System;
using System.Collections.Concurrent;
using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;

namespace PureMVC.Core
{
    public class Controller : IController
    {
        #region 字段和属性
        protected IView view;
        protected readonly ConcurrentDictionary<string, Func<ICommand>> commandMap;
        protected const string Singleton_MSG = "Controller Singleton already constructed!";
        #endregion

        #region 单例
        protected static IController instance;
        public static IController GetInstance(Func<IController> controllerFunc)
        {
            if (instance == null)
            {
                instance = controllerFunc();
            }
            return instance;
        }
        #endregion

        #region 构造函数
        public Controller()
        {
            if (instance != null) throw new Exception(Singleton_MSG);
            instance = this;
            commandMap = new ConcurrentDictionary<string, Func<ICommand>>();
            InitializeController();
        }
        #endregion

        #region 方法
        protected virtual void InitializeController()
        {
            view = View.GetInstance(() => new View());
        }
        public virtual void ExecuteCommand(INotification notification)
        {
            if (commandMap.TryGetValue(notification.Name, out Func<ICommand> commandFunc))
            {
                ICommand commandInstance = commandFunc();
                commandInstance.Execute(notification);
            }
        }
        public virtual void RegisterCommand(string notificationName, Func<ICommand> commandFunc)
        {
            if (commandMap.TryGetValue(notificationName, out Func<ICommand> _) == false)
            {
                view.RegisterObserver(notificationName, new Observer(ExecuteCommand, this)); //添加监听者
            }
            commandMap[notificationName] = commandFunc; //会主动去重
        }
        public virtual void RemoveCommand(string notificationName)
        {
            if (commandMap.TryRemove(notificationName, out Func<ICommand> _))
            {
                view.RemoveObserver(notificationName, this);
            }
        }
        public virtual bool HasCommand(string notificationName)
        {
            return commandMap.ContainsKey(notificationName);
        }
        #endregion

    }
}
