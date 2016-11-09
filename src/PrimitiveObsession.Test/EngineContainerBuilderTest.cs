using System;
using Autofac;
using FluentAssertions;
using PrimitiveObsession.Config;
using PrimitiveObsession.IoC;
using Xunit;

namespace PrimitiveObsession.Test
{
    public class EngineContainerBuilderTest : IDisposable
    {
        private readonly IContainer _container;
        private ILifetimeScope _scope;

        public EngineContainerBuilderTest()
        {
            var sut = new PrimitiveObsessionModule("6", "4");

            var builder = new ContainerBuilder();
            builder.RegisterModule(sut);
            _container = builder.Build();
            _scope = _container.BeginLifetimeScope();
        }

        public void Dispose()
        {
            _scope.Dispose();
            _container.Dispose();
        }

        [Fact]
        public void should_resolve_IEngineBuilder()
        {
            _scope.Resolve<IEngineBuilder>().Should().BeOfType<EngineBuilder>();
        }

        [Fact]
        public void should_resolve_ITireBuilder()
        {
            _scope.Resolve<ITireBuilder>().Should().BeOfType<TireBuilder>();
        }

        [Fact]
        public void should_resolve_TireCount()
        {
            _scope.Resolve<TireCount>().Should().BeOfType<TireCount>();
        }

        [Fact]
        public void should_TireCount_have_correct_value()
        {
            _scope.Resolve<TireCount>().Value.Should().Be(4);
        }

        [Fact]
        public void should_resolve_PistonCount()
        {
            _scope.Resolve<PistonCount>().Should().BeOfType<PistonCount>();
        }

        [Fact]
        public void should_PistonCount_have_correct_value()
        {
            _scope.Resolve<PistonCount>().Value.Should().Be(6);
        }
    }
}
