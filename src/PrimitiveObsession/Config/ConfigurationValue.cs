using System;

namespace PrimitiveObsession.Config
{
    public class ConfigurationValue<T> : IEquatable<T>
    {
        public T Value { get; internal set; }

        public bool Equals(T other)
        {
            return other.Equals(this);
        }
    }
}