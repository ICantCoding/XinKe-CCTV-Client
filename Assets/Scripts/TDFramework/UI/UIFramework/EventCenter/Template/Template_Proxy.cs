

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using PureMVC.Patterns.Proxy;

    public class Template_Proxy : Proxy
    {
        #region 字段
        private static string Name = "Template_Proxy";
        #endregion

        #region 构造函数
        public Template_Proxy() : this(null, null)
        {

        }
        public Template_Proxy(object data) : this(null, data)
        {
            
        }
        public Template_Proxy(string proxyName, object data = null) : base(proxyName, data)
        {
            
        }
        #endregion

        #region 重写方法
        public override void OnRegister()
        { 
            
        }        
        public override void OnRemove()
        {

        }
        #endregion
    }
}