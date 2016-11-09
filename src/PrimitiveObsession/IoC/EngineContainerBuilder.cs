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

            builder.RegisterModule(new PrimitiveObsessionModule(_config));

            return builder.Build();
        }
    }
}
