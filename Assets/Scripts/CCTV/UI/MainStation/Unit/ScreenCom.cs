using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCom : MonoBehaviour {
    #region 组件字段
    //ScreenCom所绑定画面对应的Camer的所在站台的Index
    private UInt16 m_stationIndex;
    //ScreenCom所绑定画面对应的Camera的Index
    private UInt16 m_cameraIndex;
    //ScreenCom所绑定画面对应的Camera
    private Camera m_bindCamera;
    private RectTransform m_rectTrans;
    private RawImage m_rawImage;
    private Text m_cameraNameText;
    private Text m_timeText;
    private Color m_grayColor = new Color (36.0f / 255.0f, 32.0f / 255.0f, 32.0f / 255.0f, 1.0f);
    private Color m_whiteColor = Color.white;
    #endregion

    #region 属性
    public UInt16 StationIndex {
        get { return m_stationIndex; }
        set { m_stationIndex = value; }
    }
    public UInt16 CameraIndex {
        get { return m_cameraIndex; }
        set { m_cameraIndex = value; }
    }
    public Camera BindCamera {
        get { return m_bindCamera; }
        set { m_bindCamera = value; }
    }
    public RawImage RawImage {
        get { return m_rawImage; }
    }
    public Text CameraNameText {
        get { return m_cameraNameText; }
    }
    public Text TimeText {
        get { return m_timeText; }
    }
    #endregion

    #region Unity生命周期
    void Awake () {
        m_rectTrans = transform.GetComponent<RectTransform> ();
        m_rawImage = transform.GetComponent<RawImage> ();
        m_cameraNameText = transform.Find ("CameraNameTxt").GetComponent<Text> ();
        m_timeText = transform.Find ("TimeTxt").GetComponent<Text> ();
    }
    #endregion

    #region 方法
    public void SetContent (Camera pCamera, RenderTexture texture, string cameraName) {
        SetCamraAspect (pCamera);
        SetRawImage (texture);
        SetCameraName (cameraName);
        StartTimeCount ();
    }
    public void Clear () {
        StopAllCoroutines ();
        if (m_rawImage != null) {
            m_rawImage.color = m_grayColor;
            m_rawImage.texture = null;
        }
        if (m_cameraNameText != null)
            m_cameraNameText.text = "";
        if (m_timeText != null)
            m_timeText.text = "";
    }
    public void DestroySelfBindCamera () {
        if (BindCamera != null) {
            //回收ScreenCom所绑定画面的摄像机对象
            TDFramework.SingletonMgr.ObjectManager.ReleaseGameObjectItem (BindCamera.gameObject);
            BindCamera = null;
        }
    }
    #endregion

    #region 私有方法
    //设置摄像机纵横比，避免画面变形
    private void SetCamraAspect (Camera pCamera) {
        if (pCamera != null && m_rectTrans != null)
            pCamera.aspect = m_rectTrans.rect.x / m_rectTrans.rect.y;
    }
    private void SetRawImage (RenderTexture texture) {
        if (m_rawImage != null) {
            m_rawImage.texture = texture;
            m_rawImage.color = m_whiteColor;
        }
    }
    private void SetCameraName (string cameraName) {
        if (m_cameraNameText != null)
            m_cameraNameText.text = cameraName;
    }
    private void StartTimeCount () {
        StartCoroutine (TimeCount ());
    }
    IEnumerator TimeCount () {
        while (true) {
            m_timeText.text = DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
            yield return new WaitForSeconds (1.0f);
        }
    }
    #endregion
}