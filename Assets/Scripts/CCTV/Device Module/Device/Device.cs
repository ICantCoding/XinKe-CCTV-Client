/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-06 15:55:57

** 功能描述:  场景设备基类

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//场景设备类型
public enum DeviceType
{
    ZhaJi = 1000,           //闸机
    DianFuTi = 2000,        //电扶梯
    PingBiMen = 3000,       //屏蔽门
    SheXiangTou = 4000,     //摄像头

    None = 100000,			//无设备类型
}

//设备基类
public class Device : MonoBehaviour
{
    #region 字段
    //设备类型
    [SerializeField]
    protected DeviceType m_deviceType;
    //设备唯一标识
    [SerializeField]
    protected int m_deviceId;
    //设备所属站台
    [SerializeField]
    protected UInt16 m_stationIndex;
    #endregion

    #region 属性
    public virtual DeviceType DeviceType
    {
        get { return m_deviceType; }
        set { m_deviceType = value; }
    }
    public int DeviceId
    {
        get { return m_deviceId; }
        set { m_deviceId = value; }
    }
    public UInt16 StationIndex
    {
        get { return m_stationIndex; }
        set { m_stationIndex = value; }
    }
    #endregion

    #region Unity生命周期

    #endregion

    #region 方法
    public virtual void Open(System.Action openCallback)
    {
       
    }
    public virtual void Close(System.Action closeCallback)
    {

    }
    #endregion
}

