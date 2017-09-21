using System;
using System.Text;
using Autofac;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Consumer.Consumer;


namespace RabbitMQ.Consumer
{
    public class NotificationService: IService
    {
        private IContainer _container;
        private INotificationTaskStateConsumer _consumer;
        private AutofacContainer autofacContainer = new AutofacContainer();

        public void Start()
        {
            _container = autofacContainer.Register();
            _consumer = _container.Resolve<INotificationTaskStateConsumer>();
            ConfigureBus();

            Console.WriteLine("NotificationService started");
        }


        public void Stop()
        {
            Console.WriteLine("NotificationService stoped");
        }


        private void ConfigureBus()
        {
            string hostName = "localhost" /*_config.GetValue(Option.ServiceBus.RabbitMQServerAdress)*/;
            int port = 5672              /*Int32.Parse(_config.GetValue(Option.ServiceBus.RabbitMQServerPort))*/;
            string password = "admin"   /*_config.GetValue(Option.ServiceBus.RabbitMQServerPassword)*/;
            string userName = "admin"; /*_config.GetValue(Option.ServiceBus.RabbitMQServerLogin);*/
            string queueName = "TestQueue";/* _config.GetValue(Option.ServiceBus.RabbitMQQueueName);*/
            try
            {

                var factory = new ConnectionFactory
                {
                    UserName = userName,
                    Password = password,
                    HostName = hostName,
                    Port = port
                };

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(
                            queue: queueName,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        var consumer = new EventingBasicConsumer(channel);

                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var json = Encoding.UTF8.GetString(body);
                           _consumer.Consume(json);
                        };

                        channel.BasicConsume(
                            queue: queueName,
                            noAck: true,
                            consumer: consumer);

                        Console.WriteLine("Consumer Start");

                        Console.ReadLine();
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Ошибка подключения Comsumerа к очереди: " + e);
                //_logger.Error("Ошибка подключения Comsumerа к очереди", e);
            }
        }
    }
}
