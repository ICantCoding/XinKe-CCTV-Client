using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TDFramework;

//网络消息与处理方法类的一种映射关系类
public class NetworkMsgHandleFuncMap : Singleton<NetworkMsgHandleFuncMap>
{
    #region 字段
    private Dictionary<UInt16, Dictionary<UInt16, string>> m_networkMsgHandleFuncMapDict = 
        new Dictionary<UInt16, Dictionary<UInt16, string>>();
    #endregion

    #region 构造方法
    public NetworkMsgHandleFuncMap()
    {
        UInt16 firstId, secondId = 0;
        string handleClassName = "";
        //U3D登录信令
        firstId = 0;
        secondId = 0;
        handleClassName = "U3DClientLoginResponseHandle";
        AddHandleClassName(firstId, secondId, handleClassName);

        //Station登录信令
        firstId = 0;
        secondId = 1;
        handleClassName = "StationClientLoginResponseHandle";
        AddHandleClassName(firstId, secondId, handleClassName);

        //NpcPosition同步信令
        firstId = 0;
        secondId = 2;
        handleClassName = "NpcPositionHandle";
        AddHandleClassName(firstId, secondId, handleClassName);

        //NpcAnimation同步信令
        firstId = 0;
        secondId = 3;
        handleClassName = "NpcAnimationHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
    }
    #endregion 

    #region 方法
    private void AddHandleClassName(UInt16 firstId, UInt16 secondId, string handleClassName)
    {
        if (string.IsNullOrEmpty(handleClassName)) return;
        Dictionary<UInt16, string> dic = null;
        m_networkMsgHandleFuncMapDict.TryGetValue(firstId, out dic);
        if (dic == null)
        {
            dic = new Dictionary<UInt16, string>();
            m_networkMsgHandleFuncMapDict.Add(firstId, dic);
        }
        if (dic.ContainsKey(secondId) == false)
        {
            dic.Add(secondId, handleClassName);
        }
    }
    public BaseHandle GetHandleInstantiateObj(UInt16 firstId, UInt16 secondId, BaseNetworkEngine networkEngine)
    {
        string handleClassName = "";
        if (m_networkMsgHandleFuncMapDict.ContainsKey(firstId))
        {
            if (m_networkMsgHandleFuncMapDict[firstId].ContainsKey(secondId))
            {
                handleClassName = m_networkMsgHandleFuncMapDict[firstId][secondId];
            }
        }
        if (string.IsNullOrEmpty(handleClassName)) return null;
        //带参数的反射类实例
        Assembly assembly = Type.GetType(handleClassName).Assembly;
        Object[] parameters = new Object[1];
        parameters[0] = networkEngine;
        BaseHandle handle = (BaseHandle)Assembly.Load(assembly.FullName).CreateInstance(handleClassName, false, BindingFlags.Default, null, parameters, null, null);
        return handle;
    }
    #endregion
}
