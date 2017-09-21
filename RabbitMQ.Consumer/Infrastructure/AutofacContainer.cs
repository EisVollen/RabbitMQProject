using Autofac;
using RabbitMQ.Consumer.Consumer;

namespace RabbitMQ.Consumer
{
    public class AutofacContainer
    {
        private ContainerBuilder _builder;
        private IContainer _container;

        public IContainer Register()
        {
            _builder = new ContainerBuilder();

            _builder.RegisterType<NotificationTaskStateConsumer>().As<INotificationTaskStateConsumer>();

            _container = _builder.Build();

            return _container;
        }

    }
}