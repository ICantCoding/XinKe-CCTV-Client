using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class CameraInfo
{
    [XmlElement("CameraIndex")]
    public UInt16 CameraIndex;
    [XmlElement("CameraName")]
    public string CameraName;

    [XmlElement("CameraType")]
    public CCTVCameraType CameraType;

    [XmlElement("PositionX")]
    public float PositionX;
    [XmlElement("PositionY")]
    public float PositionY;
    [XmlElement("PositionZ")]
    public float PositionZ;

    [XmlElement("EulerAngleX")]
    public float EulerAngleX;
    [XmlElement("EulerAngleY")]
    public float EulerAngleY;
    [XmlElement("EulerAngleZ")]
    public float EulerAngleZ;
    
    [XmlElement("ClippingPlanesNear")]
    public float ClippingPlanesNear;
    [XmlElement("ClippingPlanesFar")]
    public float ClippingPlanesFar;
}

public class CameraInfoList
{
    [XmlElement("CameraInfo")]
    public List<CameraInfo> m_cameraInfoList = new List<CameraInfo>();

    #region 方法
    //根据CameraIndex获取CameraInfo
    public CameraInfo GetCameraInfoByCameraIndex(UInt16 cameraIndex)
    {
        CameraInfo tempCameraInfo = null;
        CameraInfo cameraInfo = null;
        var enumerator = m_cameraInfoList.GetEnumerator();
        while(enumerator.MoveNext())
        {
            cameraInfo = enumerator.Current;
            if(cameraInfo.CameraIndex == cameraIndex)
            {
                tempCameraInfo = cameraInfo;
                break;
            }
        }
        enumerator.Dispose();
        return tempCameraInfo;
    }
    //序列化
    public static void SerializeCameraInfo2Xml(string path, Transform cameraRootTrans)
    {
        if(cameraRootTrans == null) return;

        CameraInfoList cameraInfoList = new CameraInfoList();
        int count = cameraRootTrans.childCount;
        Transform tempTrans = null;
        Camera tempCamera = null;
        CameraInfo cameraInfo = null;
        for(int i = 0; i < count; ++i)
        {
            tempTrans = cameraRootTrans.GetChild(i);
            Info4Camera info4Camera = tempTrans.GetComponent<Info4Camera>();
            tempCamera = tempTrans.GetComponent<Camera>();
            cameraInfo = new CameraInfo();
            cameraInfo.CameraIndex = (UInt16)i;
            cameraInfo.CameraName = tempTrans.gameObject.name;
            cameraInfo.CameraType = info4Camera.CctvCameraType;
            cameraInfo.PositionX = tempTrans.localPosition.x;
            cameraInfo.PositionY = tempTrans.localPosition.y;
            cameraInfo.PositionZ = tempTrans.localPosition.z;
            cameraInfo.EulerAngleX = tempTrans.localEulerAngles.x;
            cameraInfo.EulerAngleY = tempTrans.localEulerAngles.y;
            cameraInfo.EulerAngleZ = tempTrans.localEulerAngles.z;
            cameraInfo.ClippingPlanesNear = tempCamera.nearClipPlane;
            cameraInfo.ClippingPlanesFar = tempCamera.farClipPlane;
            cameraInfoList.m_cameraInfoList.Add(cameraInfo);
        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        FileStream fileStream = new FileStream(path,
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(cameraInfoList.GetType());
        xml.Serialize(sw, cameraInfoList);
        sw.Close();
        fileStream.Close();
    }
    //反序列化
    public static CameraInfoList DeserializeCameraInfoFromXml(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(CameraInfoList));
        CameraInfoList cameraInfoList = (CameraInfoList)xml.Deserialize(fs);
        fs.Close();
        return cameraInfoList;
    }
    #endregion
}
