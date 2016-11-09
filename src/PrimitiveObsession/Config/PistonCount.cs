namespace PrimitiveObsession.Config
{
    public class PistonCount
    {
        short Value { get; }

        public PistonCount(short value)
        {
            Value = value;
        }

        public static implicit operator short(PistonCount pistonCount)
        {
            return pistonCount.Value;
        }

        public static explicit operator PistonCount(short value)
        {
            return new PistonCount(value);
        }
    }
}