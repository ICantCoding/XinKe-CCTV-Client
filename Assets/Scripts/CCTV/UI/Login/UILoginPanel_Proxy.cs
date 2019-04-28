
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Proxy;

public class UILoginPanel_Proxy : Proxy
{
    #region 字段
    private static string Name = "UILoginPanel_Proxy";
    #endregion

    #region 构造函数
    public UILoginPanel_Proxy() : this(null, null)
    {

    }
    public UILoginPanel_Proxy(object data) : this(null, data)
    {

    }
    public UILoginPanel_Proxy(string proxyName, object data = null) : base(proxyName, data)
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