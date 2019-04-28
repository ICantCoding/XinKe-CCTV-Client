using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomHandle : BaseHandle
{
    #region 字段

    #endregion

    #region 构造函数
    public CameraZoomHandle(BaseNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "CameraZoomHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        CameraZoom response = new CameraZoom(packet.m_data);
        if (response == null) return;
        Debug.Log("接收到CameraZoom消息.");
        //站台索引
        UInt16 stationIndex = response.m_stationIndex;
        //摄像机索引
        UInt16 cameraIndex = response.m_cameraIndex;
        //0缩小,1放大
        byte zoomUpOrDown = response.m_zoomUpOrDown;

        Camera curCamera = GameGlobalComponent.StationMgr.GetCameraByCameraIndex(stationIndex, cameraIndex);
        if(curCamera != null)
        {
            CameraCtrl cameraCtrl = curCamera.GetComponent<CameraCtrl>();
            if(cameraCtrl != null)
            {
                cameraCtrl.ZoomAction(zoomUpOrDown);
            }
        } 
    }
    #endregion
}
