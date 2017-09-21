using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Consumer
{
    interface INotificationTaskStateConsumer
    {
        Task Consume(string json);
    }
}
