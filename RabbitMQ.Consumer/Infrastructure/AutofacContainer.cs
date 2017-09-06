//using Autofac;
//using DT.DocFlow.ApplicationBase;
//using DT.DocFlow.DataAccess;
//using DT.DocFlow.DataAccess.Impl;
//using DT.DocFlow.ServiceBus.Infrastructure;
//using DT.DocFlow.ServiceBus.Infrastructure.Impl;
//using DT.DocFlow.Utilities.Log;
//using DT.DocFlow.Utilities.Options;
//using DTech.Framework.DataAccess;
//using NotificationRabbitMQClientWithTopshelf.Consumer;

//namespace NotificationRabbitMQClientWithTopshelf
//{
//    public class AutofacContainer : AutofacContainerBase
//    {
       
//        protected override void Register()
//        {
//            _builder.RegisterType<Config>().As<IConfig>();
//            _builder.RegisterType<Logger>().As<ILogger>();

//            RegisterClients();
           
//        }

//        private void RegisterClients()
//        {
//            _builder.RegisterType<SoapServiceInvoker>().As<ISoapServiceInvoker>();
            
//        }
//        protected override void RegisterSessionContext()
//        {
//            _builder.RegisterType<SessionContext>().As<ISessionContext>().SingleInstance();
//        }
//    }
//}