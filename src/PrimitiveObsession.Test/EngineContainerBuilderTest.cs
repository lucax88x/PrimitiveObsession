using Autofac;
using FluentAssertions;
using PrimitiveObsession.Config;
using PrimitiveObsession.IoC;
using Xunit;

namespace PrimitiveObsession.Test
{
    public class EngineContainerBuilderTest
    {
        private readonly IContainer _sut;

        public EngineContainerBuilderTest()
        {
            _sut = new EngineContainerBuilder().Build(tireCount: "4", pistonCount: "6");
        }

        [Fact]
        public void should_resolve_IEngineBuilder()
        {
            _sut.Resolve<IEngineBuilder>().Should().BeOfType<EngineBuilder>();
        }

        [Fact]
        public void should_resolve_ITireBuilder()
        {
            _sut.Resolve<ITireBuilder>().Should().BeOfType<TireBuilder>();
        }

        [Fact]
        public void should_resolve_TireCount()
        {
            _sut.Resolve<TireCount>().Should().BeOfType<TireCount>();
        }

        [Fact]
        public void should_TireCount_have_correct_value()
        {
            _sut.Resolve<TireCount>().Value.Should().Be(4);
        }

        [Fact]
        public void should_resolve_PistonCount()
        {
            _sut.Resolve<PistonCount>().Should().BeOfType<PistonCount>();
        }

        [Fact]
        public void should_PistonCount_have_correct_value()
        {
            _sut.Resolve<PistonCount>().Value.Should().Be(6);
        }
    }
}
