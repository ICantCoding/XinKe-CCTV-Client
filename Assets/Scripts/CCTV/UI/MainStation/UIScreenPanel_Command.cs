using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

public class UIScreenPanel_Command : SimpleCommand
{
    #region 构造函数
    public UIScreenPanel_Command()
    {

    }
    #endregion

    #region 重写方法
    public override void Execute(INotification notification)
    {
        switch (notification.Name)
        {
            case EventID_Cmd.DivisionBigScreen:
                {
                    DivisionBigScreen_Callback(notification);
                    break;
                }
            case EventID_Cmd.DivisionSmallScreen:
                {
                    DivisionSmallScreen_Callback(notification);
                    break;
                }
            case EventID_Cmd.ScreenBindCamera:
                {
                    ScreenBindCamera_Callback(notification);
                    break;
                }
            default:
                break;
        }
    }
    #endregion

    #region 方法
    private void DivisionBigScreen_Callback(INotification notification)
    {
        SendNotification(EventID_UI.DivisionBigScreen, notification.Body, null);
    }
    private void DivisionSmallScreen_Callback(INotification notification)
    {
        SendNotification(EventID_UI.DivisionSmallScreen, notification.Body, null);
    }
    private void ScreenBindCamera_Callback(INotification notification)
    {
        SendNotification(EventID_UI.ScreenBindCamera, notification.Body, null);
    }

    #endregion
}
