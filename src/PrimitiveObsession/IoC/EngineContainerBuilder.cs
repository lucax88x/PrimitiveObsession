using Autofac;

namespace PrimitiveObsession.IoC
{
    public class EngineContainerBuilder
    {
        public IContainer Build(string tireCount, string pistonCount)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new PrimitiveObsessionModule(pistonCount, tireCount));

            return builder.Build();
        }
    }
}
