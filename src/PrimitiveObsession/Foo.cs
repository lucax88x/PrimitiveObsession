using PrimitiveObsession.Config;

namespace PrimitiveObsession
{
    public class Foo
    {
        public ConnectionString ConnectionString { get; }

        public Foo(Bar bar, ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}