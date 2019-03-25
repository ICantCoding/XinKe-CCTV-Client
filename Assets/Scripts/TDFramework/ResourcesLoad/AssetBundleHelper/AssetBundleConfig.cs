

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Xml.Serialization;

    [System.Serializable]
    public class AssetBundleConfig
    {
        [XmlElement("ABList")]
        public List<ABBase> ABList { get; set; }


        public override string ToString(){
            string content = "";
            for(int i = 0; i < ABList.Count; i++){
                ABBase abbase = ABList[i];
                content += abbase.Path + ", ";
                content += abbase.Crc + ", ";
                content += abbase.ABName + ", ";
                content += abbase.AssetName + ", ";
                content += "Depend: ";
                for(int j = 0; j < abbase.DependABList.Count; j++){
                    content += abbase.DependABList[j] + ", ";
                }
            }
            return content;
        }
    }

    [System.Serializable]
    public class ABBase
    {
        [XmlAttribute("Path")]
        public string Path { get; set; } //Asset/相对路径
        [XmlAttribute("Crc")]
        public uint Crc { get; set; } //Path所对应的Crc值
        [XmlAttribute("ABName")]
        public string ABName { get; set; } //包名
        [XmlAttribute("AssetName")]
        public string AssetName { get; set; } //资源名
        [XmlElement("DependABList")]
        public List<string> DependABList { get; set; } //文件所依赖的其他AB包
    }
}

