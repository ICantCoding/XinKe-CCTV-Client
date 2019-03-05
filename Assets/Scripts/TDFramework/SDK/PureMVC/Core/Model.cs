
using System;
using System.Collections.Concurrent;
using PureMVC.Interfaces;

namespace PureMVC.Core
{
    public class Model: IModel
    {
        #region 字段和属性
        protected readonly ConcurrentDictionary<string, IProxy> proxyMap;
        protected const string Singleton_MSG = "Model Singleton already constructed!";
        #endregion

        #region 单例
        protected static IModel instance;
        public static IModel GetInstance(Func<IModel> modelFunc)
        {
            if (instance == null) {
                instance = modelFunc();
            }
            return instance;
        }
        #endregion

        #region 构造方法
        public Model()
        {
            if (instance != null) throw new Exception(Singleton_MSG);
            instance = this;
            proxyMap = new ConcurrentDictionary<string, IProxy>(); //Model为Proxy代理
            InitializeModel();
        }
        #endregion

        #region 方法
        protected virtual void InitializeModel()
        {

        }
        public virtual void RegisterProxy(IProxy proxy)
        {
            proxyMap[proxy.ProxyName] = proxy;
            proxy.OnRegister();
        }
        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return proxyMap.TryGetValue(proxyName, out IProxy proxy) ? proxy : null;
        }
        public virtual IProxy RemoveProxy(string proxyName)
        {
            if (proxyMap.TryRemove(proxyName, out IProxy proxy))
            {
                proxy.OnRemove();
            }
            return proxy;
        }
        public virtual bool HasProxy(string proxyName)
        {
            return proxyMap.ContainsKey(proxyName);
        }
        #endregion
    }
}
