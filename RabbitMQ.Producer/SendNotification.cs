using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Objects.Notification;
//using DT.DocFlow.Utilities.Options;

namespace RabbitMQ.Producer
{
    public class SendNotification : ISendNotification
    {
        private Notification GenerateNotification(Guid messageRecipient, string data, int status, Guid toGlobalId,
            string messageCode)
        {
            //TODO: Если будет не сложно подскажи, как сковертировать soap в объект
            //В проекте хранится файл с именем SOAPMessage.txt

            // в data хранится в soap формате
            //var xDocument = XDocument.Parse(data);
            //var xMessageBody = xDocument.Descendants(XName.Get("Body", @"http://schemas.xmlsoap.org/soap/envelope/"))
            //    .Descendants()
            //    .FirstOrDefault()
            //    .ToString();

            Notification notification = new Notification()
            {
                MessageRecipient = messageRecipient,
                Data = data,
                Status = status,
                GlobalId = toGlobalId,
                MessageTypeCode = messageCode
            };
            return notification;
        }

        public void Send(Guid messageRecipient, string data, int status, Guid toGlobalId, string messageCode)
        {
            
            string hostName = "localhost" /*_config.GetValue(Option.ServiceBus.RabbitMQServerAdress)*/;
            int port = 5672              /*Int32.Parse(_config.GetValue(Option.ServiceBus.RabbitMQServerPort))*/;
            string password = "admin"   /*_config.GetValue(Option.ServiceBus.RabbitMQServerPassword)*/;
            string userName = "admin"; /*_config.GetValue(Option.ServiceBus.RabbitMQServerLogin);*/
            string queueName = "TestQueue";/* _config.GetValue(Option.ServiceBus.RabbitMQQueueName);*/

             var notification = GenerateNotification(messageRecipient, data, status, toGlobalId,
                messageCode);
            var factory = new ConnectionFactory
            {
                UserName = userName,
                Password = password,
                HostName = hostName,
                Port = port
            };

            using (var connect = factory.CreateConnection())
            {
                using (var model = connect.CreateModel())
                {
                   
                    model.QueueDeclare(queueName, true, false, false, null);
                   byte[] message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notification));
                    model.BasicPublish("", queueName, null, message);

                }
            }
        }
    }
}

