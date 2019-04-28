using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageID
{
    public System.UInt16 FirstID;
    public System.UInt16 SecondID;

    public MessageID(System.UInt16 firstId, System.UInt16 secondId)
    {
        this.FirstID = firstId;
        this.SecondID = secondId;
    }
}

public class MessageIDMgr : TDFramework.Singleton<MessageIDMgr>
{
    #region 字段

    //==========================================================================================================
    //U3DClient和U3DServer交互信令===============================================================================
    //U3D客户端登录消息
    public MessageID U3DClientLoginMessageID = new MessageID(0, 0);
    //U3D站台登录消息
    public MessageID StationClientLoginMessageID = new MessageID(0, 1);
    //NpcPosition同步消息
    public MessageID NpcPositionMessageID = new MessageID(0, 2);
    //NpcAnimation同步信令
    public MessageID NpcAnimationMessageID = new MessageID(0, 3);
    //NpcDestroy销毁信令
    public MessageID NpcDestroyMessageId = new MessageID(0, 4);
    //DeviceAction设备动作信令
    public MessageID DeviceActionMessageId = new MessageID(0, 5);
    //客户端重连信令
    public MessageID ClientRelinkMessageId = new MessageID(0, 6);



    //==========================================================================================================
    //ATS和U3DClient交互信令,其实是U3DServer转发ATS信令===========================================================
    //屏蔽门控制信令
    public MessageID PingBiMenCtrlMessageId = new MessageID(10000, 0);
    //Npc人物上下行，上下车信令
    public MessageID NpcCtrlMessageId = new MessageID(10000, 10);
    //Npc任务上下行，上下车完毕信令
    public MessageID NpcCtrlCompleteMessageId = new MessageID(10000, 20);
    //加载列车信令
    public MessageID LoadTrainMessageId = new MessageID(10000, 30);
    //卸载列车信令
    public MessageID UnloadTrainMessageId = new MessageID(10000, 40);
    //定位车信令
    public MessageID TrainPositionMessageId = new MessageID(10000, 50);
    //列车开门信令
    public MessageID TrainDoorCtrlMessageId = new MessageID(10000, 60);



    //==========================================================================================================
    //CCTV和U3DClient交互信令,其实是U3DServer转发CCTV信令=========================================================
    //切割大屏信令
    public MessageID DivisionBigScreenMessageId = new MessageID(20000, 10);
    //切割小屏信令
    public MessageID DivisionSmallScreenMessageId = new MessageID(20000, 20);
    //屏幕绑定摄像机信令
    public MessageID ScreenBindCameraMessageId = new MessageID(20000, 30);
    #endregion
}
