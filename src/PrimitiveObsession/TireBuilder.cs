using System.Text;
using PrimitiveObsession.Config;

namespace PrimitiveObsession
{
    public class TireBuilder
    {
        private readonly TireCount _tireCount;

        public TireBuilder(TireCount tireCount)
        {
            _tireCount = tireCount;
        }

        public string Build()
        {
            var sb = new StringBuilder();

            sb.Append("Tires: ");
            for (var i = 0; i < _tireCount; i++) sb.Append("()");

            return sb.ToString();
        }
    }
}