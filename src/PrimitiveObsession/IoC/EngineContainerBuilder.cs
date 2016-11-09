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

            builder.RegisterModule(new ConfigModule(_config));
            builder.RegisterModule(new MainModule());

            return builder.Build();
        }
    }
}
