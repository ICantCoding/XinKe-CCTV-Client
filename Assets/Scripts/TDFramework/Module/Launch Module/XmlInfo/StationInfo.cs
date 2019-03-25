using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class StationInfo
{
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


