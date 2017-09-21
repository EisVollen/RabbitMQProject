using System;

namespace RabbitMQ.Producer
{
    public interface ISendNotification
    {
        void Send(Guid messageRecipient, string data, int status, Guid toGlobalId, string messageCode);

    }
}
