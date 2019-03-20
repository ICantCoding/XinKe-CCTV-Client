using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class StationInfo
{
    //上行上车Id， 用于网络连接Id标识
    [XmlElement("ShangXingShangCheId")]
    public UInt16 ShangXingShangCheId;
    //上行下车Id, 用于网络连接Id标识
    [XmlElement("ShangXingXiaCheId")]
    public UInt16 ShangXingXiaCheId;
    //下行上车Id, 用于网络连接Id标识
    [XmlElement("XiaXingShangCheId")]
    public UInt16 XiaXingShangCheId;
    //下行下车Id, 用于网络连接Id标识
    [XmlElement("XiaXingXiaCheId")]
    public UInt16 XiaXingXiaCheId;
    [XmlElement("Index")]
    public UInt16 Index;
    [XmlElement("Name")]
    public string Name;
    [XmlElement("EngName")]
    public string EngName;
}

public class StationInfoList
{
    [XmlElement("StationInfo")]
    public List<StationInfo> stationInfoList = new List<StationInfo>();

    #region 方法
    public StationInfo GetStationInfo(UInt16 index)
    {
        int count = stationInfoList.Count;
        for(int i = 0; i < count; ++i)
        {
            if(stationInfoList[i].Index == index)
            {
                return stationInfoList[i];
            }
        }
        return null;
    }
    public StationInfo GetStationInfo(string name)
    {
        int count = stationInfoList.Count;
        for(int i = 0; i < count; ++i)
        {
            if(stationInfoList[i].Name == name || stationInfoList[i].EngName == name)
            {
                return stationInfoList[i];
            }
        }
        return null;
    }
    #endregion

    #region 序列化，反序列化方法
    public static void SerializeStationInfoList2Xml()
    {
        StationInfoList list = new StationInfoList();

        StationInfo station = new StationInfo();
        station.ShangXingShangCheId = 1001;
        station.ShangXingXiaCheId = 1002;
        station.XiaXingShangCheId = 1003;
        station.XiaXingXiaCheId = 1004;
        station.Index = 0;
        station.Name = "赵营";
        station.EngName = "ZhaoYing";
        list.stationInfoList.Add(station);

        if (File.Exists(AppConfigPath.StationInfoXmlPath))
        {
            File.Delete(AppConfigPath.StationInfoXmlPath);
        }

        FileStream fileStream = new FileStream(AppConfigPath.StationInfoXmlPath,
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(list.GetType());
        xml.Serialize(sw, list);
        sw.Close();
        fileStream.Close();
    }
    public static StationInfoList DeserializeStationInfoListFromXml()
    {
        FileStream fs = new FileStream(AppConfigPath.StationInfoXmlPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(StationInfoList));
        StationInfoList stationInfoList = (StationInfoList)xml.Deserialize(fs);
        fs.Close();
        return stationInfoList;
    }
    #endregion
}


