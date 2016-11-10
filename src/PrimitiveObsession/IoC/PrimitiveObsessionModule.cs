using Autofac;
using PrimitiveObsession.Config;

namespace PrimitiveObsession.IoC
{
    public class PrimitiveObsessionModule : Module
    {
        private readonly string _connectionString;
        private readonly string _url;

        public PrimitiveObsessionModule(string connectionString, string url)
        {
            _connectionString = connectionString;
            _url = url;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConnectionString(_connectionString));
            builder.RegisterType<Foo>();
            builder.RegisterType<Bar>();

            builder.Register(c =>
            {
                var bar = c.Resolve<Bar>();
                return new Baz(bar, _connectionString);
            });

            builder.RegisterType<Qux>().WithParameter("url", _url);
        }
    }
}
