using Topshelf;

namespace RabbitMQ.Consumer
{
    internal static class ConfigureService
    {
       
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<NotificationService>(service =>
                {
                    service.ConstructUsing(() => new NotificationService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                configure.RunAsLocalService();
                configure.SetServiceName("DT.Docflow.RabbitMQNotificationService");
                configure.SetDisplayName("DT.Docflow.RabbitMQNotificationService");
                configure.SetDescription("Evridoc Service for send Notification through RabbitMQ");
                configure.StartAutomatically(); 
            });
        }
      
    }

   
}
