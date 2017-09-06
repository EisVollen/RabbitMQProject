using System;

namespace NotificationWithUsingRabbitMQ.Notifications
{
    public class Notification2 /*: INotification<string>*/
    {
        /// <summary>
        /// Получатель, который указан в изначальном письме
        /// </summary>
        public Guid MessageRecipient { get; set; }

        /// <summary>
        /// Тело сообщения изначального письма
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Статус задания
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Получатель уведомления
        /// Для изначального письма это отправитель
        /// </summary>
        public Guid GlobalId { get; set; }

        /// <summary>
        /// Тип сообщения в изначальном письме
        /// </summary>
        public string MessageTypeCode { get; set; }

    }
}