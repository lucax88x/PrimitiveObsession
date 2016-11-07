namespace PrimitiveObsession.Config
{
    public class PistonCount : ConfigurationValue<short>
    {
        public PistonCount(string value)
        {
            Value = short.Parse(value);
        }
    }
}