
using PureMVC.Interfaces;

namespace PureMVC.Patterns.Observer
{
    public class Notification: INotification
    {
        public Notification(string name, object body=null, string type=null)
        {
            Name = name;
            Body = body;
            Type = type;
        }
        public override string ToString()
        {
            string msg = "Notification Name: " + Name;
            msg += "\nBody:" + ((Body == null) ? "null" : Body.ToString());
            msg += "\nType:" + ((Type == null) ? "null" : Type);
            return msg;
        }
        public string Name { get; }
        public object Body { get; set; }
        public string Type { get; set; }
    }
}
