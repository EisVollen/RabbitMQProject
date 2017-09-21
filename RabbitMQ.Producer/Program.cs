using System;

namespace RabbitMQ.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            for (;;)
            {
                Console.WriteLine("Write text or quit: ");
                var text = Console.ReadLine();
                if (text == "quit")
                    break;

                var sender = new SendNotification();
                sender.Send(Guid.NewGuid(), text, 3, Guid.NewGuid(), "");
                Console.WriteLine("Finish");
            }
            Console.Read();
        }
      
    }
}
