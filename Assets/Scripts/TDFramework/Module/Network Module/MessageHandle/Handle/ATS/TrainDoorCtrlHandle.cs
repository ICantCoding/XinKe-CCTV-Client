using System;
using System.Collections;
using System.Collections.Generic;

public class TrainDoorCtrlHandle : BaseHandle
{
    #region 构造函数
    public TrainDoorCtrlHandle(BaseNetworkEngine networkEngine) :
        base(networkEngine)
    {
        Name = "TrainDoorCtrlHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
        TrainDoorCtrl response = new TrainDoorCtrl(packet.m_data);
        UInt16 stationIndex = response.m_stationIndex;
        //上行还是下行列车，0表示上行列车，1表示下行列车
        byte upOrDownFalg = response.m_upOrDownFalg;
        //列车左侧车门或列车右侧车门，1右侧,0左侧
        byte leftOrRightDoorFlag = response.m_leftOrRightDoorFlag;
        //屏蔽门开关状态, 1打开,0关闭
        byte statusFlag = response.m_statusFlag;

        //列车开关门动画=============================================================
        StationMgr stationMgr = GameGlobalComponent.StationMgr;
        if (stationMgr == null) return;
        if(upOrDownFalg == 0 && leftOrRightDoorFlag == 0 && statusFlag == 1)
        {
            //上行，左侧，开门
            stationMgr.OpenUpTrainLeftDoors(stationIndex);
        }
        else if(upOrDownFalg == 0 && leftOrRightDoorFlag == 0 && statusFlag == 0)
        {
            //上行，左侧，关门
            stationMgr.CloseUpTrainLeftDoors(stationIndex);
        }
        else if(upOrDownFalg == 0 && leftOrRightDoorFlag == 1 && statusFlag == 1)
        {
            //上行，右侧，开门
            stationMgr.OpenUpTrainRightDoors(stationIndex);
        }
        else if(upOrDownFalg == 0 && leftOrRightDoorFlag == 1 && statusFlag == 0)
        {
            //上行，右侧，关门
            stationMgr.CloseUpTrainRightDoors(stationIndex);
        }
        else if(upOrDownFalg == 1 && leftOrRightDoorFlag == 0 && statusFlag == 1)
        {
            //下行，左侧，开门
            stationMgr.OpenDownTrainLeftDoors(stationIndex);
        }
        else if(upOrDownFalg == 1 && leftOrRightDoorFlag == 0 && statusFlag == 0)
        {
            //上行，左侧，关门
            stationMgr.CloseDownTrainLeftDoors(stationIndex);
        }
        else if(upOrDownFalg == 1 && leftOrRightDoorFlag == 1 && statusFlag == 1)
        {
            //下行，右侧，开门
            stationMgr.OpenDownTrainRightDoors(stationIndex);
        }
        else if(upOrDownFalg == 1 && leftOrRightDoorFlag == 1 && statusFlag == 0)
        {
            //上行，右侧，关门
            stationMgr.CloseDownTrainRightDoors(stationIndex);
        }
    }
    #endregion
}
