using System;

namespace RabbitMQ.Objects.SendingObject
{
    //Взято со стэковерфлоу
    public interface IMessage
    {
        Guid Id { get; set; }

        string RoutingKey { get; set; }

        string MessageBody { get; set; }
    }
}
