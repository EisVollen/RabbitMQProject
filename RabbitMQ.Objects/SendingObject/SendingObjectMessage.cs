using System;
using System.Collections.Generic;

namespace RabbitMQ.Objects.SendingObject
{
    //Masstransit через жопу работает с объектами в очереди отправленые не через него.
    //Надстройка для правильной сериализацией. 
    //Взято со стэковерфлоу
    public class SendingObjectMessage
    {
        public string DestinationAddress { get; set; }

        public string MessageId { get; set; }

        public string ConversationId { get; set; }

        public string SourceAddress { get; set; }

        public HostInfo Host { get; set; }

        public IDictionary<string, object> Headers { get; set; }

        public LogEntryPayload Message { get; set; }

        public string[] MessageType { get; set; }
    }
    public class LogEntryPayload : IMessage
    {
        public Guid Id { get; set; }

        public string MessageBody { get; set; }

        public int LogType { get; set; }

        public string RoutingKey { get; set; }
    }
}
