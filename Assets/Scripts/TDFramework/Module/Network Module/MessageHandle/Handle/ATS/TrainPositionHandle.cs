using System;
using System.Collections;
using System.Collections.Generic;

public class TrainPositionHandle : BaseHandle
{
    #region 构造函数
    public TrainPositionHandle(BaseNetworkEngine networkEngine) :
        base(networkEngine)
    {
        Name = "TrainPositionHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
        TrainPosition response = new TrainPosition(packet.m_data);
        //站台索引
        UInt16 stationIndex = response.m_stationIndex;
        //上行还是下行,0表示上行,1表示下行
        byte upOrDownFlag = response.m_upOrDownFlag;
        //正向行车还是反向行车,0表示正向行车,1表示反向行车, 其实反向没反向并没有什么关系，因为都是依靠对标距离来在曲线上等比计算
        byte positiveOrNegativeDir = response.m_positiveOrNegativeDir;
        //对标距离
        float markDistance = response.m_markDistance;

        StationMgr stationMgr = GameGlobalComponent.StationMgr;
        if (stationMgr == null) return;
        Train train = stationMgr.GetTrain(stationIndex, upOrDownFlag);
        if(train == null) return;
        train.SetPositionByMarkLength(markDistance);
    }
    #endregion
}
