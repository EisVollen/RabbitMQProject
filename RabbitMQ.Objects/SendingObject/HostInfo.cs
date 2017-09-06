
namespace RabbitMQ.Objects.SendingObject
{
    //Взято со стэковерфлоу
    public class HostInfo
    {
        public string MachineName { get; set; }

        public string ProcessName { get; set; }

        public int ProcessId { get; set; }

        public string Assembly { get; set; }

        public string AssemblyVersion { get; set; }

        public string FrameworkVersion { get; set; }

        public string MassTransitVersion { get; set; }

        public string OperatingSystemVersion { get; set; }

    }
}
