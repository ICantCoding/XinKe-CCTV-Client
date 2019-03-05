
using System;
using PureMVC.Interfaces;

namespace PureMVC.Patterns.Observer
{
    public class Observer: IObserver
    {
        public Observer(Action<INotification> notifyMethod, object notifyContext)
        {
            NotifyMethod = notifyMethod;
            NotifyContext = notifyContext;
        }
        public virtual void NotifyObserver(INotification Notification)
        {
            NotifyMethod(Notification);
        }
        public virtual bool CompareNotifyContext(object obj)
        {
            return NotifyContext.Equals(obj);
        }
        public Action<INotification> NotifyMethod { get; set; }
        public object NotifyContext { get; set; }
    }
}
