namespace PrimitiveObsession.Config
{
    public class TireCount : ConfigurationValue<short>
    {
        public TireCount(string value)
        {
            Value = short.Parse(value);
        }
    }
}