
using PureMVC.Interfaces;

namespace PureMVC.Patterns.Observer
{
    public class Notifier: INotifier
    {
        public virtual void SendNotification(string notificationName, object body, string type)
        {
            Facade.SendNotification(notificationName, body, type);
        }
        protected IFacade Facade
        {
            get {
                return Patterns.Facade.Facade.Instance;
            }
        }
    }
}
