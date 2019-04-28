
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class TrainLineInfo
{
    //曲线点位文本名称
    [XmlElement("LineInfoTextFileName")]
    public string LineInfoTextFileName;
    //对应站台索引
    [XmlElement("StationIndex")]
    public int StationIndex;
    //线名称
    [XmlElement("LineName")]
    public string LineName;
    //正向对标点
    [XmlElement("PositiveMarkPosX")]
    public float PositiveMarkPosX;
    [XmlElement("PositiveMarkPosY")]
    public float PositiveMarkPosY;
    [XmlElement("PositiveMarkPosZ")]
    public float PositiveMarkPosZ;
    //正向对标Y轴旋转
    [XmlElement("PositiveYAngle")]
    public float PositiveYAngle;
    //反向对标点
    [XmlElement("NegativeMarkPosX")]
    public float NegativeMarkPosX;
    [XmlElement("NegativeMarkPosY")]
    public float NegativeMarkPosY;
    [XmlElement("NegativeMarkPosZ")]
    public float NegativeMarkPosZ;
    //反向对标Y轴旋转
    [XmlElement("NegativeYAngle")]
    public float NegativeYAngle;
    //线总长度
    [XmlElement("LineLength")]
    public float LineLength;
    //正向对标距离
    [XmlElement("PositiveMarkDistance")]
    public float PositiveMarkDistance;
    //反向对标距离
    [XmlElement("NegativeMarkDistance")]
    public float NegativeMarkDistance;
}

public class AllTrainLineInfo
{
    [XmlElement("TrainLineInfo")]
    public List<TrainLineInfo> list = new List<TrainLineInfo>();

    public TrainLineInfo GetTrainLineInfo(string lineName)
    {
        TrainLineInfo info = null;
        var enumerator = list.GetEnumerator();
        while(enumerator.MoveNext())
        {
            if(enumerator.Current.LineName == lineName)
            {
                info = enumerator.Current;
            }
        }
        enumerator.Dispose();
        return info;
    }

    //序列化
    public static void SerializeTrainLineInfo2Xml()
    {
        AllTrainLineInfo allTrainLineInfo = new AllTrainLineInfo();

        #region 赵营站台
        //下行曲线
        TrainLineInfo info = new TrainLineInfo();
        info.LineInfoTextFileName = "XiaXingTrainLineInfoStation5.txt";
        info.StationIndex = 5;
        info.LineName = "Station5XiaXingLine";
        //正向对标点 3881.429,-42.911,-10213.583
        info.PositiveMarkPosX = -3881.429f;
        info.PositiveMarkPosY = -42.911f;
        info.PositiveMarkPosZ = -10213.583f;
        info.PositiveYAngle = 68.5f;
        //反向对标点 3917.842,-42.911,-10227.551
        info.NegativeMarkPosX = -3917.842f;
        info.NegativeMarkPosY = -42.911f;
        info.NegativeMarkPosZ = -10227.551f;
        info.NegativeYAngle = 68.5f;
        info.LineLength = GetLineLength(info.LineInfoTextFileName);
        info.PositiveMarkDistance = GetMarkLength(info.LineInfoTextFileName, new Vector3(info.PositiveMarkPosX, info.PositiveMarkPosY, info.PositiveMarkPosZ));
        info.NegativeMarkDistance = GetMarkLength(info.LineInfoTextFileName, new Vector3(info.NegativeMarkPosX, info.NegativeMarkPosY, info.NegativeMarkPosZ));
        allTrainLineInfo.list.Add(info);
        //上行曲线
        info = new TrainLineInfo();
        info.LineInfoTextFileName = "ShangXingTrainLineInfoStation5.txt";
        info.StationIndex = 5;
        info.LineName = "Station5ShangXingLine";
        //正向对标点 3997.823,-42.91198617,-10244.054
        info.PositiveMarkPosX = -3997.823f;
        info.PositiveMarkPosY = -42.91198617f;
        info.PositiveMarkPosZ = -10244.054f;
        info.PositiveYAngle = -111.75f;        
        //反向对标点 3961.462,-42.91199997,-10229.751
        info.NegativeMarkPosX = -3961.462f;
        info.NegativeMarkPosY = -42.91199997f;
        info.NegativeMarkPosZ = -10229.751f;
        info.NegativeYAngle = -111.75f;
        info.LineLength = GetLineLength(info.LineInfoTextFileName);
        info.PositiveMarkDistance = GetMarkLength(info.LineInfoTextFileName, new Vector3(info.PositiveMarkPosX, info.PositiveMarkPosY, info.PositiveMarkPosZ));
        info.NegativeMarkDistance = GetMarkLength(info.LineInfoTextFileName, new Vector3(info.NegativeMarkPosX, info.NegativeMarkPosY, info.NegativeMarkPosZ));
        allTrainLineInfo.list.Add(info);
        #endregion
        

        if (File.Exists(AppConfigPath.TrainLineInfoXmlPath))
        {
            File.Delete(AppConfigPath.TrainLineInfoXmlPath);
        }

        FileStream fileStream = new FileStream(AppConfigPath.TrainLineInfoXmlPath,
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(allTrainLineInfo.GetType());
        xml.Serialize(sw, allTrainLineInfo);
        sw.Close();
        fileStream.Close();
    }
    public static AllTrainLineInfo DeserializeTrainLineInfoFromXml()
    {
        FileStream fs = new FileStream(AppConfigPath.TrainLineInfoXmlPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(AllTrainLineInfo));
        AllTrainLineInfo allTrainLineInfo = (AllTrainLineInfo)xml.Deserialize(fs);
        fs.Close();
        return allTrainLineInfo;
    }


    #region 私有方法
    private static float GetLineLength(string lineInfoFileName)
    {
        List<Vector3> list = new List<Vector3>();
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Txt/" + lineInfoFileName);
        string line = "";
        while ((line = sr.ReadLine()) != null)
        {
            string[] strs = line.Split(',');
            list.Add(new Vector3(-(float.Parse(strs[0])), float.Parse(strs[1]), float.Parse(strs[2])));
        }
        float len = 0.0f;
        for (int i = 0; i < list.Count - 1; i++)
        {
            len += Vector3.Distance(list[i], list[i + 1]);
        }
        return len;
    }
    private static float GetMarkLength(string lineInfoFileName, Vector3 markPosition)
    {
        List<Vector3> list = new List<Vector3>();
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Txt/" + lineInfoFileName);
        string line = "";
        while ((line = sr.ReadLine()) != null)
        {
            string[] strs = line.Split(',');
            list.Add(new Vector3(-(float.Parse(strs[0])), float.Parse(strs[1]), float.Parse(strs[2])));
        }
        float len = 0.0f;
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (Vector3.Distance(list[i], markPosition) < 0.01f)
            {
                break;
            }
            len += Vector3.Distance(list[i], list[i + 1]);
        }
        return len;
    }
    #endregion
}
