/********************************************************************************
** Coder：    ???

** 创建时间： 2019-03-07 10:14:07

** 功能描述:  ???

** version:   v1.2.0

*********************************************************************************/
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
            System.UInt16 stationIndex = (System.UInt16)System.Enum.Parse(typeof(StationType), stationTrans.gameObject.name);
            Station station = stationMgr.GetStation(stationIndex);
            if (station == null) return;
            // DeviceMgr deviceMgr = HHH(stationMgr, stationTrans, stationIndex);
            // station.AddDeviceMgr(deviceMgr);
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
            deviceMgr.AddDevice(deviceCom);

            #region 设备关联Point
            string name = deviceTrans.gameObject.name;
            string[] str = name.Split('|');
            //管理上行和下行屏蔽门
            if (str[0] == DeviceType.PingBiMen.ToString())
            {
                PingBiMenMgr pingBiMenMgr = (PingBiMenMgr)deviceMgr;
                if (str[1] == "WaitTrain_Down")
                {
                    //下行屏蔽门
                    pingBiMenMgr.AddDevice2XiaXingPingBiMenList(deviceCom);
                }
                else if (str[1] == "WaitTrain_Up")
                {
                    //上行屏蔽门
                    pingBiMenMgr.AddDevice2ShangXingPingBiMenList(deviceCom);
                }
            }
            #endregion
        }
    }
    #endregion
}
