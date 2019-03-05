using System;
using System.Collections;
using System.Collections.Generic;

public class ResultID
{
    public const UInt16 Success_ResultId = 0;                   //成功
    public const UInt16 U3DClientOnLineFail_ResultId = 10001; //U3D客户端登录失败
}

public class ResultReason
{
    public const string Success_ResultReason = "";
    public const string U3DClientOnLineFail_ResultReason = "已经有相同的ID登录了CCTV服务器!"; //U3D客户端登录失败原因  10001
}
