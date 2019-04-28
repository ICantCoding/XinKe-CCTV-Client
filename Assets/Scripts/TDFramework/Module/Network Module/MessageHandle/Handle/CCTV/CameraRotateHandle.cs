using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateHandle : BaseHandle
{
    #region 字段

    #endregion

    #region 构造函数
    public CameraRotateHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "CameraRotateHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        CameraRotate response = new CameraRotate(packet.m_data);
        if (response == null) return;
        //站台索引
        UInt16 stationIndex = response.m_stationIndex;
        //摄像机索引
        UInt16 cameraIndex = response.m_cameraIndex;
        //旋转控制，0向左，1向右，2向上，3向下
        byte rotateUpOrDown = response.m_rotateUpOrDown;
        Debug.Log("接收到CameraRotate消息." + cameraIndex);

        Camera curCamera = GameGlobalComponent.StationMgr.GetCameraByCameraIndex(stationIndex, cameraIndex);
        CameraInfoList cameraInfoList = TDFramework.SingletonMgr.GameGlobalInfo.CameraInfoListDict[stationIndex];
        CameraInfo cameraInfo = cameraInfoList.GetCameraInfoByCameraIndex (cameraIndex);
        if(curCamera != null)
        {
            CameraCtrl cameraCtrl = curCamera.GetComponent<CameraCtrl>();
            if(cameraCtrl != null && cameraInfo != null && cameraInfo.CameraType == CCTVCameraType.BallMachine)
            {
                cameraCtrl.RotationAction(rotateUpOrDown);
            }
        } 
    }
    #endregion
}
