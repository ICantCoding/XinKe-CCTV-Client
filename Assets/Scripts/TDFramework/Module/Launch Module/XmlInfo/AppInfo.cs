using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

/* 
        ExclusiveFullScreen = 0,
        FullScreenWindow = 1,
        MaximizedWindow = 2,
        Windowed = 3
*/

public class AppInfo
{
    //屏幕宽
    [XmlElement("ScreenWidth")]
    public int ScreenWidth;
    //屏幕高
    [XmlElement("ScreenHeight")]
    public int ScreenHeight;
    //全屏模式
    [XmlElement("FullMode")]
    public int FullMode;
    //屏幕刷新频率，默认为0
    [XmlElement("ScreenRefreshRate")]
    public int ScreenRefreshRate;
    //默认大屏分屏横向个数
    [XmlElement("ScreenXCount")]
    public int ScreenXCount;
    //默认小屏分屏纵向个数
    [XmlElement("ScreenYCount")]
    public int ScreenYCount;

    #region 方法
    public static void SerializeAppInfo2Xml()
    {
        AppInfo appInfo = new AppInfo();
        appInfo.ScreenWidth = 1920;
        appInfo.ScreenHeight = 1080;
        appInfo.FullMode = 1;
        appInfo.ScreenRefreshRate = 0; //默认为0
        appInfo.ScreenXCount = 2;
        appInfo.ScreenYCount = 2;

        if (File.Exists(AppConfigPath.AppInfoXmlPath))
        {
            File.Delete(AppConfigPath.AppInfoXmlPath);
        }

        FileStream fileStream = new FileStream(AppConfigPath.AppInfoXmlPath,
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(appInfo.GetType());
        xml.Serialize(sw, appInfo);
        sw.Close();
        fileStream.Close();
    }
    public static AppInfo DeserializeAppInfoFromXml()
    {
        FileStream fs = new FileStream(AppConfigPath.AppInfoXmlPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(AppInfo));
        AppInfo appInfo = (AppInfo)xml.Deserialize(fs);
        fs.Close();
        return appInfo;
    }
    #endregion
}
