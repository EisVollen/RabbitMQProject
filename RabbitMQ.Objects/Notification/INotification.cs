using System;

namespace RabbitMQ.Objects.Notification
{ 
    //Уведомление которое отправится после успешной отправки шлюзом письма
    //Или в случае провальной доставки
    public interface INotification
    {
        /// <summary>
        /// Получатель, который указан в изначальном письме
        /// </summary>
        Guid MessageRecipient { get; set; }

        /// <summary>
        /// Тело сообщения
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

        string MessageTypeCode { get; set; }

    }
}
