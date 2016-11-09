using System.Collections.Specialized;
using Autofac;

namespace PrimitiveObsession.IoC
{
    public class EngineContainerBuilder
    {
        private readonly NameValueCollection _config;

        public EngineContainerBuilder(NameValueCollection config)
        {
            _config = config;
        }


        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            var pistonCount = _config["PistonCount"];
            var tireCount = _config["TireCount"];
            builder.RegisterModule(new PrimitiveObsessionModule(pistonCount, tireCount));

            return builder.Build();
        }
    }
}
