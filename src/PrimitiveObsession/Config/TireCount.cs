namespace PrimitiveObsession.Config
{
    public class TireCount
    {
        short Value { get; }

        public TireCount(short value)
        {
            Value = value;
        }

        public static implicit operator short(TireCount TireCount)
        {
            return TireCount.Value;
        }

        public static explicit operator TireCount(short value)
        {
            return new TireCount(value);
        }
    }
}