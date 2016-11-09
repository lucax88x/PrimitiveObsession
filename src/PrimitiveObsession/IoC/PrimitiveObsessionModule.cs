using Autofac;
using PrimitiveObsession.Config;

namespace PrimitiveObsession.IoC
{
    public class PrimitiveObsessionModule : Module
    {
        private readonly string _someConnectionString;

        public PrimitiveObsessionModule(string someConnectionString)
        {
            _someConnectionString = someConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new ConnectionString(_someConnectionString));
            builder.RegisterType<Foo>();
            builder.RegisterType<Bar>();
        }
    }
}