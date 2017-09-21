using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Consumer.Consumer;

namespace RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
           ConfigureService.Configure();
        }
      
    }
}
