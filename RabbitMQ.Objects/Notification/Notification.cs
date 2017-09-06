using System;

namespace RabbitMQ.Objects.Notification
{
    public class Notification: INotification
    {
        public Guid MessageRecipient { get; set; }
        public string Data { get; set; }
        public int Status { get; set; }
        public Guid GlobalId { get; set; }
        public string MessageTypeCode { get; set; }
        public Guid CorrelationId { get; }
    }
  
}
