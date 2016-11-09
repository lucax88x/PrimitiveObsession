using Autofac;
using PrimitiveObsession.Config;

namespace PrimitiveObsession.IoC
{
    public class PrimitiveObsessionModule : Module
    {
        private readonly string _pistonCount;
        private readonly string _tireCount;

        public PrimitiveObsessionModule(string pistonCount, string tireCount)
        {
            _pistonCount = pistonCount;
            _tireCount = tireCount;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => { return new TireCount(_tireCount); });
            builder.Register(x => { return new PistonCount(_pistonCount); });
            builder.RegisterType<EngineBuilder>().As<IEngineBuilder>();
            builder.RegisterType<TireBuilder>().As<ITireBuilder>();
        }
    }
}