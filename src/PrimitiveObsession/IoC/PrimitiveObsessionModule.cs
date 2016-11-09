using Autofac;
using PrimitiveObsession.Config;

namespace PrimitiveObsession.IoC
{
    public class PrimitiveObsessionModule : Module
    {
        private readonly short _pistonCount;
        private readonly short _tireCount;

        public PrimitiveObsessionModule(short pistonCount, short tireCount)
        {
            _pistonCount = pistonCount;
            _tireCount = tireCount;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new TireCount(_tireCount));
            builder.Register(x => new PistonCount(_pistonCount));
            builder.RegisterType<EngineBuilder>();
            builder.RegisterType<TireBuilder>();
        }
    }
}