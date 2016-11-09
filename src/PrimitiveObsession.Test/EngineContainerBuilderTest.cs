using Autofac;
using FluentAssertions;
using PrimitiveObsession.Config;
using PrimitiveObsession.IoC;
using Xunit;

namespace PrimitiveObsession.Test
{
    public class EngineContainerBuilderTest
    {
        private readonly IContainer _container;

        public EngineContainerBuilderTest()
        {
            var sut = new PrimitiveObsessionModule("6", "4");

            var builder = new ContainerBuilder();
            builder.RegisterModule(sut);
            _container = builder.Build();
        }

        [Fact]
        public void should_resolve_IEngineBuilder()
        {
            _container.Resolve<IEngineBuilder>().Should().BeOfType<EngineBuilder>();
        }

        [Fact]
        public void should_resolve_ITireBuilder()
        {
            _container.Resolve<ITireBuilder>().Should().BeOfType<TireBuilder>();
        }

        [Fact]
        public void should_resolve_TireCount()
        {
            _container.Resolve<TireCount>().Should().BeOfType<TireCount>();
        }

        [Fact]
        public void should_TireCount_have_correct_value()
        {
            _container.Resolve<TireCount>().Value.Should().Be(4);
        }

        [Fact]
        public void should_resolve_PistonCount()
        {
            _container.Resolve<PistonCount>().Should().BeOfType<PistonCount>();
        }

        [Fact]
        public void should_PistonCount_have_correct_value()
        {
            _container.Resolve<PistonCount>().Value.Should().Be(6);
        }
    }
}
