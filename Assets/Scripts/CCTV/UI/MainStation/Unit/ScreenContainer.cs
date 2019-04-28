using System.Collections;
using System.Collections.Generic;
using TDFramework;
using UnityEngine;
using UnityEngine.UI;

public class ScreenContainer : MonoBehaviour {
    #region 数据字段
    //横向大屏数量，初始化默认为1
    private int m_screenXCount = 0;
    //纵向大屏数量, 初始化默认为1
    private int m_screenYCount = 0;
    //当前Container中所包含的RawScreen游戏物体
    private List<ScreenCom> m_childScreenComList = new List<ScreenCom> ();
    #endregion

    #region 组件字段
    private GridLayoutGroup m_gridLayoutGroup;
    private RawImage m_rawImage;
    private ScreenCom m_screenCom;
    #endregion

    #region 属性
    public List<ScreenCom> ChildScreenComList {
        get { return m_childScreenComList; }
    }
    public int Count {
        get { return m_childScreenComList.Count; }
    }
    #endregion

    #region Unity生命周期
    void Awake () {
        m_gridLayoutGroup = transform.GetComponent<GridLayoutGroup> ();
        m_rawImage = transform.GetComponent<RawImage> ();
        m_screenCom = transform.GetComponent<ScreenCom> ();
    }
    #endregion

    #region 方法
    public bool SetGridLayoutGroupConstraintColumnCount (float screenTotalWidth, float screenTotalHeight, int columnCount, int rowCount) {
        //检测是否更改了屏幕布局
        if (IsChangedScreenLayout (columnCount, rowCount) == false) return false;
        //更改了屏幕布局
        m_screenXCount = columnCount;
        m_screenYCount = rowCount;
        m_gridLayoutGroup.constraintCount = columnCount;
        float width = screenTotalWidth / columnCount;
        float height = screenTotalHeight / rowCount;
        m_gridLayoutGroup.cellSize = new Vector2 (width, height);
        return true;
    }
    public void CreateChildScreen (int childScreenCount, string prefabName) {
        for (int i = 0; i < childScreenCount; i++) {
            GameObject go = TDFramework.SingletonMgr.ObjectManager.Instantiate (prefabName, false);
            go.transform.SetParent (transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;
            ScreenCom com = go.GetComponent<ScreenCom> ();
            if (com != null) {
                com.Clear ();
                m_childScreenComList.Add (com);
            }
        }
    }
    public void DestroyChildScreen () {
        int childCount = m_childScreenComList.Count;
        ScreenCom tempScreenCom = null;
        GameObject tempGo = null;
        for (int i = 0; i < childCount; ++i) {
            tempScreenCom = m_childScreenComList[i];
            tempGo = m_childScreenComList[i].gameObject;
            tempScreenCom.Clear ();
            //回收ScreenCom对应的UI对象
            SingletonMgr.ObjectManager.ReleaseGameObjectItem (tempGo);
            if (tempScreenCom.BindCamera != null) {
                //回收ScreenCom所绑定画面的摄像机对象
                SingletonMgr.ObjectManager.ReleaseGameObjectItem (tempScreenCom.BindCamera.gameObject);
                tempScreenCom.BindCamera = null;

                //从StationMgr中管理的Camera中删除
                StationMgr stationMgr = GameGlobalComponent.StationMgr;
                if (stationMgr == null) return;
                stationMgr.RemoveCamera4CameraDict (tempScreenCom.StationIndex, tempScreenCom.CameraIndex);
            }
        }
        m_childScreenComList.Clear ();
    }
    #endregion

    #region 私有方法
    //是否更改了屏幕布局
    private bool IsChangedScreenLayout (int xCount, int yCount) {
        if (m_screenXCount != xCount || m_screenYCount != yCount) {
            return true;
        }
        return false;
    }
    #endregion

}