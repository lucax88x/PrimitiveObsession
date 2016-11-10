using PrimitiveObsession.Config;

namespace PrimitiveObsession
{
    public class Foo
    {
        public Foo(Bar bar, ConnectionString connectionString) { }
    }

    public class Bar { }

    public class Baz
    {
        public Baz(Bar bar, string connectionString) { }
    }

    public class Qux
    {
        public Qux(Bar bar, string url) { }
    }
}