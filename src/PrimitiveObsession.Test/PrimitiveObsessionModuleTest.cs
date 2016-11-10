using System;
using Autofac;
using FluentAssertions;
using PrimitiveObsession.Config;
using PrimitiveObsession.IoC;
using Xunit;

namespace PrimitiveObsession.Test
{
    public class PrimitiveObsessionModuleTest : IDisposable
    {
        private readonly IContainer _container;
        private readonly ILifetimeScope _scope;

        public PrimitiveObsessionModuleTest()
        {
            var sut = new PrimitiveObsessionModule(connectionString: "someConnectionString", url: "http://some.url");

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
        public void should_resolve_Foo()
        {
            _scope.Resolve<Foo>().Should().NotBeNull();
        }

        [Fact]
        public void should_resolve_Bar()
        {
            _scope.Resolve<Bar>().Should().NotBeNull();
        }

        [Fact]
        public void should_resolve_Baz()
        {
            _scope.Resolve<Baz>().Should().NotBeNull();
        }

        [Fact]
        public void should_resolve_Qux()
        {
            _scope.Resolve<Qux>().Should().NotBeNull();
        }

        [Fact]
        public void should_resolve_ConnectionString()
        {
            _scope.Resolve<ConnectionString>().Should().NotBeNull();
        }

        [Fact]
        public void connectionString_should_have_the_correct_value()
        {
            var connectionString = _scope.Resolve<ConnectionString>();
            connectionString.Value.Should().Be("someConnectionString");
        }
    }
}
