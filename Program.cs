using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationWithUsingRabbitMQ;

namespace RabbitMQProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write Text and press Enter");
            string data = Console.ReadLine();

            Console.WriteLine("Call Producer");
            new SendNotification().Send(Guid.NewGuid(), data, 3, Guid.NewGuid(), "Test");

            Console.WriteLine("Message Publish");
            Console.ReadLine();

        }
    }
}
