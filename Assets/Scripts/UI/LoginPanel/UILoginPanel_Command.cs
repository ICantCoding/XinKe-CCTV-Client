
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

//ICommand是不具有SendNotification功能的， 但是SimpleCommand有，所以我们继承SimpleCommand. 
public class UILoginPanel_Command : SimpleCommand
{
    #region 构造函数
    public UILoginPanel_Command()
    {

    }
    #endregion

    #region 重写方法
    public override void Execute(INotification notification)
    {
        switch (notification.Name)
        {
            case EventID_Cmd.U3DClientOnLineSuccess:
                {
                    U3DClientOnLineSuccess_Callback(notification);
                    break;
                }
            case EventID_Cmd.U3DClientOnLineFail:
                {
                    U3DClientOnLineFail_Callback(notification);
                    break;
                }
            case EventID_Cmd.StationClientOnLineSuccess:
            {
                StationClientOnLineSuccess_Callback(notification);
                break;
            }
            case EventID_Cmd.StationClientOnLineFail:
            {
                StationClientOnLineFail_Callback(notification);
                break;
            }
            default:
                break;
        }
    }
    #endregion

    #region 方法
    private void U3DClientOnLineSuccess_Callback(INotification notification)
    {
        SendNotification(EventID_UI.U3DClientOnLineSuccess, null, null);
    }
    private void U3DClientOnLineFail_Callback(INotification notification)
    {
        string reasonMsg = (string)notification.Body;
        SendNotification(EventID_UI.U3DClientOnLineFail, reasonMsg, null);
    }
    private void StationClientOnLineSuccess_Callback(INotification notification)
    {
        SendNotification(EventID_UI.StationClientOnLineSuccess, notification.Body, null);
    }
    private void StationClientOnLineFail_Callback(INotification notification)
    {
        SendNotification(EventID_UI.StationClientOnLineFail, notification.Body, null);
    }
    #endregion
}
