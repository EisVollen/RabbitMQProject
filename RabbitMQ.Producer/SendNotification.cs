using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
//using DT.DocFlow.Utilities.Options;
using MassTransit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using RabbitMQ.Objects.Notification;
using RabbitMQ.Objects.SendingObject;
using HostInfo = RabbitMQ.Objects.SendingObject.HostInfo;

namespace NotificationWithUsingRabbitMQ
{
    public class SendNotification : ISendNotification
    {
        //private readonly IConfig _config;

        public SendNotification()
        {
            //_config = config;

        }

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
            int post = 5672              /*Int32.Parse(_config.GetValue(Option.ServiceBus.RabbitMQServerPort))*/;
            string password = "admin"   /*_config.GetValue(Option.ServiceBus.RabbitMQServerPassword)*/;
            string userName = "admin"; /*_config.GetValue(Option.ServiceBus.RabbitMQServerLogin);*/
            string queueName = "TestQueue";/* _config.GetValue(Option.ServiceBus.RabbitMQQueueName);*/

             var notification = GenerateNotification(messageRecipient, data, status, toGlobalId,
                messageCode);

            //Пытаюсь опубликовать сообщение при помощи Masstransit
            var bus = Bus.Factory.CreateUsingRabbitMq(cnf =>
            {
                var host = cnf.Host(new Uri("rabbitmq://" + hostName), hc =>
                      {
                    hc.Password(password);
                    hc.Username(userName);

                });
            });
            bus.Start();
            
            bus.GetSendEndpoint(new Uri("rabbitmq://" + hostName + "/" + queueName));
            //TODO:Проблема№1: в очереди не не появлется сообщение если использовать MT
            bus.Publish<INotification>(notification);
            bus.Stop();

            //Делает то же самое, что и код сверху, но действительно публикует
            //Готовит сообщение в формате понятному Masstransit при помощи EasyNetQ
            //http://masstransit-project.com/MassTransit/advanced/interoperability.html#example-message
            var notificationJson = JsonConvert.SerializeObject(notification);

            var payload = new LogEntryPayload
            {
                Id = Guid.NewGuid(),
                MessageBody = notificationJson,
                RoutingKey = "",
                LogType = 1

            };

            var message = new SendingObjectMessage
            {
                ConversationId = Guid.NewGuid().ToString(),
                MessageId = Guid.NewGuid().ToString(),
                //TODO:Проблема№1: если использовать EasyQ, то в Consumere Message пустой
                Message = payload,
                DestinationAddress = "rabbitmq:/" + hostName + "/" + queueName,
                Headers = { },
                SourceAddress = "",
                Host = new HostInfo
                {
                    Assembly = "RabbitMQ.Consumer",
                    AssemblyVersion = "1.0",
                    FrameworkVersion = "4.5.2",
                    MassTransitVersion = "3.5.7"
                },
                MessageType = new[]
                {
                    "urn:message:RabbitMQ.Objects.SendingObject:SendingObjectMessage"
                }
            };
            var messageJson = JsonConvert.SerializeObject(message);

            var messageBytes = Encoding.UTF8.GetBytes(messageJson);

            IBasicProperties basicProperties = new BasicProperties
            {
                MessageId = message.MessageId,
                ContentType = "application/vnd.masstransit+json",
                DeliveryMode = 2
            };


            //Создает соединение к очереди и публикует
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = post
            };

            using (var connection = factory.CreateConnection())
            using (var chanel = connection.CreateModel())
            {
                var props = chanel.CreateBasicProperties();
                props.CorrelationId = Guid.NewGuid().ToString();
                chanel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);


                chanel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: basicProperties,
                    body: messageBytes);
            }

        }
    }
}

