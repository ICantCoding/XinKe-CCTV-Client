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
        UInt16 firstId, secondId;
        string handleClassName = "";

        //==========================================================================================================
        //U3DClient和U3DServer交互信令===============================================================================
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
        //NpcDestroy销毁信令
        firstId = 0;
        secondId = 4;
        handleClassName = "NpcDestroyHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //DeviceAction设备动作信令
        firstId = 0;
        secondId = 5;
        handleClassName = "DeviceActionHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //客户端重连信令
        firstId = 0;
        secondId = 6;
        handleClassName = "ClientReConnectHandle";
        AddHandleClassName(firstId, secondId, handleClassName);


        //==========================================================================================================
        //ATS和U3DClient交互信令,其实是U3DServer转发ATS信令===========================================================
        //屏蔽门控制信令
        firstId = 10000;
        secondId = 0;
        handleClassName = "PingBiMenCtrlHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //Npc人物上下行，上下车信令
        firstId = 10000;
        secondId = 10;
        handleClassName = "NpcCtrlHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //Npc任务上下行，上下车完毕信令
        firstId = 10000;
        secondId = 20;
        handleClassName = "NpcCtrlCompleteHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //加载列车信令
        firstId = 10000;
        secondId = 30;
        handleClassName = "LoadTrainHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //卸载列车信令
        firstId = 10000;
        secondId = 40;
        handleClassName = "UnloadTrainHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //定位车信令
        firstId = 10000;
        secondId = 50;
        handleClassName = "TrainPositionHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //列车开门信令
        firstId = 10000;
        secondId = 60;
        handleClassName = "TrainDoorCtrlHandle";
        AddHandleClassName(firstId, secondId, handleClassName);


        //==========================================================================================================
        //CCTV和U3DClient交互信令,其实是U3DServer转发CCTV信令=========================================================
        //接收到大屏切割信令
        firstId = 20000;
        secondId = 10;
        handleClassName = "DivisionBigScreenHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //接收到指定大屏切割为小屏信令
        firstId = 20000;
        secondId = 20;
        handleClassName = "DivisionSmallScreenHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //接收到摄像头绑定屏幕信令
        firstId = 20000;
        secondId = 30;
        handleClassName = "ScreenBindCameraHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //摄像头旋转
        firstId = 20000;
        secondId = 40;
        handleClassName = "CameraRotateHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        //摄像头缩放
        firstId = 20000;
        secondId = 50;
        handleClassName = "CameraZoomHandle";
        AddHandleClassName(firstId, secondId, handleClassName);
        
    }
    #endregion 

    #region 方法
    private void AddHandleClassName(MessageID messageId, string handleClassName)
    {
        AddHandleClassName(messageId.FirstID, messageId.SecondID, handleClassName);
    }
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
    public BaseHandle GetHandleInstantiateObj(MessageID messageId, BaseNetworkEngine networkEngine)
    {
        return GetHandleInstantiateObj(messageId.FirstID, messageId.SecondID, networkEngine);
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
