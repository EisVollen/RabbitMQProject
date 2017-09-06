using System;
using System.Threading.Tasks;

namespace NotificationWithUsingRabbitMQ
{
    public interface ISendNotification
    {
        void Send(Guid messageRecipient, string data, int status, Guid toGlobalId, string messageCode);

    }
}
