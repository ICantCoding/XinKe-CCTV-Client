/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-07 10:14:07

** 功能描述:  用来读取场景中的Npc, 设备, Train

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;

public class ReadStationPoint
{
    #region 读取场景中各个站台的设备
    public static void BuildStationDevices(StationMgr stationMgr)
    {
        if (stationMgr == null) return;
        GameObject go = GameObject.Find("Device/DeviceRoot");
        if (go == null) return;
        Transform deviceRootTrans = go.transform;
        if (deviceRootTrans == null) return;
        int stationCount = deviceRootTrans.childCount;
        for (int i = 0; i < stationCount; ++i)
        {
            Transform stationTrans = deviceRootTrans.GetChild(i);
            System.UInt16 stationIndex = (UInt16)System.Enum.Parse(typeof(StationType), stationTrans.gameObject.name);
            Station station = stationMgr.GetStation(stationIndex);
            if (station == null) return;
            HHH(stationMgr, station, stationTrans, stationIndex);
        }
    }
    private static void HHH(StationMgr stationMgr, Station station, Transform stationTrans, System.UInt16 stationIndex)
    {
        int deviceTypeCount = stationTrans.childCount;
        for (int i = 0; i < deviceTypeCount; ++i)
        {
            DeviceMgr deviceMgr = null;
            Transform deviceTypeTrans = stationTrans.GetChild(i);
            DeviceType deviceType = (DeviceType)System.Enum.Parse(typeof(DeviceType), deviceTypeTrans.gameObject.name);
            if (deviceType == DeviceType.ZhaJi)
            {
                deviceMgr = new ZhaJiMgr();
            }
            else if (deviceType == DeviceType.PingBiMen)
            {
                deviceMgr = new PingBiMenMgr();
            }
            deviceMgr.DeviceType = deviceType;
            JJJ(stationMgr, deviceMgr, deviceTypeTrans, stationIndex, deviceType);
            station.AddDeviceMgr(deviceMgr);
        }
    }
    private static void JJJ(StationMgr stationMgr, DeviceMgr deviceMgr, Transform deviceTypeTrans, System.UInt16 stationIndex, DeviceType deviceType)
    {
        int deviceCount = deviceTypeTrans.childCount;
        for (int i = 0; i < deviceCount; ++i)
        {
            Transform deviceTrans = deviceTypeTrans.GetChild(i);
            Device deviceCom = deviceTrans.GetComponent<Device>();
            if (deviceCom == null) continue;
            deviceCom.DeviceId = (int)deviceType + i + 1;
            deviceCom.StationIndex = stationIndex;
            deviceMgr.AddDevice(deviceCom);
            
            #region 设备是屏蔽门，需管理上行和下行屏蔽门
            if (deviceCom is PingBiMenDevice)
            {
                PingBiMenMgr pingBiMenMgr = (PingBiMenMgr)deviceMgr;
                PingBiMenDevice device = (PingBiMenDevice)deviceCom;
                if (device.PingBiMenType == PingBiMenType.Down)
                {
                    //下行屏蔽门
                    pingBiMenMgr.AddDevice2XiaXingPingBiMenList(deviceCom);
                }
                else if (device.PingBiMenType == PingBiMenType.Up)
                {
                    //上行屏蔽门
                    pingBiMenMgr.AddDevice2ShangXingPingBiMenList(deviceCom);
                }
            }
            #endregion
        }
    }
    #endregion

    #region 读取场景中的列车
    public static void BuildStationTrains(StationMgr stationMgr)
    {
        if (stationMgr == null) return;
        GameObject go = GameObject.Find("Train/TrainRoot");
        if (go == null) return;
        Transform trainRootTrans = go.transform;
        if (trainRootTrans == null) return;
        int stationCount = trainRootTrans.childCount;
        for (int i = 0; i < stationCount; ++i)
        {
            Transform stationTrans = trainRootTrans.GetChild(i);
            System.UInt16 stationIndex = (System.UInt16)System.Enum.Parse(typeof(StationType), stationTrans.gameObject.name);
            Station station = stationMgr.GetStation(stationIndex);
            if (station == null) return;
            QQQ(stationMgr, station, stationTrans, stationIndex);
        }
    }
    private static void QQQ(StationMgr stationMgr, Station station, Transform stationTrans, System.UInt16 stationIndex)
    {
        station.UpTrain = stationTrans.Find("Up Train")?.GetComponent<Train>();
        TrainLineInfo info = TDFramework.SingletonMgr.GameGlobalInfo.AllTrainLineInfo.GetTrainLineInfo("Station" + stationIndex + "ShangXingLine");
        if(info == null) return;
        station.UpTrain.StationIndex = stationIndex;
        station.UpTrain.PositiveDirMarkPosition = new Vector3(info.PositiveMarkPosX, info.PositiveMarkPosY, info.PositiveMarkPosZ);
        station.UpTrain.PositiveDirYAngle = info.PositiveYAngle;
        station.UpTrain.PositiveDirMarkPosition = new Vector3(info.NegativeMarkPosX, info.NegativeMarkPosY, info.NegativeMarkPosZ);
        station.UpTrain.NegativeDirYAngle = info.NegativeYAngle;

        station.DownTrain = stationTrans.Find("Down Train")?.GetComponent<Train>();
        info = TDFramework.SingletonMgr.GameGlobalInfo.AllTrainLineInfo.GetTrainLineInfo("Station" + stationIndex + "XiaXingLine");   
        if(info == null) return;     
        station.DownTrain.StationIndex = stationIndex;
        station.DownTrain.PositiveDirMarkPosition = new Vector3(info.PositiveMarkPosX, info.PositiveMarkPosY, info.NegativeMarkPosZ);
        station.DownTrain.PositiveDirYAngle = info.PositiveYAngle;
        station.DownTrain.NegativeDirMarkPosition = new Vector3(info.NegativeMarkPosX, info.NegativeMarkPosY, info.NegativeMarkPosZ);
        station.DownTrain.NegativeDirYAngle = info.NegativeYAngle;
    }
    #endregion
}
