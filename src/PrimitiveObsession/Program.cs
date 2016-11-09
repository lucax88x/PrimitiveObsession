using System;
using System.Collections.Specialized;
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
            var pistonCount = appSettings["PistonCount"];
            var tireCount = appSettings["TireCount"];

            using (var container = new EngineContainerBuilder().Build(pistonCount, tireCount))
            {
                var engineBuilder = container.Resolve<IEngineBuilder>();

                Console.WriteLine(engineBuilder.Build());
            }

            Console.WriteLine("Engine is ready!");
            Console.ReadLine();
        }
    }
}
