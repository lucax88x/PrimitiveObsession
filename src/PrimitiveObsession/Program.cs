using System;
using System.Configuration;
using Autofac;
using PrimitiveObsession.IoC;

namespace PrimitiveObsession
{
    class Program
    {
        static void Main()
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            var builder = new ContainerBuilder();
            builder.RegisterModule(new PrimitiveObsessionModule(connectionString));

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                var noObsession = scope.Resolve<Foo>();

                string connect = noObsession.ConnectionString;
                Console.WriteLine($"connectionString is {connect}");
            }

            Console.ReadLine();
        }
    }
}
