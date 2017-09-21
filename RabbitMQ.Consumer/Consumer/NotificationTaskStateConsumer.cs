using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Objects.Notification;
//using DT.DocFlow.ServiceBus.Domain.Endpoints;
//using DT.DocFlow.ServiceBus.Infrastructure;
//using DT.DocFlow.Utilities.Log;
//using DT.DocFlow.Utilities.Options;
//using DTech.Framework.DataAccess;


namespace RabbitMQ.Consumer.Consumer
{
    public class NotificationTaskStateConsumer:INotificationTaskStateConsumer
    {
    //    private readonly IConfig _config;
    //    private readonly IRepository<Endpoint> _repositoryEndpoint;
    //    private readonly ISoapServiceInvoker _soapServiceInvoker;
    //    private ILogger _logger;
    //    private string MessageTypeCode = "notificationTaskState";
     
        public NotificationTaskStateConsumer()
        {
            Console.WriteLine(".cctor NotificationTaskStateConsumer");
            //_config = config;
            //_repositoryEndpoint = repositoryEndpoint;
            //_soapServiceInvoker = soapServiceInvoker;
            //_logger = logger;
            
        }

        public Task Consume(string json)
        {
            Console.WriteLine("Consume NotificationTaskStateConsumer");
           
            var notification = JsonConvert.DeserializeObject<Notification>(json);
            var targedId = notification.GlobalId;
            Console.WriteLine(notification.Data);
            //Достает объект из бд с ссылкой на сервис 
            // var endpoint = GetEndpoint(targedId);
            //var action = endpoint.Actions.First(o => o.MessageType.Code == MessageTypeCode);

            //var notificationXml = CreateNotificationXml(notification);
            //if (notificationXml == null)
            //{
            //    //_logger.Warning($"После сериализии объект остался пустым." +
            //    //                $"GlobalId = {notification.GlobalId}" +
            //    //                $"messageTypeCode = {MessageTypeCode}");
            //    return Task.FromResult(0);
            //}

            //try
            //{
            //    //Поскольку объекта нет, написала данные вручную
            //    CallWebService("http://localhost:8775/CloudServices/ServiceBus/InteropService.svc",
            //        "http://tempuri.org/IInteropService/DepositNotificationTaskState",
            //       notificationXml, Guid.Empty, "", "");
            //}
            //catch (Exception e)
            //{
            ////    _logger.Error("Ошибка при подключении к сервису", e);
            //}
            return Task.FromResult(0);
        }

        private string CreateNotificationXml(Notification notification)
        {

            try
            {
                //TODO:Проблема№2: не могу подобрать верную конвертацию в soap
                using (MemoryStream stream = new MemoryStream())
                {
                    //XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(stream);
                    //DataContractSerializer serializer = new DataContractSerializer(typeof(Notification));
                    //serializer.WriteObject(writer, notification);

                    SoapFormatter formatter = new SoapFormatter();
                    formatter.Serialize(stream, notification);
                    stream.Flush();

                    //XmlSerializer serializer = new XmlSerializer(typeof(Notification));
                    //serializer.Serialize(stream, notification);

                    //SoapFormatter formatter = new SoapFormatter();
                    //formatter.Serialize(stream, notification);

                    return UTF8Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Position);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка сериализации объекта" + e);
                //_logger.Error("Ошибка сериализации объекта", e);
            }
            return string.Empty;
        }

        //TODO: Проблема№2: может проблема с вызовом сервиса, поэтому вынесла код его вызова сюда 
        //Но этот код используется не только мной и со всеми другими - проблем не возникает

        private string CallWebService(string url, string action, string message, Guid targedId, string userName,
            string password)
        {
            var task = CallAsync(url, action, message, targedId, userName, password);

            task.Wait();

            return task.Result;
        }

        private async Task<string> CallAsync(string url, string action, string message, Guid targedId, string userName,
            string password)
        {

            var webRequest = CreateWebRequest(url, action, targedId, userName, password);

            byte[] data = Encoding.UTF8.GetBytes(message);
            webRequest.ContentLength = data.Length;

            using (
                var requestStream =
                    await
                        Task<Stream>.Factory.FromAsync(webRequest.BeginGetRequestStream,
                            webRequest.EndGetRequestStream,
                            webRequest))
            {
                await requestStream.WriteAsync(data, 0, data.Length);
            }

            var responseObject =
                await
                    Task<WebResponse>.Factory.FromAsync(webRequest.BeginGetResponse, webRequest.EndGetResponse,
                        webRequest);
            var responseStream = responseObject.GetResponseStream();
            var sr = new StreamReader(responseStream);
            var received = await sr.ReadToEndAsync();

            return received;
        }


        private HttpWebRequest CreateWebRequest(string url, string action, Guid targedId, string userName,
            string password)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            //action = action.Trim('"');

            webRequest.Headers.Add("SOAPAction", string.Format("\"{0}\"", action));
            webRequest.Headers.Add("GlobalId", targedId.ToString());

            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.KeepAlive = true;
            webRequest.Timeout = 1200000;
            webRequest.ReadWriteTimeout = 1200000;
            webRequest.ContinueTimeout = 1200000;

            return webRequest;
        }

        //private Endpoint GetEndpoint(Guid targetId)
        //{
        //    try
        //    {
        //        var endopoint = _repositoryEndpoint.Query()
        //                            .FirstOrDefault(o =>
        //                                o.GlobalId == targetId &&
        //                                o.Actions.Any(a => a.MessageType.Code == MessageTypeCode) &&
        //                                o.ServiceConfiguration != null)
        //                        ?? _repositoryEndpoint.Query()
        //                            .FirstOrDefault(o =>
        //                                o.GlobalId == Guid.Empty &&
        //                                o.Actions.Any(a => a.MessageType.Code == MessageTypeCode) &&
        //                                o.ServiceConfiguration != null);

        //        return endopoint;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.Error("Ошибка при подключении к точке входа(Endpoint)", e);
        //    }
        //    return null;
        //}
    }
}
