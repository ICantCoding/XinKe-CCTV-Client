using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadTxt
{
    #region 字段

    #endregion

    #region 方法
    public static List<Vector3> XXX()
    {
        List<Vector3> list = new List<Vector3>();
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Txt/XX.txt");
        string line = "";
        while((line = sr.ReadLine()) != null)
        {
            string[] strs = line.Split(',');
            list.Add(new Vector3(-(float.Parse(strs[0])), float.Parse(strs[1]), float.Parse(strs[2])));
        }
        return list;
    }
    #endregion
}
