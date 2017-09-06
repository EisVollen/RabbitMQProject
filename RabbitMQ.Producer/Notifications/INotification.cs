using System;

namespace NotificationWithUsingRabbitMQ.Notifications
{
    public interface ICommand<T>
    {
    }

    //Уведомление которое отправится после успешной отправки шлюзом письма
    //Или в случае провальной доставки
    public interface INotification<T> :ICommand<T>
    {
        /// <summary>
        /// Получатель, который указан в изначальном письме
        /// </summary>
        Guid MessageRecipient { get; set; }

        /// <summary>
        /// Тело сообщения изначального письма
        /// </summary>
        string Data { get; set; }

        /// <summary>
        /// Статус задания
        /// </summary>
        int Status { get; set; }

        /// <summary>
        /// Получатель уведомления
        /// Для изначального письма это отправитель
        /// </summary>
        Guid GlobalId { get; set; }

        /// <summary>
        /// Тип сообщения в изначальном письме
        /// </summary>
        string MessageTypeCode { get; set; }

    }
}
