using System.Collections.Specialized;
using Autofac;
using PrimitiveObsession.Config;

namespace PrimitiveObsession.IoC
{
    public class PrimitiveObsessionModule : Module
    {
        private readonly NameValueCollection _config;

        public PrimitiveObsessionModule(NameValueCollection config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new TireCount(_config["TireCount"]));
            builder.Register(x => new PistonCount(_config["PistonCount"]));
            builder.RegisterType<EngineBuilder>().As<IEngineBuilder>();
            builder.RegisterType<TireBuilder>().As<ITireBuilder>();

        }
    }
}