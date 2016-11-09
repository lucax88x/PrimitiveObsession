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
            var appSettings = ConfigurationManager.AppSettings;
            var pistonCount = short.Parse(appSettings["PistonCount"]);
            var tireCount = short.Parse(appSettings["TireCount"]);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new PrimitiveObsessionModule(tireCount, pistonCount));

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                var engineBuilder = scope.Resolve<EngineBuilder>();

                Console.WriteLine(engineBuilder.Build());
            }

            Console.WriteLine("Engine is ready!");
            Console.ReadLine();
        }
    }
}
