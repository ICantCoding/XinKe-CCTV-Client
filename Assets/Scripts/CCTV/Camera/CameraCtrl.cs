using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {
    #region 字段
    private Camera m_camera;
    #endregion

    #region Unity生命周期
    void Awake () {
        m_camera = GetComponent<Camera> ();
    }
    #endregion

    #region 方法
    public void RotationAction (byte directionFlag) {
        switch (directionFlag) {
            case 0:
                {
                    LeftRotationAction ();
                    break;
                }
            case 1:
                {
                    RightRotationAction ();
                    break;
                }
            case 2:
                {
                    UpRotationAction ();
                    break;
                }
            case 3:
                {
                    DownRotationAction ();
                    break;
                }
            default:
                break;
        }
    }
    //向左旋转
    public void LeftRotationAction () {
        transform.Rotate(0, -2.0f, 0, Space.World);
    }
    //向右旋转
    public void RightRotationAction () {
        transform.Rotate(0, 2.0f, 0, Space.World);
    }
    //向上旋转
    public void UpRotationAction () {
        transform.Rotate(1f, 0, 0, Space.World);
    }
    //向下旋转
    public void DownRotationAction () {
        transform.Rotate(-1f, 0, 0, Space.World);
    }
    public void ZoomAction (byte zoomFlag) {
        switch (zoomFlag) {
            case 0:
                {
                    ZoomDownAction ();
                    break;
                }
            case 1:
                {
                    ZoomUpAction ();
                    break;
                }
            default:
                break;
        }
    }
    //放大画面
    public void ZoomUpAction () {
        float fov = m_camera.fieldOfView;
        fov += 1.0f;
        fov = Mathf.Clamp (fov, 30.0f, 80.0f);
        m_camera.fieldOfView = fov;
    }
    //缩小画面
    public void ZoomDownAction () {
        float fov = m_camera.fieldOfView;
        fov -= 1.0f;
        fov = Mathf.Clamp (fov, 30.0f, 80.0f);
        m_camera.fieldOfView = fov;
    }
    #endregion
}