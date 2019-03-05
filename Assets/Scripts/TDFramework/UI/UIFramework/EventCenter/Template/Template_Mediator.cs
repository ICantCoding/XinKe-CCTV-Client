
/*
PureMVC 模板Mediator
*/
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using PureMVC.Patterns.Mediator;
    using PureMVC.Interfaces;

    public class Template_Mediator : Mediator
    {
        #region 字段
        private static string Name = "Template_Mediator";
        #endregion	

        #region 构造函数
        public Template_Mediator() : this(null, null)
        {
            
        }
        public Template_Mediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
        {
            
        }
        #endregion

        #region 重写方法
        public override string[] ListNotificationInterests()
        {
            return new string[]
            {
                "Event1",
                "Event2",
                "Event3"
            };
        }

        public override void HandleNotification(INotification notification)
        {
            if(notification.Name == "Event1")
            {
                UnityEngine.Debug.Log("Template_Mediator Handle Event1 Message.");
                SendNotification("Event5", null, null);
            }
            else if(notification.Name == "Event2")
            {
                UnityEngine.Debug.Log("Template_Mediator Handle Event2 Message.");
            }
            else if(notification.Name == "Event3")
            {
                UnityEngine.Debug.Log("Template_Mediator Handle Event3 Message.");
            }
        }

        public override void OnRegister()
        {
        }

        public override void OnRemove()
        {
        }
        #endregion
    }
}
