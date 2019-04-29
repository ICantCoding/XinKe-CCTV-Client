using System;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using TDFramework;
using TDFramework.UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenPanel : UIPanel {
    #region 数据字段
    private List<GameObject> m_bigScreenGoList = new List<GameObject> ();
    #endregion

    #region 组件字段
    //摄像机的根容器
    public Transform m_cameraRootTrans;
    private ScreenContainer m_screenContainer;
    #endregion

    #region Unity生命周期
    protected override void Awake () {
        base.Awake ();
        
        UIScreenPanel_Command command = new UIScreenPanel_Command ();
        RegisterCommand (EventID_Cmd.DivisionBigScreen, command);
        RegisterCommand (EventID_Cmd.DivisionSmallScreen, command);
        RegisterCommand (EventID_Cmd.ScreenBindCamera, command);
        RegisterMediator (new UIScreenPanel_Mediator ()); //注册Mediator
        RegisterMediator (this); //注册UIPanelMediator
        RegisterProxy (new UIScreenPanel_Proxy ()); //注册Proxy

        m_screenContainer = transform.Find ("ScreenContainer").GetComponent<ScreenContainer> ();
        m_cameraRootTrans = GameObject.Find ("Camera").transform;
    }
    void Start () {
        //默认读取AppInfo中的配置，来显示默认分屏
        //应用启动，进入站台界面时，默认是4分屏
        UInt16 xCount = (UInt16) TDFramework.SingletonMgr.GameGlobalInfo.AppInfo.ScreenXCount;
        UInt16 yCount = (UInt16) TDFramework.SingletonMgr.GameGlobalInfo.AppInfo.ScreenYCount;
        DivisionBigScreen (xCount, yCount);
        int count = xCount * yCount;
        CameraInfo cameraInfo = null;
        CameraInfoList list = TDFramework.SingletonMgr.GameGlobalInfo.CameraInfoListDict[5];
        for (int i = 0; i < xCount * yCount; i++) {
            cameraInfo = list.GetCameraInfoByCameraIndex ((UInt16) i);
            ScreenBindCamera (5, cameraInfo.CameraIndex, (UInt16) (i + 1), 0);
        }
    }
    protected void OnDestroy () {
        RemoveCommand (EventID_Cmd.DivisionBigScreen);
        RemoveCommand (EventID_Cmd.DivisionSmallScreen);
        RemoveCommand (EventID_Cmd.ScreenBindCamera);
        RemoveMediator (UIScreenPanel_Mediator.NAME);
        RemoveMediator (this.MediatorName);
        RemoveProxy (UIScreenPanel_Proxy.NAME);
    }
    #endregion

    #region Mediator功能实现
    public override string[] ListNotificationInterests () {
        return new string[] {
            EventID_UI.DivisionBigScreen,
                EventID_UI.DivisionSmallScreen,
                EventID_UI.ScreenBindCamera,
        };
    }
    public override void HandleNotification (INotification notification) {
        switch (notification.Name) {
            case EventID_UI.DivisionBigScreen:
                {
                    DivisionBigScreen_Callback (notification);
                    break;
                }
            case EventID_UI.DivisionSmallScreen:
                {
                    DivisionSmallScreen_Callback (notification);
                    break;
                }
            case EventID_UI.ScreenBindCamera:
                {
                    ScreenBindCamera_Callback (notification);
                    break;
                }
            default:
                break;
        }
    }
    public override string MediatorName {
        get { return "UIScreenPanel"; }
    }
    private void DivisionBigScreen_Callback (INotification notification) {
        DivisionBigScreen divisionBigScreen = (DivisionBigScreen) notification.Body;
        UInt16 x = divisionBigScreen.m_bigScreenXDivisionCount;
        UInt16 y = divisionBigScreen.m_bigScreenYDivisionCount;
        if (x < 1 || y < 1) return;
        DivisionBigScreen (x, y);
    }
    private void DivisionSmallScreen_Callback (INotification notification) {
        DivisionSmallScreen divisionSmallScreen = (DivisionSmallScreen) notification.Body;
        System.UInt16 bigScreenIndex = divisionSmallScreen.m_bigScreenIndex;
        System.UInt16 x = divisionSmallScreen.m_smallScreenXDivisionCount;
        System.UInt16 y = divisionSmallScreen.m_smallScreenYDivisionCount;
        if (bigScreenIndex < 1 || x < 1 || y < 1) return;
        ScreenCom screenCom = m_screenContainer.ChildScreenComList[bigScreenIndex - 1];
        DivisionSmallScreen (x, y, screenCom);
    }
    private void ScreenBindCamera_Callback (INotification notification) {
        ScreenBindCamera response = (ScreenBindCamera) notification.Body;
        if (response == null) return;
        UInt16 stationIndex = response.m_stationIndex;
        UInt16 cameraIndex = response.m_cameraIndex;
        UInt16 bigScreenIndex = response.m_bigScreenIndex;
        UInt16 smallScreenIndex = response.m_smallScreenIndex;
        string cameraName = response.m_cameraName;
        ScreenBindCamera (stationIndex, cameraIndex, bigScreenIndex, smallScreenIndex);
    }
    #endregion

    #region 方法
    //大屏分屏
    private void DivisionBigScreen (UInt16 xCount, UInt16 yCount) {
        if (xCount < 1 || yCount < 1) return;
        bool status = m_screenContainer.SetGridLayoutGroupConstraintColumnCount (Screen.width, Screen.height, xCount, yCount);
        if (status) {
            //大屏需要切换屏幕布局之前，先要检测大屏下是否有小屏，如果有小屏需要清理小屏数据
            ScreenCom tempScreenCom = null;
            ScreenContainer tempScreenContainer = null;
            for (int i = 0; i < m_screenContainer.Count; ++i) {
                tempScreenCom = m_screenContainer.ChildScreenComList[i];
                if (tempScreenCom != null) {
                    tempScreenContainer = tempScreenCom.GetComponent<ScreenContainer> ();
                    for (int j = 0; j < tempScreenContainer.Count; ++j) {
                        tempScreenContainer.DestroyChildScreen ();
                    }
                }
            }
            //大屏需要切换屏幕布局， 先删除以前的布局，再创建新布局
            m_screenContainer.DestroyChildScreen ();
            m_screenContainer.CreateChildScreen (xCount * yCount, StringMgr.RawBigScreen);
        }
    }
    //小屏分屏
    private void DivisionSmallScreen (UInt16 xCount, UInt16 yCount, ScreenCom screenCom) {
        if (screenCom != null) {
            //大屏划分小屏的时候，先要清除大屏自身绑定的Camera游戏对象后，再开始切分为小屏
            screenCom.DestroySelfBindCamera ();
            RectTransform rectTrans = screenCom.GetComponent<RectTransform> ();
            ScreenContainer screenContainer = screenCom.GetComponent<ScreenContainer> ();
            bool status = screenContainer.SetGridLayoutGroupConstraintColumnCount (rectTrans.rect.size.x, rectTrans.rect.size.y, xCount, yCount);
            if (status) {
                //小屏需要切换屏幕布局，先删除之前的布局，再创建新的布局
                screenContainer.DestroyChildScreen ();
                screenContainer.CreateChildScreen (xCount * yCount, StringMgr.RawSmallScreen);
            }
        }
    }
    //分屏绑定Camera视图数据
    private void ScreenBindCamera (UInt16 p_stationIndex, UInt16 p_cameraIndex, UInt16 p_bigScreenIndex,
        UInt16 p_smallScreenIndex) {
        //数据
        UInt16 stationIndex = p_stationIndex;
        UInt16 cameraIndex = p_cameraIndex;
        UInt16 bigScreenIndex = p_bigScreenIndex;
        UInt16 smallScreenIndex = p_smallScreenIndex;
        CameraInfoList cameraInfoList = TDFramework.SingletonMgr.GameGlobalInfo.CameraInfoListDict[stationIndex];
        CameraInfo cameraInfo = cameraInfoList.GetCameraInfoByCameraIndex (cameraIndex);

        //如果分屏数小于大屏索引数，表示数据有错误，直接返回
        if (m_screenContainer.Count < bigScreenIndex)
            return;
        ScreenCom bigScreenCom = m_screenContainer.ChildScreenComList[bigScreenIndex - 1];
        if (bigScreenCom == null) return;
        ScreenContainer bigScreenContainer = bigScreenCom.GetComponent<ScreenContainer> ();

        Camera tempCamera = null;
        ScreenCom tempScreenCom = null;
        string tempTexturePath = "";
        RenderTexture renderTexture = null;
        if (smallScreenIndex == 0) {
            //表示该摄像头绑定到大屏
            tempTexturePath = "Assets/Res/RenderTexture/New Render Texture " +
                bigScreenIndex.ToString () + " 1.renderTexture";
            if (bigScreenCom.BindCamera != null) {
                tempCamera = bigScreenCom.BindCamera;
            }
            tempScreenCom = bigScreenCom;
        } else {
            //表示绑定Camera到小屏
            //如果分屏数小于小屏索引数，表示数据有错误，直接返回
            if (bigScreenContainer == null || bigScreenContainer.Count < smallScreenIndex)
                return;
            //表示该摄像头绑定到指定大屏中的指定小屏
            tempTexturePath = "Assets/Res/RenderTexture/New Render Texture " +
                bigScreenIndex.ToString () + " " + smallScreenIndex.ToString () + ".renderTexture";
            ScreenCom smallScreenCom = bigScreenContainer.ChildScreenComList[smallScreenIndex - 1];
            if (smallScreenCom == null) return;
            if (smallScreenCom.BindCamera != null) {
                tempCamera = smallScreenCom.BindCamera;
            }
            tempScreenCom = smallScreenCom;
        }
        if (tempCamera == null) {
            //实例化摄像机并设置相机参数
            GameObject cameraGo = TDFramework.SingletonMgr.ObjectManager.Instantiate (StringMgr.CCTVCamera, false);
            if (cameraGo == null) return;
            if (m_cameraRootTrans != null) {
                cameraGo.transform.SetParent (m_cameraRootTrans);
            }
            tempCamera = cameraGo.GetComponent<Camera> ();
            if (tempCamera == null) return;
        }
        tempCamera.transform.localPosition = new Vector3 (cameraInfo.PositionX, cameraInfo.PositionY, cameraInfo.PositionZ);
        tempCamera.transform.localEulerAngles = new Vector3 (cameraInfo.EulerAngleX, cameraInfo.EulerAngleY, cameraInfo.EulerAngleZ);
        tempCamera.nearClipPlane = cameraInfo.ClippingPlanesNear;
        tempCamera.farClipPlane = cameraInfo.ClippingPlanesFar;

        renderTexture = ResourceMgr.Instance.LoadAsset<RenderTexture> (tempTexturePath);
        if (renderTexture == null) return;
        StationMgr stationMgr = GameGlobalComponent.StationMgr;
        if (stationMgr == null) return;
        stationMgr.RemoveCamera4CameraDict(stationIndex, tempScreenCom.CameraIndex);
        tempCamera.targetTexture = renderTexture;
        tempScreenCom.SetContent (tempCamera, renderTexture, cameraInfo.CameraName);
        tempScreenCom.BindCamera = tempCamera;
        tempScreenCom.CameraIndex = cameraIndex;
        tempScreenCom.StationIndex = stationIndex;
        stationMgr.AddCamera2CameraDict (stationIndex, cameraIndex, tempCamera);
    }
    #endregion
}