

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using PureMVC.Interfaces;
    using PureMVC.Patterns.Command;

    //ICommand是不具有SendNotification功能的， 但是SimpleCommand有，所以我们继承SimpleCommand. 
    public class Template_Command : SimpleCommand
    {
        #region 构造函数
        public Template_Command()
        {

        }
        #endregion

        #region 重写方法
        public override void Execute(INotification notification)
        {
            if(notification.Name == "Event1")
            {
                UnityEngine.Debug.Log("Template_Command Handle Event1 Message.");
                SendNotification("Event4", null, null);
            }
            else if(notification.Name == "Event2")
            {
                UnityEngine.Debug.Log("Template_Command Handle Event2 Message.");
            }
            else if(notification.Name == "Event3")
            {
                UnityEngine.Debug.Log("Template_Command Handle Event3 Message.");
            }
        }
        #endregion
    }
}