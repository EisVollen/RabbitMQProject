using System;
using System.Collections.Generic;
using MassTransit;
using RabbitMQ.Consumer.Consumer;

namespace RabbitMQ.Consumer
{
    public class NotificationService: IService
    {
      
        private IBusControl _bus;
       
    public void Start()
        {
          
            ConfigureBus();
            _bus.Start();
            Console.WriteLine("NotificationService started");
        }
     

        public void Stop()
        {
            _bus?.Stop();
            Console.WriteLine("NotificationService stoped");
        }

        private static void AutofacRegister()
        {
           // Container = AutofacContainer.RegisterTypes();
        }

        private static void ConfigureNHibernate()
        {

        }

        private static string RemoveStrindAtEnd(List<string> needlessPaths, string fullCurrentPath)
        {
            foreach (var path in needlessPaths)
            {
                if (fullCurrentPath.ToLower().EndsWith(path))
                {
                    return fullCurrentPath.Substring(0, fullCurrentPath.Length - path.Length);
                }
            }
            return fullCurrentPath;
        }

        private void ConfigureBus()
        {
            string hostName = "rabbitmq://localhost/" /*_config.GetValue(Option.ServiceBus.RabbitMQServerAdress)*/;
            int post = 5672              /*Int32.Parse(_config.GetValue(Option.ServiceBus.RabbitMQServerPort))*/;
            string password = "admin"   /*_config.GetValue(Option.ServiceBus.RabbitMQServerPassword)*/;
            string userName = "admin"; /*_config.GetValue(Option.ServiceBus.RabbitMQServerLogin);*/
            string queueName = "TestQueue";/* _config.GetValue(Option.ServiceBus.RabbitMQQueueName);*/
            try
            {
                _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(hostName), h =>
                    {
                        h.Username(userName);
                        h.Password(password);
                    });
                    cfg.ReceiveEndpoint(queueName, e =>
                        {
                            e.UseBinarySerializer();
                            e.Consumer(() => new NotificationTaskStateConsumer());
                        }
                    );
                });
               
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка подключения Comsumerа к очереди: " + e);
                //_logger.Error("Ошибка подключения Comsumerа к очереди", e);
            }
        }
    }
}
