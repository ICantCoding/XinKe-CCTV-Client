using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerInfo
{
    #region 字段
    [XmlElement("Id")]
    public UInt16 Id;
    [XmlElement("Name")]
    public string Name;
    [XmlElement("ServerIpAddress")]
    public string ServerIpAddress;
    [XmlElement("ServerPort")]
    public int ServerPort;
    #endregion

    #region 方法
    public static void SerializePlayerInfo2Xml()
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.Id = 100;
        playerInfo.Name = "U3D客户端1";
        playerInfo.ServerIpAddress = "127.0.0.1";
        playerInfo.ServerPort = 3322;

        if(File.Exists(AppConfigPath.PlayerInfoXmlPath))
        {
            File.Delete(AppConfigPath.PlayerInfoXmlPath);
        }

        FileStream fileStream = new FileStream(AppConfigPath.PlayerInfoXmlPath,
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(playerInfo.GetType());
        xml.Serialize(sw, playerInfo);
        sw.Close();
        fileStream.Close();
    }
    public static PlayerInfo DeserializePlayerInfoFromXml()
    {
        FileStream fs = new FileStream(AppConfigPath.PlayerInfoXmlPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(PlayerInfo));
        PlayerInfo playerInfo = (PlayerInfo)xml.Deserialize(fs);
        fs.Close();
        return playerInfo;
    }
    #endregion
}
