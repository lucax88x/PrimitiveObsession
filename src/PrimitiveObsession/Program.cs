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
            using (var container = new EngineContainerBuilder(ConfigurationManager.AppSettings).Build())
            {
                var engineBuilder = container.Resolve<IEngineBuilder>();

                Console.WriteLine(engineBuilder.Build());
            }

            Console.WriteLine("Engine is ready!");
            Console.ReadLine();
        }
    }
}
