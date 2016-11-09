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
            var sut = new PrimitiveObsessionModule(pistonCount: 6, tireCount: 4);

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
        public void should_resolve_EngineBuilder()
        {
            _scope.Resolve<EngineBuilder>().Should().NotBeNull();
        }

        [Fact]
        public void should_resolve_TireBuilder()
        {
            _scope.Resolve<TireBuilder>().Should().NotBeNull();
        }

        [Fact]
        public void should_resolve_TireCount()
        {
            _scope.Resolve<TireCount>().Should().NotBeNull();
        }

        [Fact]
        public void should_TireCount_have_correct_value()
        {
            var tireCount = _scope.Resolve<TireCount>();
            ((short)tireCount).Should().Be(4);
        }

        [Fact]
        public void should_resolve_PistonCount()
        {
            _scope.Resolve<PistonCount>().Should().NotBeNull();
        }

        [Fact]
        public void should_PistonCount_have_correct_value()
        {
            var pistonCount = _scope.Resolve<PistonCount>();
            ((short)pistonCount).Should().Be(6);
        }
    }
}
