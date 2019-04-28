using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum TrainType
{
    Up,
    Down
}

public class Train : MonoBehaviour
{
    #region 字段
    //Train所属的站台
    private UInt16 m_stationIndex;
    //Train是上行车，还是下行车
    private TrainType m_trainType;
    //列车左侧车门集合
    private List<TrainDoor> m_leftTrainDoorList = new List<TrainDoor>();
    //列车右侧车门集合
    private List<TrainDoor> m_rightTrainDoorList = new List<TrainDoor>();
    //列车上行轨道点集合
    private List<Vector3> m_upLinePoints = new List<Vector3>();
    //列车下行轨道点集合
    private List<Vector3> m_downLinePoints = new List<Vector3>();
    //正向行车对标点
    private Vector3 m_positiveDirMarkPosition;
    //正向行车Y轴旋转
    private float m_positiveDirYAngle;
    //反向行车对标点
    private Vector3 m_negativeDirMarkPosition;
    //反向行车Y轴旋转
    private float m_negativeDirYAngle;
    #endregion


    #region 属性
    public UInt16 StationIndex
    {
        get { return m_stationIndex; }
        set { m_stationIndex = value; }
    }
    public TrainType TrainType
    {
        get { return m_trainType; }
        set { m_trainType = value; }
    }
    public List<TrainDoor> LeftTrainDoorList
    {
        get { return m_leftTrainDoorList; }
    }
    public List<TrainDoor> RightTrainDoorList
    {
        get { return m_rightTrainDoorList; }
    }
    public Vector3 PositiveDirMarkPosition
    {
        get { return m_positiveDirMarkPosition; }
        set { m_positiveDirMarkPosition = value; }
    }
    public float PositiveDirYAngle
    {
        get { return m_positiveDirYAngle; }
        set { m_positiveDirYAngle = value; }
    }
    public Vector3 NegativeDirMarkPosition
    {
        get { return m_negativeDirMarkPosition; }
        set { m_negativeDirMarkPosition = value; }
    }
    public float NegativeDirYAngle
    {
        get { return m_negativeDirYAngle; }
        set { m_negativeDirYAngle = value; }
    }
    #endregion


    #region Unity生命周期
    void Awake()
    {
        #region 获取列车左侧和右侧的车门集合
        Transform doorsTrans = transform.Find("LeftDoors");
        int doorCount = doorsTrans.childCount;
        TrainDoor tempTrainDoor = null;
        for (int i = 0; i < doorCount; ++i)
        {
            tempTrainDoor = doorsTrans.GetChild(i)?.GetComponent<TrainDoor>();
            if (tempTrainDoor != null)
            {
                m_leftTrainDoorList.Add(tempTrainDoor);
            }
        }
        doorsTrans = transform.Find("RightDoors");
        doorCount = doorsTrans.childCount;
        for (int i = 0; i < doorCount; ++i)
        {
            tempTrainDoor = doorsTrans.GetChild(i)?.GetComponent<TrainDoor>();
            if (tempTrainDoor != null)
            {
                m_rightTrainDoorList.Add(tempTrainDoor);
            }
        }
        #endregion
    }
    void Start()
    {
        #region 解析轨道曲线位置点
        Debug.Log("m_stationIndex: " + m_stationIndex);
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Txt/ShangXingTrainLineInfoStation" + m_stationIndex.ToString() + ".txt");
        string line = "";
        while ((line = sr.ReadLine()) != null)
        {
            string[] strs = line.Split(',');
            m_upLinePoints.Add(new Vector3(-(float.Parse(strs[0])), float.Parse(strs[1]), float.Parse(strs[2])));
        }
        sr = new StreamReader(Application.streamingAssetsPath + "/Txt/XiaXingTrainLineInfoStation" + m_stationIndex.ToString() + ".txt");
        while ((line = sr.ReadLine()) != null)
        {
            string[] strs = line.Split(',');
            m_downLinePoints.Add(new Vector3(-(float.Parse(strs[0])), float.Parse(strs[1]), float.Parse(strs[2])));
        }
        #endregion

        //初始化列车方向
        Vector3 eulerAngle = transform.eulerAngles;
        eulerAngle.y = PositiveDirYAngle;
        transform.localEulerAngles = eulerAngle;

        #region 调试画线
        // if (TrainType == TrainType.Up)
        // {
        //     LineRenderer lineRenderer = transform.GetComponent<LineRenderer>();
        //     lineRenderer.positionCount = m_upLinePoints.Count;
        //     lineRenderer.SetPositions(m_upLinePoints.ToArray());
        // }
        // else if (TrainType == TrainType.Down)
        // {
        //     LineRenderer lineRenderer = transform.GetComponent<LineRenderer>();
        //     lineRenderer.positionCount = m_downLinePoints.Count;
        //     lineRenderer.SetPositions(m_downLinePoints.ToArray());
        // }
        #endregion

        #region 调试跑车
        // StartCoroutine(Test());
        #endregion
    }
    #endregion

    #region 方法
    public void OpenLeftDoors()
    {
        var enumerator = m_leftTrainDoorList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.OpenDoor();
        }
        enumerator.Dispose();
    }
    public void CloseLeftDoors()
    {
        var enumerator = m_leftTrainDoorList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.CloseDoor();
        }
        enumerator.Dispose();
    }
    public void OpenRightDoors()
    {
        var enumerator = m_rightTrainDoorList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.OpenDoor();
        }
        enumerator.Dispose();
    }
    public void CloseRightDoors()
    {
        var enumerator = m_rightTrainDoorList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.CloseDoor();
        }
        enumerator.Dispose();
    }
    public void SetPositionByMarkLength(float markLength)
    {
        List<Vector3> list = null;
        if (TrainType == TrainType.Up)
        {
            list = m_upLinePoints;
        }
        else if (TrainType == TrainType.Down)
        {
            list = m_downLinePoints;
        }
        float len = 0;
        for (int i = 0; i < list.Count - 1; i++)
        {
            len += Vector3.Distance(list[i], list[i + 1]);
            if (len > markLength)
            {
                float offset = 0.0f;
                offset = markLength - (len - Vector3.Distance(list[i], list[i + 1]));
                Vector3 normal = (list[i + 1] - list[i]).normalized;
                Vector3 localPosition = list[i] + normal * offset;
                transform.localPosition = localPosition;
                //最好不要这样来确定列车的方向，不准确
                // transform.LookAt(list[i + 1]); 
                break;
            }
        }
    }
    #endregion    

    #region 测试
    IEnumerator Test()
    {
        float len = 0.0f;
        float totalLen = 0.0f;
        if (TrainType == TrainType.Up)
        {
            totalLen = 451.591125f;
        }
        else if (TrainType == TrainType.Down)
        {
            totalLen = 518.781f;
        }
        while (len < totalLen)
        {
            yield return new WaitForSeconds(0.05f);
            len += 2f;
            if (len > totalLen)
            {
                SetPositionByMarkLength(totalLen);
                yield break;
            }
            else
            {
                SetPositionByMarkLength(len);
            }
        }
    }
    #endregion
}
