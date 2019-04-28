using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CCTVCameraType
{
    BallMachine,    //球机
    GunMachine,     //枪机

    None,
}

public class Info4Camera : MonoBehaviour
{
    #region 字段
    public CCTVCameraType CctvCameraType;
    #endregion
}
