namespace PrimitiveObsession.Config
{
    public class ConnectionString
    {
        public string Value { get; }

        public ConnectionString(string value)
        {
            Value = value;
        }

        public static implicit operator string(ConnectionString connectionString)
        {
            return connectionString.Value;
        }

        public static explicit operator ConnectionString(string value)
        {
            return new ConnectionString(value);
        }
    }
}