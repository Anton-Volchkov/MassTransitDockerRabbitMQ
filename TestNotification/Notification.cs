using System;

namespace TestNotification
{
    public interface INotification
    {
         long Id { get; set; }
    }

    public class Notification : INotification
    {
        public long Id { get; set; }
    }
}
